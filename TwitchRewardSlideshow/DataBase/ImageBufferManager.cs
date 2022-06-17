using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
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

        public ImageBuffer GetImageBuffer() {
            ImageBuffer buffer = new();
            TableQuery<ImageInfo> imageInfos = _dbConnection.Table<ImageInfo>().OrderBy(x => x.index);
            buffer.toCheckImagesQueue = new Queue<ImageInfo>(imageInfos.Where(x => x.toCheckImageQueue).ToList());
            buffer.exclusiveImagesQueue = new Queue<ImageInfo>(imageInfos.Where(x => x.exclusiveImageQueue).ToList());
            buffer.activeImages = imageInfos.Where(x => x.activeImage).ToList();
            buffer.displayedImages = imageInfos.Where(x => x.displayedImage).ToList();
            buffer.defaultImages = imageInfos.Where(x => x.defaultImage).ToList();
            buffer.displayedDefaultImages = imageInfos.Where(x => x.displayedDefaultImage).ToList();
            buffer.activeExclusiveImage = imageInfos.First(x => x.isActiveExclusiveImage);
            return buffer;
        }

        public void SetImageBuffer(ImageBuffer buffer) {
            InsertOrReplaceAll(buffer.toCheckImagesQueue.ToList());
            InsertOrReplaceAll(buffer.exclusiveImagesQueue.ToList());
            InsertOrReplaceAll(buffer.activeImages);
            InsertOrReplaceAll(buffer.displayedImages);
            InsertOrReplaceAll(buffer.defaultImages);
            InsertOrReplaceAll(buffer.displayedDefaultImages);
            _dbConnection.InsertOrReplace(buffer.activeExclusiveImage);
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

        public ImageInfo GetToCheckImage() {
            ImageInfo info = GetAllToCheckImagesQueue().First();
            info.toCheckImageQueue = false;
            _dbConnection.Update(info);
            return info;
        }

        public List<ImageInfo> GetAllToCheckImagesQueue() {
            List<ImageInfo> images = _dbConnection.Table<ImageInfo>().Where(x => x.toCheckImageQueue).ToList();
            images.Sort();
            return images;
        }

        public void InsetExclusiveImageQueue(ImageInfo imageInfo) {
            imageInfo.exclusiveImageQueue = true;
            _dbConnection.Insert(imageInfo);
        }

        public ImageInfo GetNextExclusiveImage() {
            ImageInfo info = GetAllToCheckImagesQueue().First();
            info.exclusiveImageQueue = false;
            _dbConnection.Update(info);
            return info;
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

        public int InsertOrReplaceAll(IEnumerable objects, bool runInTransaction = true) {
            var c = 0;
            if (objects == null) return c;

            if (runInTransaction) {
                _dbConnection.RunInTransaction(() => {
                    c += objects.Cast<object>().Sum(r => _dbConnection.Insert(r, "OR REPLACE", Orm.GetType(r)));
                });
            } else {
                c += objects.Cast<object>().Sum(r => _dbConnection.Insert(r, "OR REPLACE", Orm.GetType(r)));
            }

            return c;
        }
    }
}