using System;

namespace BooruViewer.Interop.Dtos.Booru.Posts
{
    public class Uploader
    {
        public String FriendlyName { get; set; }
        public String Href { get; set; }

        public Uploader(String friendlyName, String href)
        {
            this.FriendlyName = friendlyName;
            this.Href = href;
        }
    }
}
