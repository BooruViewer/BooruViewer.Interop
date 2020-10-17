using System;
using BooruViewer.Interop.Boorus.Abstract;
using BooruViewer.Interop.Dtos.Booru;
using BooruViewer.Interop.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace BooruViewer.Interop.Boorus
{
    public class KonaChan: Moebooru
    {
        private static SourceBooru _sourceBooru = new SourceBooru("konachan", "Konachan", new Uri("https://konachan.com/"));

        // ReSharper disable once SuggestBaseTypeForParameter
        public KonaChan(IKonachanApi api, IDistributedCache cache) : base(api, _sourceBooru, cache)
        { }
    }
}
