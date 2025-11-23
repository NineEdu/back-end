using ELearningPTIT.Modules.Courses.Domain.Entities;
using ELearningPTIT.Modules.Courses.Domain.Repositories;
using MongoDB.Driver;
using YourCompany.YourProject.Shared.Infrastructure.MongoDb.Repositories;

namespace ELearningPTIT.Modules.Courses.Infrastructure.Repositories;

public class CourseRepository : MongoDbRepository<Course>, ICourseRepository
{
    public CourseRepository(IMongoDatabase database) : base(database)
    {
        CreateIndexAsync(Builders<Course>.IndexKeys.Ascending(x => x.Slug)).Wait();
        CreateIndexAsync(Builders<Course>.IndexKeys.Ascending(x => x.InstructorId)).Wait();
        CreateIndexAsync(Builders<Course>.IndexKeys.Ascending(x => x.CategoryId)).Wait();
        CreateIndexAsync(Builders<Course>.IndexKeys.Ascending(x => x.Status)).Wait();
        CreateIndexAsync(Builders<Course>.IndexKeys.Text(x => x.Title).Text(x => x.Description)).Wait();
    }

    private async Task CreateIndexAsync(IndexKeysDefinition<Course> keys)
    {
        var indexModel = new CreateIndexModel<Course>(keys);
        await Collection.Indexes.CreateOneAsync(indexModel);
    }
}
