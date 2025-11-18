using ELearningPTIT.Modules.Courses.Domain.Entities;
using ELearningPTIT.Modules.Courses.Domain.Repositories;
using ELearningPTIT.Modules.Courses.Domain.ValueObjects;
using MongoDB.Driver;
using YourCompany.YourProject.Shared.Infrastructure.MongoDb.Repositories;

namespace ELearningPTIT.Modules.Courses.Infrastructure.Repositories;

public class CourseRepository : MongoDbRepository<Course>, ICourseRepository
{
    public CourseRepository(IMongoDatabase database) : base(database)
    {
        // Create indexes
        CreateIndexAsync(Builders<Course>.IndexKeys.Ascending(x => x.Slug)).Wait();
        CreateIndexAsync(Builders<Course>.IndexKeys.Ascending(x => x.InstructorId)).Wait();
        CreateIndexAsync(Builders<Course>.IndexKeys.Ascending(x => x.CategoryId)).Wait();
        CreateIndexAsync(Builders<Course>.IndexKeys.Ascending(x => x.Status)).Wait();
        CreateIndexAsync(Builders<Course>.IndexKeys.Text(x => x.Title).Text(x => x.Description)).Wait();
    }

    public async Task<IEnumerable<Course>> GetByInstructorIdAsync(string instructorId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Course>.Filter.Eq(x => x.InstructorId, instructorId);
        return await Collection.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Course>> GetByCategoryIdAsync(string categoryId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Course>.Filter.Eq(x => x.CategoryId, categoryId);
        return await Collection.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Course>> GetByStatusAsync(CourseStatus status, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Course>.Filter.Eq(x => x.Status, status);
        return await Collection.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<Course?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Course>.Filter.Eq(x => x.Slug, slug);
        return await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Course>> GetPublishedCoursesAsync(int skip = 0, int take = 20, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Course>.Filter.Eq(x => x.Status, CourseStatus.Published);
        return await Collection.Find(filter)
            .SortByDescending(x => x.PublishedAt)
            .Skip(skip)
            .Limit(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Course>> SearchCoursesAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Course>.Filter.And(
            Builders<Course>.Filter.Eq(x => x.Status, CourseStatus.Published),
            Builders<Course>.Filter.Text(searchTerm)
        );

        return await Collection.Find(filter)
            .Limit(50)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateRatingAsync(string courseId, double averageRating, int totalReviews, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Course>.Filter.Eq(x => x.Id, courseId);
        var update = Builders<Course>.Update
            .Set(x => x.AverageRating, averageRating)
            .Set(x => x.TotalReviews, totalReviews);

        await Collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
    }

    public async Task IncrementEnrollmentCountAsync(string courseId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Course>.Filter.Eq(x => x.Id, courseId);
        var update = Builders<Course>.Update.Inc(x => x.TotalEnrollments, 1);
        await Collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
    }

    private async Task CreateIndexAsync(IndexKeysDefinition<Course> keys)
    {
        var indexModel = new CreateIndexModel<Course>(keys);
        await Collection.Indexes.CreateOneAsync(indexModel);
    }
}
