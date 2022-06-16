using System.Collections.Generic;
using SQLite;

namespace TwitchRewardSlideshow.DataBase {
    public class ImageBufferManager {
        private readonly string _dbPath;
        private readonly SQLiteConnection _dbConnection;

        public ImageBufferManager(string dbPath) {
            _dbPath = dbPath;
            _dbConnection = new SQLiteConnection(dbPath);
            _dbConnection.CreateTable<ImageInfo>();
        }

        public void DeleteImage(string id) {
            _dbConnection.Delete<ImageInfo>(id);
        }

        public void DeleteImage(ImageInfo imageInfo) {
            DeleteImage(imageInfo.id);
        }

        public void InsetToCheckImage(ImageInfo imageInfo) {
            imageInfo.toCheckImageQueue = true;
            _dbConnection.Insert(imageInfo);
        }

        public List<ImageInfo> GetAllToCheckImages() {
            List<ImageInfo> images = _dbConnection.Table<ImageInfo>().Where(x => x.toCheckImageQueue == false).ToList();
            images.Sort();
            return images;
        }

        public void InsetExclusiveImageQueue(ImageInfo imageInfo) {
            imageInfo.exclusiveImageQueue = true;
            _dbConnection.Insert(imageInfo);
        }

        public List<ImageInfo> GetAllExclusiveImageQueue() {
            List<ImageInfo> images = _dbConnection.Table<ImageInfo>().Where(x => x.exclusiveImageQueue == false)
                                                  .ToList();
            images.Sort();
            return images;
        }

        public void InsetActiveImage(ImageInfo imageInfo) {
            imageInfo.activeImage = true;
            _dbConnection.Insert(imageInfo);
        }

        public List<ImageInfo> GetAllActiveImages() {
            List<ImageInfo> images = _dbConnection.Table<ImageInfo>().Where(x => x.exclusiveImageQueue == false)
                                                  .ToList();
            images.Sort();
            return images;
        }

        public void InsertDisplayedImage(ImageInfo imageInfo) {
            imageInfo.displayedImage = true;
            _dbConnection.Insert(imageInfo);
        }

        public List<ImageInfo> GetAllDisplayedImages() {
            List<ImageInfo> images = _dbConnection.Table<ImageInfo>().Where(x => x.exclusiveImageQueue == false)
                                                  .ToList();
            images.Sort();
            return images;
        }

        public void InsertDefaultImage(ImageInfo imageInfo) {
            imageInfo.defaultImage = true;
            _dbConnection.Insert(imageInfo);
        }

        public List<ImageInfo> GetAllDefaultImages() {
            List<ImageInfo> images = _dbConnection.Table<ImageInfo>().Where(x => x.exclusiveImageQueue == false)
                                                  .ToList();
            images.Sort();
            return images;
        }

        public void InsertDisplayedDefaultImage(ImageInfo imageInfo) {
            imageInfo.displayedDefaultImage = true;
            _dbConnection.Insert(imageInfo);
        }

        public List<ImageInfo> GetAllDisplayedDefaultImages() {
            List<ImageInfo> images = _dbConnection.Table<ImageInfo>().Where(x => x.exclusiveImageQueue == false)
                                                  .ToList();
            images.Sort();
            return images;
        }

        public void InsertExclusiveImage(ImageInfo imageInfo) {
            _dbConnection.Table<ImageInfo>().Delete(x => x.isActiveExclusiveImage);
            imageInfo.isActiveExclusiveImage = true;
            _dbConnection.Insert(imageInfo);
        }

        public ImageInfo GetActiveExclusiveImage() {
            return _dbConnection.Table<ImageInfo>().First(x => x.isActiveExclusiveImage);
        }
    }
}