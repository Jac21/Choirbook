using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<List<Score>> Get()
        {
            var scoreCursor = await _scores.FindAsync(score => true);

            return await scoreCursor.ToListAsync();
        }

        public async Task<Score> Get(string id)
        {
            var scoreCursor = await _scores.FindAsync(score => score.Id == id);

            return await scoreCursor.FirstOrDefaultAsync();
        }

        public async Task<Score> Create(Score score)
        {
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