using Main.Service.Auth0.Model;

namespace Main.Service.MongoDB
{
    public interface IMongoDbService
    {
        public Task<bool> InsertSession(SessionItem session);
    }
}