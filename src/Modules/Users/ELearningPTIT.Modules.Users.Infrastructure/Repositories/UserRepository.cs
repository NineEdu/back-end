using ELearningPTIT.Modules.Users.Domain.Entities;
using ELearningPTIT.Modules.Users.Domain.Repositories;
using MongoDB.Driver;
using YourCompany.YourProject.Shared.Infrastructure.MongoDb.Repositories;

namespace ELearningPTIT.Modules.Users.Infrastructure.Repositories;

public class UserRepository : MongoDbRepository<User>, IUserRepository
{
    public UserRepository(IMongoDatabase database) : base(database)
    {
        CreateIndexes();
    }

    private void CreateIndexes()
    {
        var emailIndexKeys = Builders<User>.IndexKeys.Ascending(u => u.Email);
        var emailIndexOptions = new CreateIndexOptions { Unique = true };
        var emailIndexModel = new CreateIndexModel<User>(emailIndexKeys, emailIndexOptions);

        try
        {
            Collection.Indexes.CreateOne(emailIndexModel);
        }
        catch (MongoCommandException)
        {
        }
    }
}
