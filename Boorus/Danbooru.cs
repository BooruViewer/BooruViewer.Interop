using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BooruViewer.Interop.Dtos.Booru;
using BooruViewer.Interop.Dtos.Booru.Posts;
using BooruViewer.Interop.Dtos.Danbooru;
using BooruViewer.Interop.Interfaces;
using Refit;
using BooruPost = BooruViewer.Interop.Dtos.Booru.Posts.Post;
using Post = BooruViewer.Interop.Dtos.Danbooru.Post;

namespace BooruViewer.Interop.Boorus
{
    public class Danbooru : IBooru
    {
        private IDanbooruApi _api;
        private static SourceBooru _sourceBooru;

        public SourceBooru Booru => _sourceBooru;

        static Danbooru()
        {
            _sourceBooru = new SourceBooru("danbooru", "Danbooru", new Uri("https://danbooru.donmai.us/"));
        }

        public Danbooru(IDanbooruApi api)
        {
            this._api = api;
        }

        public async Task<IEnumerable<BooruPost>> GetPostsAsync(String tags, UInt64 page = 1, UInt64 limit = 100)
        {
            if (page == 0)
                throw new ArgumentException($"{nameof(page)} cannot be zero.", nameof(page));
            if (limit == 0)
                throw new ArgumentException($"{nameof(limit)} cannot be zero.", nameof(limit));

            // TODO: Support authentication?
            var posts = await this._api.GetPostsAsync(tags, page, limit);

            return posts.Select(p => this.MapPost(p));
        }

        private BooruPost MapPost(Post danbooru)
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

            static Files GetFilesFromSource(Post db)
            {
                if (String.IsNullOrWhiteSpace(db.FileUrl))
                    return null;

                var previewUrl = db.LargeFileUrl;
                var originalUrl = db.FileUrl;
                if (db.HasLarge.HasValue && !db.HasLarge.Value)
                    previewUrl = originalUrl;

                var files = new Files(db.PreviewFileUrl, previewUrl, originalUrl, db.FileSize);

                if (!db.HasLarge.HasValue && files.IsVideo)
                    files.Preview = db.PreviewFileUrl; // It's a small thumbnail, but best we can do for videos

                return files;
            }
            static Size GetSizeFromSource(UInt64 width, UInt64 height)
                => new Size(width, height);
            Uploader GetUploaderFromSource(Post db)
                => new Uploader(db.UploaderName, $"{this.Booru.BaseUri}users/{db.UploaderId}");
            static Source GetSourceFromSource(Post db)
            {
                if (!Uri.TryCreate(db.Source, UriKind.Absolute, out var sauce))
                    return String.IsNullOrWhiteSpace(db.Source) ? null : new Source(db.Source, null);
                return new Source(sauce.Host, sauce.ToString());
            }
            static Func<String, Tag> GetTagFromString(TagTypes type)
                => tag => new Tag(tag, type);
            static String[] Split(String str)
                => str?.Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? new String[0];

            var post = new BooruPost();
            post.Id = danbooru.Id;
            post.ParentId = danbooru.ParentId;
            // TODO: Make runtime safe
            if (!String.IsNullOrWhiteSpace(danbooru.ChildrenIds))
                post.ChildIds = Split(danbooru.ChildrenIds)
                    .Select(s => UInt64.Parse(s)).ToList();

            post.IsVisible = !String.IsNullOrWhiteSpace(danbooru.FileUrl);
            post.IsPending = danbooru.IsPending;
            post.IsDeleted = danbooru.IsDeleted;

            post.HasNotes = danbooru.LastNotedAt.HasValue;
            post.HasSound = danbooru.TagString.Contains("video_with_sound") ||
                            danbooru.TagString.Contains("flash_with_sound");

            post.Hash = danbooru.Md5;

            post.Score = danbooru.Score;
            post.Favorites = danbooru.FavCount;
            post.IsFavorited = danbooru.IsFavorited;

            post.UploadedAt = danbooru.CreatedAt;
            post.LastModifiedAt = danbooru.UpdatedAt;

            post.Rating = GetRatingFromSource(danbooru.Rating);
            post.Files = GetFilesFromSource(danbooru);
            post.Size = GetSizeFromSource(danbooru.ImageWidth, danbooru.ImageHeight);
            post.Uploader = GetUploaderFromSource(danbooru);
            post.Source = GetSourceFromSource(danbooru);

            post.Tags = Split(danbooru.TagStringCopyright).Select(GetTagFromString(TagTypes.Copyright))
                .Union(Split(danbooru.TagStringCharacter).Select(GetTagFromString(TagTypes.Character)))
                .Union(Split(danbooru.TagStringArtist).Select(GetTagFromString(TagTypes.Artist)))
                .Union(Split(danbooru.TagStringGeneral).Select(GetTagFromString(TagTypes.General)))
                .Union(Split(danbooru.TagStringMeta).Select(GetTagFromString(TagTypes.Meta)))
                .ToList();

            return post;
        }
    }
}
