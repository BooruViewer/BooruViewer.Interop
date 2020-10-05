using System;
using System.Collections.Generic;

namespace BooruViewer.Interop.Dtos.Danbooru
{
    public class RelatedTags
    {
        public String Query { get; set; }
        public TagType? Category { get; set; }

        public List<List<Object>> Tags { get; set; }
//        public List<List<Object>> WikiPageTags { get; set; }
    }
}
