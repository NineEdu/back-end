using ELearningPTIT.Modules.Users.Domain.Entities;
using ELearningPTIT.Modules.Users.Domain.Repositories;
using MongoDB.Driver;
using YourCompany.YourProject.Shared.Infrastructure.MongoDb.Repositories;

namespace ELearningPTIT.Modules.Users.Infrastructure.Repositories;

/// <summary>
/// MongoDB implementation of IUserRepository
/// </summary>
public class UserRepository : MongoDbRepository<User>, IUserRepository
{
    public UserRepository(IMongoDatabase database) : base(database)
    {
        // Create indexes for frequently queried fields
        CreateIndexes();
    }

    private void CreateIndexes()
    {
        // Index on email for fast lookups and uniqueness
        var emailIndexKeys = Builders<User>.IndexKeys.Ascending(u => u.Email);
        var emailIndexOptions = new CreateIndexOptions { Unique = true };
        var emailIndexModel = new CreateIndexModel<User>(emailIndexKeys, emailIndexOptions);

        // Index on roles for role-based queries
        var rolesIndexKeys = Builders<User>.IndexKeys.Ascending(u => u.Roles);
        var rolesIndexModel = new CreateIndexModel<User>(rolesIndexKeys);

        // Index on email verification token
        var verificationTokenIndexKeys = Builders<User>.IndexKeys.Ascending(u => u.EmailVerificationToken);
        var verificationTokenIndexModel = new CreateIndexModel<User>(verificationTokenIndexKeys);

        // Index on password reset token
        var resetTokenIndexKeys = Builders<User>.IndexKeys.Ascending(u => u.PasswordResetToken);
        var resetTokenIndexModel = new CreateIndexModel<User>(resetTokenIndexKeys);

        // Index on refresh tokens
        var refreshTokenIndexKeys = Builders<User>.IndexKeys.Ascending("RefreshTokens.Token");
        var refreshTokenIndexModel = new CreateIndexModel<User>(refreshTokenIndexKeys);

        try
        {
            Collection.Indexes.CreateMany(new[]
            {
                emailIndexModel,
                rolesIndexModel,
                verificationTokenIndexModel,
                resetTokenIndexModel,
                refreshTokenIndexModel
            });
        }
        catch (MongoCommandException)
        {
            // Indexes might already exist, ignore the error
        }
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Email, email.ToLowerInvariant());
        return await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Email, email.ToLowerInvariant());
        var count = await Collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
        return count > 0;
    }

    public async Task<List<User>> GetByRoleAsync(UserRole role, CancellationToken cancellationToken = default)
    {
        var filter = Builders<User>.Filter.AnyEq(u => u.Roles, role);
        return await Collection.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<User?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var filter = Builders<User>.Filter.ElemMatch(
            u => u.RefreshTokens,
            rt => rt.Token == refreshToken
        );
        return await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<User?> GetByEmailVerificationTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        var filter = Builders<User>.Filter.Eq(u => u.EmailVerificationToken, token);
        return await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<User?> GetByPasswordResetTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        var filter = Builders<User>.Filter.And(
            Builders<User>.Filter.Eq(u => u.PasswordResetToken, token),
            Builders<User>.Filter.Gt(u => u.PasswordResetTokenExpiry, DateTime.UtcNow)
        );
        return await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<User>> SearchUsersAsync(
        string searchTerm,
        int skip = 0,
        int take = 20,
        CancellationToken cancellationToken = default
    )
    {
        var filter = Builders<User>.Filter.Or(
            Builders<User>.Filter.Regex(u => u.Email, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")),
            Builders<User>.Filter.Regex(u => u.FirstName, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")),
            Builders<User>.Filter.Regex(u => u.LastName, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i"))
        );

        return await Collection.Find(filter)
            .Skip(skip)
            .Limit(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<(List<User> Instructors, long TotalCount)> GetInstructorsAsync(
        int skip = 0,
        int take = 20,
        CancellationToken cancellationToken = default
    )
    {
        var filter = Builders<User>.Filter.AnyEq(u => u.Roles, UserRole.Instructor);

        var instructors = await Collection.Find(filter)
            .Skip(skip)
            .Limit(take)
            .ToListAsync(cancellationToken);

        var totalCount = await Collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        return (instructors, totalCount);
    }
}
