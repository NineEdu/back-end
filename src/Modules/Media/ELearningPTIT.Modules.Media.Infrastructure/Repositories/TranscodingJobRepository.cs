using ELearningPTIT.Modules.Media.Domain.Entities;
using ELearningPTIT.Modules.Media.Domain.Repositories;
using MongoDB.Driver;
using YourCompany.YourProject.Shared.Infrastructure.MongoDb.Repositories;

namespace ELearningPTIT.Modules.Media.Infrastructure.Repositories;

public class TranscodingJobRepository : MongoDbRepository<TranscodingJob>, ITranscodingJobRepository
{
    public TranscodingJobRepository(IMongoDatabase database) : base(database)
    {
    }
}
