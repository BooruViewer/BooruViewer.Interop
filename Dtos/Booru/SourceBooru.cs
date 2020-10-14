using System;

namespace BooruViewer.Interop.Dtos.Booru
{
    public class SourceBooru
    {
        public Uri BaseUri { get; }
        public String Name { get; }
        public String Identifier { get; }

        public SourceBooru(String id, String name, Uri baseUri)
        {
            this.Identifier = id;
            this.Name = name;
            this.BaseUri = baseUri;
        }
    }
}
