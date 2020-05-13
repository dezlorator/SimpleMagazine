using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PetStore.Models.MongoDb
{
    public class ImagesDbContext
    {
        IMongoDatabase _database; // база данных
        IGridFSBucket _gridFS;   // файловое хранилище

        public ImagesDbContext()
        {
            // строка подключения
            string connectionString = "mongodb+srv://admin:123@cluster0-yxi0t.azure.mongodb.net/test?retryWrites=true&w=majority";
            var connection = new MongoUrlBuilder(connectionString);
            // получаем клиента для взаимодействия с базой данных
            MongoClient client = new MongoClient(connectionString);
            // получаем доступ к самой базе данных
            _database = client.GetDatabase("Images");
            // получаем доступ к файловому хранилищу
            _gridFS = new GridFSBucket(_database);
        }

        public async Task<byte[]> GetImage(string id)
        {
            return await _gridFS.DownloadAsBytesAsync(new ObjectId(id));
        }

        public async Task<string> StoreImage(Stream imageStream, string imageName)
        {
            ObjectId imageId = await _gridFS.UploadFromStreamAsync(imageName, imageStream);

            return imageId.ToString();
        }
    }
}
