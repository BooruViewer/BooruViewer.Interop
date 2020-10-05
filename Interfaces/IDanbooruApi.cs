using System;
using System.Net.Http;
using System.Threading.Tasks;
using BooruViewer.Interop.Dtos.Danbooru;
using Refit;

namespace BooruViewer.Interop.Interfaces
{
    /// <summary>
    /// The interface for the Refit Implementation
    /// </summary>
    public interface IDanbooruApi
    {
        [Get("/posts.json")]
        Task<Post[]> GetPostsAsync(String tags, UInt64 page, UInt64 limit, [Header("Authorization")] String authorization = null);
        [Get("/tags/autocomplete.json")]
        Task<AutoComplete[]> GetAutocompleteAsync([AliasAs("search[name_matches]")] String tags, [Header("Authorization")] String authorization = null);
        [Post("/favorites.json")]
        Task<Post> AddFavorite([AliasAs("post_id")] UInt64 postId, [Header("Authorization")] String authorization = null); // returns post when not already favorited, otherwise errors.
        [Delete("/favorites/{postId}.json")]
        Task RemoveFavorite(UInt64 postId, [Header("Authorization")] String authorization = null); // Expects 204 no content, returns 204 when id isn't favorited.
        [Get("/notes.json")]
        Task<Note[]> GetNotesByIdAsync([AliasAs("search[post_id]")] UInt64 postId, UInt64 limit = 1000, [AliasAs("group_by")] String groupBy = "note");
        [Get("/profile.json")]
        Task<Profile> GetProfile([Header("Authorization")] String authorization = null);
        [Get("/related_tag.json")]
        Task<RelatedTags> GetRelatedTags(String query);
        [Get("/saved_searches.json?limit=1000")]
        Task<SavedSearches[]> GetSavedSearches([Header("Authorization")] String authorization = null);
        [Get("/{**path}")]
        Task<HttpContent> GetImageAsync(String path);
    }
}
