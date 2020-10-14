using System;
using System.Runtime.CompilerServices;
using BooruViewer.Interop.Boorus.Abstract;
using BooruViewer.Interop.Dtos.Booru;
using BooruViewer.Interop.Interfaces;

namespace BooruViewer.Interop.Boorus
{
    public class Yandere : Moebooru
    {
        private static SourceBooru _sourceBooru = new SourceBooru("yandere", "Yande.re", new Uri("https://yande.re/"));

        // ReSharper disable once SuggestBaseTypeForParameter
        public Yandere(IYandereApi api) : base(api, _sourceBooru)
        { }
    }
}
