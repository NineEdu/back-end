using ELearningPTIT.Modules.Courses.Domain.Entities;
using ELearningPTIT.Modules.Courses.Domain.Repositories;
using MongoDB.Driver;
using YourCompany.YourProject.Shared.Infrastructure.MongoDb.Repositories;

namespace ELearningPTIT.Modules.Courses.Infrastructure.Repositories;

public class ReviewRepository : MongoDbRepository<Review>, IReviewRepository
{
    public ReviewRepository(IMongoDatabase database) : base(database)
    {
        // Create indexes
        CreateIndexAsync(Builders<Review>.IndexKeys.Ascending(x => x.CourseId)).Wait();
        CreateIndexAsync(Builders<Review>.IndexKeys.Ascending(x => x.UserId)).Wait();
        CreateIndexAsync(Builders<Review>.IndexKeys
            .Ascending(x => x.CourseId)
            .Ascending(x => x.UserId)).Wait();
        CreateIndexAsync(Builders<Review>.IndexKeys.Ascending(x => x.IsFlagged)).Wait();
    }

    public async Task<IEnumerable<Review>> GetByCourseIdAsync(string courseId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Review>.Filter.And(
            Builders<Review>.Filter.Eq(x => x.CourseId, courseId),
            Builders<Review>.Filter.Eq(x => x.IsPublished, true)
        );

        return await Collection.Find(filter)
            .SortByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Review?> GetByUserAndCourseAsync(string userId, string courseId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Review>.Filter.And(
            Builders<Review>.Filter.Eq(x => x.UserId, userId),
            Builders<Review>.Filter.Eq(x => x.CourseId, courseId)
        );

        return await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<(double AverageRating, int TotalReviews)> GetCourseRatingStatsAsync(string courseId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Review>.Filter.And(
            Builders<Review>.Filter.Eq(x => x.CourseId, courseId),
            Builders<Review>.Filter.Eq(x => x.IsPublished, true)
        );

        var reviews = await Collection.Find(filter).ToListAsync(cancellationToken);

        if (!reviews.Any())
        {
            return (0.0, 0);
        }

        var averageRating = reviews.Average(r => r.Rating);
        var totalReviews = reviews.Count;

        return (Math.Round(averageRating, 2), totalReviews);
    }

    public async Task<IEnumerable<Review>> GetFlaggedReviewsAsync(CancellationToken cancellationToken = default)
    {
        var filter = Builders<Review>.Filter.Eq(x => x.IsFlagged, true);
        return await Collection.Find(filter)
            .SortByDescending(x => x.ReportCount)
            .ToListAsync(cancellationToken);
    }

    private async Task CreateIndexAsync(IndexKeysDefinition<Review> keys)
    {
        var indexModel = new CreateIndexModel<Review>(keys);
        await Collection.Indexes.CreateOneAsync(indexModel);
    }
}
