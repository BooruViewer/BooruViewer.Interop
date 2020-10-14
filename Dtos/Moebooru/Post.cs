using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BooruViewer.Interop.Dtos.Moebooru
{
    public class Post
    {
        public UInt32 Id { get; set; }

        public UInt32? ParentId { get; set; }

        public String Tags { get; set; }
        public String Md5 { get; set; }

        public Int32 Score { get; set; }

        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime CreatedAt { get; set; }
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime? UpdatedAt { get; set; }
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime? LastNotedAt { get; set; }
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime? LastCommentedAt { get; set; }

        public UInt32 CreatorId { get; set; }
        public UInt32? ApproverId { get; set; }

        // Uploader
        public String Author { get; set; }

        public Int32 Change { get; set; }

        public String Source { get; set; }

        public String FileUrl { get; set; }
        public UInt32 Width { get; set; }
        public UInt32 Height { get; set; }
        public UInt32 FileSize { get; set; }

        public String PreviewUrl { get; set; }
        public UInt32 PreviewWidth { get; set; }
        public UInt32 PreviewHeight { get; set; }
        public UInt32 ActualPreviewWidth { get; set; }
        public UInt32 ActualPreviewHeight { get; set; }

        public String SampleUrl { get; set; }
        public UInt32 SampleWidth { get; set; }
        public UInt32 SampleHeight { get; set; }
        public UInt32 SampleFileSize { get; set; }

        public String JpegUrl { get; set; }
        public UInt32 JpegWidth { get; set; }
        public UInt32 JpegHeight { get; set; }
        public UInt32 JpegFileSize { get; set; }

        public String Rating { get; set; }

        public String Status { get; set; }

        public Boolean HasChildren { get; set; }

        public Boolean IsPending { get; set; }
        public Boolean IsNoteLocked { get; set; }
        public Boolean IsHeld { get; set; }
        public Boolean IsRatingLocked { get; set; }
        public Boolean IsShownInIndex { get; set; }

        public String IsPendingString { get; set; }
        public Object FramePending { get; set; }
        public Object FrameString { get; set; }
        public Object Frames { get; set; }

        public Object FlagDetail { get; set; }
    }
}
