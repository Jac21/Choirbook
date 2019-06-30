using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using Choirbook.Models;
using Choirbook.Models.Interfaces;
using MongoDB.Bson;

namespace Choirbook.Services
{
    public class ChoirbookService
    {
        private readonly IMongoCollection<Score> _scores;
        private readonly GridFSBucket _gridFsBucket;

        public ChoirbookService(IChoirbookDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _gridFsBucket = new GridFSBucket(database, new GridFSBucketOptions
            {
                BucketName = "scores",
                ChunkSizeBytes = 1048576,
                WriteConcern = WriteConcern.WMajority,
                ReadPreference = ReadPreference.Secondary
            });

            _scores = database.GetCollection<Score>(settings.ChoirbookCollectionName);
        }

        public async Task<List<Score>> Get()
        {
            var scoreCursor = await _scores.FindAsync(score => true);

            return await scoreCursor.ToListAsync();
        }

        public async Task<Score> Get(string id)
        {
            // var bytes = await _gridFsBucket.DownloadAsBytesAsync(new BsonObjectId(new ObjectId(id)));

            var scoreCursor = await _scores.FindAsync(score => score.Id == id);

            return await scoreCursor.FirstOrDefaultAsync();
        }

        public async Task<Score> Create(Score score)
        {
            var scoreTrack = Convert.FromBase64String(score.Track);

            var id = await _gridFsBucket.UploadFromBytesAsync(score.Title, scoreTrack, new GridFSUploadOptions
            {
                Metadata = new BsonDocument
                {
                    {"Composer", score.Composer}
                }
            });

            score.Id = id.ToString();

            await _scores.InsertOneAsync(score);
            return score;
        }

        public async Task Update(string id, Score scoreIn)
        {
            await _scores.ReplaceOneAsync(score => score.Id == id, scoreIn);
        }

        public async Task Remove(string id)
        {
            await _scores.DeleteOneAsync(score => score.Id == id);
        }
    }
}