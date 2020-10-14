using System;
using BooruViewer.Interop.Boorus.Abstract;
using BooruViewer.Interop.Dtos.Booru;
using BooruViewer.Interop.Interfaces;

namespace BooruViewer.Interop.Boorus
{
    public class KonaChan: Moebooru
    {
        private static SourceBooru _sourceBooru = new SourceBooru("konachan", "Konachan", new Uri("https://konachan.com/"));

        public override SourceBooru Booru => _sourceBooru;

        // ReSharper disable once SuggestBaseTypeForParameter
        public KonaChan(IKonachanApi api) : base(api)
        { }
    }
}
