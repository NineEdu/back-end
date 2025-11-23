using ELearningPTIT.Modules.Courses.Domain.Entities;
using ELearningPTIT.Modules.Courses.Domain.Repositories;
using MongoDB.Driver;
using YourCompany.YourProject.Shared.Infrastructure.MongoDb.Repositories;

namespace ELearningPTIT.Modules.Courses.Infrastructure.Repositories;

public class ReviewRepository : MongoDbRepository<Review>, IReviewRepository
{
    public ReviewRepository(IMongoDatabase database) : base(database)
    {
        CreateIndexAsync(Builders<Review>.IndexKeys.Ascending(x => x.CourseId)).Wait();
        CreateIndexAsync(Builders<Review>.IndexKeys.Ascending(x => x.UserId)).Wait();
        CreateIndexAsync(Builders<Review>.IndexKeys
            .Ascending(x => x.CourseId)
            .Ascending(x => x.UserId)).Wait();
        CreateIndexAsync(Builders<Review>.IndexKeys.Ascending(x => x.IsFlagged)).Wait();
    }

    private async Task CreateIndexAsync(IndexKeysDefinition<Review> keys)
    {
        var indexModel = new CreateIndexModel<Review>(keys);
        await Collection.Indexes.CreateOneAsync(indexModel);
    }
}
