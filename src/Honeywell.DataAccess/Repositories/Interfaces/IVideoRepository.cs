using Honeywell.Models;

namespace Honeywell.DataAccess.Repositories.Interfaces;

public interface IVideoRepository : IRepositoryBase<VideoFile>
{
    Task<IEnumerable<VideoFile>> GetVideosAsync(bool trackChanges);
    Task SaveVideoFile(VideoFile videoFile);
}