using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Main.Service.Auth0.Model
{
    public class SessionItem : SignInResponse
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string? Username { get; set; }
    }
}
