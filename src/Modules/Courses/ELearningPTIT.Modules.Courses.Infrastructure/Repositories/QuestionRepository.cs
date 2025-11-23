using ELearningPTIT.Modules.Courses.Domain.Entities;
using ELearningPTIT.Modules.Courses.Domain.Repositories;
using MongoDB.Driver;
using YourCompany.YourProject.Shared.Infrastructure.MongoDb.Repositories;

namespace ELearningPTIT.Modules.Courses.Infrastructure.Repositories;

public class QuestionRepository : MongoDbRepository<Question>, IQuestionRepository
{
    public QuestionRepository(IMongoDatabase database) : base(database)
    {
        CreateIndexAsync(Builders<Question>.IndexKeys.Ascending(x => x.CourseId)).Wait();
        CreateIndexAsync(Builders<Question>.IndexKeys.Ascending(x => x.LectureId)).Wait();
        CreateIndexAsync(Builders<Question>.IndexKeys.Ascending(x => x.UserId)).Wait();
        CreateIndexAsync(Builders<Question>.IndexKeys.Ascending(x => x.IsFlagged)).Wait();
    }

    private async Task CreateIndexAsync(IndexKeysDefinition<Question> keys)
    {
        var indexModel = new CreateIndexModel<Question>(keys);
        await Collection.Indexes.CreateOneAsync(indexModel);
    }
}
