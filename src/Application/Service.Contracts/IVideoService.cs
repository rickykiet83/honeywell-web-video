using Honeywell.Models.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Service.Contracts;

public interface IVideoService
{
    Task SaveVideoFileAsync(List<IFormFile> files);
    Task<IEnumerable<VideoVM>> GetVideoFilesAsync();
}