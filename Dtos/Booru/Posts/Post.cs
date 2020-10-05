using System;
using System.Collections.Generic;

namespace BooruViewer.Interop.Dtos.Booru.Posts
{
    public class Post
    {
        public UInt64 Id { get; set; }

        public UInt64? ParentId { get; set; }

        // TODO: Add a ICollection<PostDto> for the child posts?
        public ICollection<UInt64> ChildIds { get; set; }

        public Boolean HasChildren => this.ChildIds.Count > 0;

        public Boolean IsVisible { get; set; }
        public Boolean IsPending { get; set; }
        public Boolean IsDeleted { get; set; }

        public Boolean? HasNotes { get; set; }
        public Boolean HasSound { get; set; }

        public String Hash { get; set; }

        public Int64? Score { get; set; }
        public UInt64 Favorites { get; set; }
        public Boolean IsFavorited { get; set; }

        public DateTimeOffset UploadedAt { get; set; }
        public DateTimeOffset? LastModifiedAt { get; set; }

        public Rating Rating { get; set; }

        public Files Files { get; set; }
        public Size Size { get; set; }
        public Uploader Uploader { get; set; }
        public Source Source { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public Dictionary<String, Object> BooruData { get; set; }

        public Post()
        {
            this.ChildIds = new List<UInt64>();
            this.Tags = new List<Tag>();
            this.BooruData = new Dictionary<String, Object>();
        }
    }
}
