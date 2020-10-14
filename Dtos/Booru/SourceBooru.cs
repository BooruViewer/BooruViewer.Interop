using System;

namespace BooruViewer.Interop.Dtos.Booru
{
    public class SourceBooru
    {
        public Uri BaseUri { get; set; }
        public String Name { get; set; }
        public String Identifier { get; set; }

        public SourceBooru(String id, String name, Uri baseUri)
        {

        }
    }
}
