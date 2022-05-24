﻿using System;
using System.Collections.Generic;
using System.IO;

namespace TwitchRewardSlideshow.Configuration {
    public class ImageBuffer : AppConfiguration.Configuration {
        public Queue<ImageInfo> toCheckImages { get; set; } = new();
        public Queue<ImageInfo> exclusiveImagesQueue { get; set; } = new();
        public List<ImageInfo> activeImages { get; set; } = new();
        public List<ImageInfo> displayedImages { get; set; } = new();
        public List<ImageInfo> defaultImages { get; set; } = new();
        public List<ImageInfo> displayedDefaultImages { get; set; } = new();
        public ImageInfo activeExclusiveImage = null;
    }

    public class ImageInfo {
        public string id => Path.GetFileName(path);
        public string path { get; set; }
        public bool exclusive { get; set; }
        public float totalActiveTime { get; set; }
        public float usedTime { get; set; }
        public string downloadLink { get; }
        public string user { get; set; }

        public ImageInfo(bool exclusive, float totalActiveTime, string downloadLink) {
            this.exclusive = exclusive;
            this.totalActiveTime = totalActiveTime;
            this.downloadLink = downloadLink;
        }

        internal void MovePath(string destinationDirectory) {
            Directory.CreateDirectory(destinationDirectory);
            string destination = Path.Combine(destinationDirectory, Path.GetFileName(path)!);
            File.Move(path!, destination);
            path = destination;
        }

        public override bool Equals(object obj) {
            return obj is ImageInfo item && id.Equals(item.id);
        }

        public override int GetHashCode() {
            return HashCode.Combine(id, downloadLink);
        }
    }
}