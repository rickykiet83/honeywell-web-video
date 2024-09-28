using Honeywell.DataAccess.Data;
using Honeywell.DataAccess.Repositories.Interfaces;
using Honeywell.Models;
using Microsoft.EntityFrameworkCore;

namespace Honeywell.DataAccess.Repositories;

public class VideoRepository : RepositoryBase<VideoFile>, IVideoRepository
{
    public VideoRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<VideoFile>> GetVideosAsync(bool trackChanges)
    {
        return await FindAll(trackChanges).ToListAsync();
    }

    public async Task SaveVideoFile(VideoFile videoFile)
    {
        Create(videoFile);
        await SaveAsync();
    }
}