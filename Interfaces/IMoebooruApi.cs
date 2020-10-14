using System;
using System.Threading.Tasks;
using BooruViewer.Interop.Dtos.Moebooru;
using Refit;

namespace BooruViewer.Interop.Interfaces
{
    public interface IMoebooruApi
    {
        [Get("/tag.json")]
        Task<Tag[]> GetTagsAsync(UInt64 limit = 0, UInt64 page = 1, [AliasAs("order")] ReqTagOrder tagOrder = ReqTagOrder.Date);

        [Get("/post.json")]
        Task<Post[]> GetPostsAsync(String tags, UInt64 page = 1, UInt64 limit = 40);
    }

    public interface IYandereApi : IMoebooruApi
    {
        [Get("/404.json")]
        Task MakeYandereExist();
    }

    public interface IKonachanApi : IMoebooruApi
    {
        [Get("/404.json")]
        Task MakeKonachanExist();
    }
}
