using Microsoft.AspNetCore.Http;

namespace Service.Contracts.Interfaces;

public interface IVideoValidator
{
    ServiceResult ValidateVideo(IFormFile file);
}