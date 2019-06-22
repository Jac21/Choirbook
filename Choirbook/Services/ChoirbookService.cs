using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using Choirbook.Models;
using Choirbook.Models.Interfaces;

namespace Choirbook.Services
{
    public class ChoirbookService
    {
        private readonly IMongoCollection<Score> _scores;

        public ChoirbookService(IChoirbookDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _scores = database.GetCollection<Score>(settings.ChoirbookCollectionName);
        }

        public List<Score> Get() =>
            _scores.Find(score => true).ToList();

        public Score Get(string id) =>
            _scores.Find(score => score.Id == id).FirstOrDefault();

        public Score Create(Score score)
        {
            _scores.InsertOne(score);
            return score;
        }

        public void Update(string id, Score scoreIn) =>
            _scores.ReplaceOne(score => score.Id == id, scoreIn);

        public void Remove(Score scoreIn) =>
            _scores.DeleteOne(score => score.Id == scoreIn.Id);

        public void Remove(string id) =>
            _scores.DeleteOne(score => score.Id == id);
    }
}