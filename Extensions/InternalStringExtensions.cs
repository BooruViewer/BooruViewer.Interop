using System;

namespace BooruViewer.Interop.Extensions
{
    internal static class InternalStringExtensions
    {
        public static Boolean IsEmpty(this String s)
            => String.IsNullOrWhiteSpace(s);
    }
}
