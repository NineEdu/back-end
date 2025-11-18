using ELearningPTIT.Modules.Courses.Domain.Entities;
using ELearningPTIT.Modules.Courses.Domain.Repositories;
using MongoDB.Driver;
using YourCompany.YourProject.Shared.Infrastructure.MongoDb.Repositories;

namespace ELearningPTIT.Modules.Courses.Infrastructure.Repositories;

public class QuestionRepository : MongoDbRepository<Question>, IQuestionRepository
{
    public QuestionRepository(IMongoDatabase database) : base(database)
    {
        // Create indexes
        CreateIndexAsync(Builders<Question>.IndexKeys.Ascending(x => x.CourseId)).Wait();
        CreateIndexAsync(Builders<Question>.IndexKeys.Ascending(x => x.LectureId)).Wait();
        CreateIndexAsync(Builders<Question>.IndexKeys.Ascending(x => x.UserId)).Wait();
        CreateIndexAsync(Builders<Question>.IndexKeys.Ascending(x => x.IsFlagged)).Wait();
    }

    public async Task<IEnumerable<Question>> GetByCourseIdAsync(string courseId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Question>.Filter.Eq(x => x.CourseId, courseId);
        return await Collection.Find(filter)
            .SortByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Question>> GetByLectureIdAsync(string lectureId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Question>.Filter.Eq(x => x.LectureId, lectureId);
        return await Collection.Find(filter)
            .SortByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Question>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Question>.Filter.Eq(x => x.UserId, userId);
        return await Collection.Find(filter)
            .SortByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Question>> GetUnansweredQuestionsAsync(string courseId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Question>.Filter.And(
            Builders<Question>.Filter.Eq(x => x.CourseId, courseId),
            Builders<Question>.Filter.Size(x => x.Answers, 0)
        );

        return await Collection.Find(filter)
            .SortByDescending(x => x.UpvoteCount)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Question>> GetFlaggedQuestionsAsync(CancellationToken cancellationToken = default)
    {
        var filter = Builders<Question>.Filter.Eq(x => x.IsFlagged, true);
        return await Collection.Find(filter)
            .SortByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    private async Task CreateIndexAsync(IndexKeysDefinition<Question> keys)
    {
        var indexModel = new CreateIndexModel<Question>(keys);
        await Collection.Indexes.CreateOneAsync(indexModel);
    }
}
