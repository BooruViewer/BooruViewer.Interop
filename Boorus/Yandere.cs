using System;
using System.Runtime.CompilerServices;
using BooruViewer.Interop.Boorus.Abstract;
using BooruViewer.Interop.Dtos.Booru;
using BooruViewer.Interop.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace BooruViewer.Interop.Boorus
{
    public class Yandere : Moebooru
    {
        private static SourceBooru _sourceBooru = new SourceBooru("yandere", "Yande.re", new Uri("https://yande.re/"));

        // ReSharper disable once SuggestBaseTypeForParameter
        public Yandere(IYandereApi api, IDistributedCache cache) : base(api, _sourceBooru, cache)
        { }
    }
}
