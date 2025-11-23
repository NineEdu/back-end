using ELearningPTIT.Modules.Media.Domain.Entities;
using ELearningPTIT.Modules.Media.Domain.Repositories;
using MongoDB.Driver;
using YourCompany.YourProject.Shared.Infrastructure.MongoDb.Repositories;

namespace ELearningPTIT.Modules.Media.Infrastructure.Repositories;

public class MediaAssetRepository : MongoDbRepository<MediaAsset>, IMediaAssetRepository
{
    public MediaAssetRepository(IMongoDatabase database) : base(database)
    {
    }
}
