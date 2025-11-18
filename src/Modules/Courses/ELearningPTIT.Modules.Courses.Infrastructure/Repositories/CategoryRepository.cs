using ELearningPTIT.Modules.Courses.Domain.Entities;
using ELearningPTIT.Modules.Courses.Domain.Repositories;
using MongoDB.Driver;
using YourCompany.YourProject.Shared.Infrastructure.MongoDb.Repositories;

namespace ELearningPTIT.Modules.Courses.Infrastructure.Repositories;

public class CategoryRepository : MongoDbRepository<Category>, ICategoryRepository
{
    public CategoryRepository(IMongoDatabase database) : base(database)
    {
        // Create indexes
        CreateIndexAsync(Builders<Category>.IndexKeys.Ascending(x => x.Slug)).Wait();
        CreateIndexAsync(Builders<Category>.IndexKeys.Ascending(x => x.IsActive)).Wait();
        CreateIndexAsync(Builders<Category>.IndexKeys.Ascending(x => x.ParentCategoryId)).Wait();
    }

    public async Task<Category?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Category>.Filter.Eq(x => x.Slug, slug);
        return await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Category>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        var filter = Builders<Category>.Filter.Eq(x => x.IsActive, true);
        return await Collection.Find(filter)
            .SortBy(x => x.DisplayOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Category>> GetByParentIdAsync(string? parentId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Category>.Filter.Eq(x => x.ParentCategoryId, parentId);
        return await Collection.Find(filter)
            .SortBy(x => x.DisplayOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task IncrementCourseCountAsync(string categoryId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Category>.Filter.Eq(x => x.Id, categoryId);
        var update = Builders<Category>.Update.Inc(x => x.CourseCount, 1);
        await Collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
    }

    public async Task DecrementCourseCountAsync(string categoryId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Category>.Filter.Eq(x => x.Id, categoryId);
        var update = Builders<Category>.Update.Inc(x => x.CourseCount, -1);
        await Collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
    }

    private async Task CreateIndexAsync(IndexKeysDefinition<Category> keys)
    {
        var indexModel = new CreateIndexModel<Category>(keys);
        await Collection.Indexes.CreateOneAsync(indexModel);
    }
}
