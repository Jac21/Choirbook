using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Choirbook.Models
{
    public class Score
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        public string Title { get; set; }

        public string Composer { get; set; }

        public int Revision { get; set; }
    }
}