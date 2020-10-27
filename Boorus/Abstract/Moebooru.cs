using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BooruViewer.Interop.Dtos.Booru;
using BooruViewer.Interop.Dtos.Booru.Posts;
using BooruViewer.Interop.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using BooruPost = BooruViewer.Interop.Dtos.Booru.Posts.Post;
using Post = BooruViewer.Interop.Dtos.Moebooru.Post;

namespace BooruViewer.Interop.Boorus.Abstract
{
    public abstract class Moebooru : IBooru
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0);
        private static readonly TagTypes[] TagsOrder = new[]
            {TagTypes.Copyright, TagTypes.Character, TagTypes.Artist, TagTypes.General, TagTypes.Meta};

        private readonly IMoebooruApi _api;

        private SourceBooru _sourceBooru;
        private readonly IDistributedCache _cache;

        public SourceBooru Booru => this._sourceBooru;

        public Moebooru(IMoebooruApi api, SourceBooru sourceBooru, IDistributedCache cache)
        {
            this._api = api;
            this._sourceBooru = sourceBooru;
            this._cache = cache;
        }

        /// <summary>Not implemented</summary>
        public IBooru WithAuthentication(String username, String password)
        {
            // throw new NotImplementedException();
            return this;
        }

        public async Task<IEnumerable<BooruPost>> GetPostsAsync(String tags, UInt64 page = 1, UInt64 limit = 100)
        {
            if (page == 0)
                throw new ArgumentException($"{nameof(page)} cannot be zero.", nameof(page));
            if (limit == 0)
                throw new ArgumentException($"{nameof(limit)} cannot be zero.", nameof(limit));

            var posts = await this._api.GetPostsAsync(tags, page, limit);
            var booruPosts = new List<BooruPost>();

            foreach (var post in posts)
            {
                booruPosts.Add(await this.MapPost(post));
            }

            return booruPosts;
        }

        private async Task<BooruPost> MapPost(Post moebooru)
        {
            static Rating GetRatingFromSource(String rating)
            {
                return rating switch
                {
                    "s" => Rating.Safe,
                    "q" => Rating.Questionable,
                    // Treat all values that are not s or q as explicit for safety sakes.
                    _ => Rating.Explicit
                };
            }
            static Files GetFilesFromSource(Post mb)
            {
                // ReSharper disable once ConvertIfStatementToReturnStatement
                if (String.IsNullOrWhiteSpace(mb.FileUrl))
                    return null;

                return new Files(mb.PreviewUrl, mb.SampleUrl, mb.FileUrl, mb.FileSize);
            }
            static Size GetSizeFromSource(UInt64 width, UInt64 height)
                => new Size(width, height);
            Uploader GetUploaderFromSource(Post db)
                => new Uploader(db.Author, $"{this.Booru.BaseUri}user/show/{db.CreatorId}");
            static Source GetSourceFromSource(Post mb)
            {
                if (!Uri.TryCreate(mb.Source, UriKind.Absolute, out var sauce))
                    return String.IsNullOrWhiteSpace(mb.Source) ? null : new Source(mb.Source, null);
                return new Source(sauce.Host, sauce.ToString());
            }

            Func<String, Tag> GetTagFromString()
            {

                return null;
            }

            static String[] Split(String str)
                => str?.Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? new String[0];

            var post = new BooruPost();
            post.Id = moebooru.Id;
            post.ParentId = moebooru.ParentId;

            post.IsVisible = true; // ?? Do we get not-visible posts from the api?
            post.IsPending = moebooru.IsPending;
            post.IsDeleted = false; // Can we view deleted?

            post.HasNotes = moebooru.LastNotedAt != UnixEpoch;
            post.HasSound = false; // Does anything have sound?

            post.Hash = moebooru.Md5;

            post.Score = moebooru.Score;
            post.Favorites = 0;
            post.IsFavorited = false;

            post.UploadedAt = moebooru.CreatedAt;
            post.LastModifiedAt = moebooru.UpdatedAt;

            post.Rating = GetRatingFromSource(moebooru.Rating);
            post.Files = GetFilesFromSource(moebooru);
            post.Size = GetSizeFromSource(moebooru.Width, moebooru.Height);
            post.Uploader = GetUploaderFromSource(moebooru);
            post.Source = GetSourceFromSource(moebooru);

            post.Tags = await this.GetTagsListAsync(moebooru.Tags);

            return post;
        }

        private async Task<List<Tag>> GetTagsListAsync(String tags)
        {
            var tagObjs = new List<Tag>();

            var tagStrings = tags.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var tagMap = await this.GetTagsMapAsync();
            var hasForced = false;

            foreach (var tagString in tagStrings)
            {
                if (!tagMap.ContainsKey(tagString))
                {
                    if (hasForced)
                        continue; // Skip this tag, we don't care about it :D

                    await this.GetTagsMapAsync(true);
                    hasForced = true;
                }

                tagObjs.Add(new Tag(tagString, tagMap[tagString]));
            }


            return tagObjs
                .OrderBy(t => Array.IndexOf(TagsOrder, t.Type))
                .ThenBy(t => t.Name)
                .ToList();
        }

        private async Task<Dictionary<String, TagTypes>> GetTagsMapAsync(Boolean forceDownload = false)
        {
            var cachedTagBytes = await this._cache.GetAsync($"moebooru__{this.GetType().Name}_tags");
            if (cachedTagBytes?.Length >= 0 && !forceDownload)
            {
                return JsonConvert.DeserializeObject<Dictionary<String, TagTypes>>(Encoding.UTF8.GetString(cachedTagBytes));
            }

            var tags = await this._api.GetTagsAsync();

            TagTypes Convert(Int32 type)
            {
                return type switch
                {
                    1 => TagTypes.Artist,
                    6 => TagTypes.Artist,
                    3 => TagTypes.Copyright,
                    4 => TagTypes.Character,
                    5 => TagTypes.Meta,
                    _ => TagTypes.General // 0,
                };
            }

            var dict = tags.ToDictionary(tag => tag.Name, tag => Convert(tag.Type));

            await this._cache.SetAsync($"moebooru__{this.GetType().Name}_tags",
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dict)));

            return dict;
        }
    }
}
