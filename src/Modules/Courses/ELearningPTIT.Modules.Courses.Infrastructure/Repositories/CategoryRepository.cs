using ELearningPTIT.Modules.Courses.Domain.Entities;
using ELearningPTIT.Modules.Courses.Domain.Repositories;
using MongoDB.Driver;
using YourCompany.YourProject.Shared.Infrastructure.MongoDb.Repositories;

namespace ELearningPTIT.Modules.Courses.Infrastructure.Repositories;

public class CategoryRepository : MongoDbRepository<Category>, ICategoryRepository
{
    public CategoryRepository(IMongoDatabase database) : base(database)
    {
        CreateIndexAsync(Builders<Category>.IndexKeys.Ascending(x => x.Slug)).Wait();
        CreateIndexAsync(Builders<Category>.IndexKeys.Ascending(x => x.IsActive)).Wait();
        CreateIndexAsync(Builders<Category>.IndexKeys.Ascending(x => x.ParentCategoryId)).Wait();
    }

    private async Task CreateIndexAsync(IndexKeysDefinition<Category> keys)
    {
        var indexModel = new CreateIndexModel<Category>(keys);
        await Collection.Indexes.CreateOneAsync(indexModel);
    }
}
