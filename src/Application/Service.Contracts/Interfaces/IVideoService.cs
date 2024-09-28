using Honeywell.Models.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Service.Contracts.Interfaces;

public interface IVideoService
{
    Task<ServiceResult> SaveVideoFileAsync(List<IFormFile> files);
    Task<IEnumerable<VideoVM>> GetVideoFilesAsync();
}