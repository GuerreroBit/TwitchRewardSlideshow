using System;
using System.Collections.Generic;
using System.IO;
using SQLite;

namespace TwitchRewardSlideshow.DataBase {
    public class ImageBuffer {
        public Queue<ImageInfo> toCheckImagesQueue { get; set; } = new();
        public Queue<ImageInfo> exclusiveImagesQueue { get; set; } = new();
        public List<ImageInfo> activeImages { get; set; } = new();
        public List<ImageInfo> displayedImages { get; set; } = new();
        public List<ImageInfo> defaultImages { get; set; } = new();
        public List<ImageInfo> displayedDefaultImages { get; set; } = new();
        public ImageInfo activeExclusiveImage = null;
    }

    public class ImageInfo : IComparable<ImageInfo> {
        [PrimaryKey] public string id => Path.GetFileName(path);
        [AutoIncrement] public int index { get; set; }
        public string path { get; set; }
        public bool exclusive { get; set; }
        public double totalActiveTime { get; set; }
        public double usedTime { get; set; }
        public string downloadLink { get; set; }
        public string user { get; set; }
        public string rewardId { get; set; }
        public string redemptionId { get; set; }
        
        public bool toCheckImageQueue { get; set; }
        public bool exclusiveImageQueue { get; set; }
        public bool activeImage { get; set; }
        public bool displayedImage { get; set; }
        public bool defaultImage { get; set; }
        public bool displayedDefaultImage { get; set; }
        public bool isActiveExclusiveImage { get; set; }

        public ImageInfo() {}

        public ImageInfo(bool exclusive, double totalActiveTime, string downloadLink) {
            this.exclusive = exclusive;
            this.totalActiveTime = totalActiveTime;
            this.downloadLink = downloadLink;
        }

        public int CompareTo(ImageInfo other) {
            if (index > other.index) return 1;
            if (index < other.index) return -1;
            return 0;
        }

        public override bool Equals(object obj) {
            return obj is ImageInfo item && id.Equals(item.id);
        }

        public override int GetHashCode() {
            return HashCode.Combine(id);
        }
    }
}