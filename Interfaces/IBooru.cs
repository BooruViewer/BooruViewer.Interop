using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BooruViewer.Interop.Dtos.Booru;
using BooruViewer.Interop.Dtos.Booru.Posts;

namespace BooruViewer.Interop.Interfaces
{
    public interface IBooru
    {

        SourceBooru Booru { get; }

        Task<IEnumerable<Post>> GetPostsAsync(String tags, UInt64 page = 1, UInt64 limit = 100);

        IBooru WithAuthentication(String username, String password);
    }
}
