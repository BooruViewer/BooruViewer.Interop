using System;

namespace BooruViewer.Interop.Dtos.Booru.Posts
{
    public class Source
    {
        public String FriendlyName { get; set; }
        public String Href { get; set; }

        public Source(String friendlyName, String href)
        {
            this.FriendlyName = friendlyName;
            this.Href = href;
        }
    }
}
