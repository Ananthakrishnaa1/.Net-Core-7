using Main.Service.Auth0.Model;
using MongoDB.Driver;

namespace Main.Service.MongoDB
{
    public class MongoDbService:IMongoDbService
    {
        private readonly IMongoDatabase _database;
        public MongoDbService() {
            MongoClientSettings settings = MongoClientSettings.FromConnectionString("mongodb+srv://ananthkrishnaa7:TuDI8fWLyrULhFhh@cluster0.mybceob.mongodb.net/?retryWrites=true&w=majority");
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var client = new MongoClient(settings);
            _database = client.GetDatabase("MainCo");
        }


        public async Task<bool> InsertSession(SessionItem session)
        {
            var _sessions = _database.GetCollection<SessionItem>("MainService");

            await _sessions.InsertOneAsync(session);

            return true;
        }
    }
}
