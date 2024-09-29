using System.Linq.Expressions;
using DataFactory.Test;
using Honeywell.DataAccess.Repositories.Interfaces;
using Honeywell.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Service;
using Service.Contracts;
using Service.Contracts.Interfaces;

namespace Honeywell.Application.Test;

public class VideoServiceTests
{
    private readonly IVideoRepository _videoRepository;
    private readonly ILogger<VideoService> _logger;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IVideoValidator _validator = Substitute.For<IVideoValidator>();
    private readonly VideoService _videoService;

    public VideoServiceTests()
    {
        _videoRepository = Substitute.For<IVideoRepository>();
        _logger = Substitute.For<ILogger<VideoService>>();
        _webHostEnvironment = Substitute.For<IWebHostEnvironment>();
        _videoService = new VideoService(_videoRepository, _logger, _webHostEnvironment, _validator);
    }

    [Fact]
    public async Task SaveVideoFileAsync_WhenCalledWithValidFiles_ShouldReturnSuccess()
    {
        // Arrange
        var files = new List<IFormFile>
        {
            CreateMockFile("video.mp4", 100 * 1024 * 1024) // 100MB file with valid extension
        };

        _validator.ValidateVideo(files[0]).Returns(new ServiceResult { Success = true });

        // Act
        var result = await _videoService.UploadVideoFileAsync(files);

        // Assert
        result.Success.Should().BeTrue();
    }

    [Fact]
    public async Task UploadVideoFileAsync_FileValidationFails_ShouldReturnError()
    {
        // Arrange
        var file = CreateMockFile("video.mp4", 100 * 1024 * 1024); // Valid size, but let's make the validation fail
        _validator.ValidateVideo(file).Returns(new ServiceResult
            { Success = false, Errors = new List<string> { "Invalid file" } });

        // Act
        var result = await _videoService.UploadVideoFileAsync(new List<IFormFile> { file });

        // Assert
        result.Errors.Should().Contain("Invalid file");
        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task UploadVideoFileAsync_ValidFile_ShouldUploadAndSave()
    {
        // Arrange
        var file = CreateMockFile("video.mp4", 100 * 1024 * 1024); // Valid file
        _validator.ValidateVideo(file).Returns(new ServiceResult()); // Valid file
        Expression<Func<VideoFile, bool>> condition = x => x.FileName == "video.mp4";
        _videoRepository.FindByCondition(condition, false)
            .Returns(Enumerable.Empty<VideoFile>().AsQueryable()); // No file exists with the same name

        // Act
        var result = await _videoService.UploadVideoFileAsync(new List<IFormFile> { file });

        // Assert
        result.Success.Should().BeTrue();
        await _videoRepository.Received(1).SaveVideoFile(Arg.Is<VideoFile>(v => v.FileName == "video.mp4"));
    }

    [Fact]
    public async Task UploadVideoFileAsync_DuplicateFileName_ShouldNotSaveAgain()
    {
        // Arrange
        var file = CreateMockFile("video.mp4", 100 * 1024 * 1024); // Valid file
        _validator.ValidateVideo(file).Returns(new ServiceResult()); // Valid file
        Expression<Func<VideoFile, bool>> condition = x => x.FileName == "video.mp4";

        // Set up mock to simulate that a file with the same name already exists
        _videoRepository.FindByCondition(Arg.Is<Expression<Func<VideoFile, bool>>>(predicate =>
            predicate.Compile().Invoke(new VideoFile { FileName = "video.mp4" })
        ), false).Returns(new List<VideoFile> { new VideoFile { FileName = "video.mp4" } }.AsQueryable());
       
        // Act
        var result = await _videoService.UploadVideoFileAsync(new List<IFormFile> { file });

        // Assert
        result.Success.Should().BeTrue(); // The service still succeeds
        await _videoRepository.DidNotReceive()
            .SaveVideoFile(Arg.Any<VideoFile>()); // No save operation should be performed
    }
    
    [Fact]
    public async Task GetVideoFilesAsync_ShouldReturnVideoFiles()
    {
        // Arrange
        var videoFiles = VideoFactory.GetVideos.Generate(2);
        var videoVMs = videoFiles.Select(VideoFactory.MapToViewModel).ToList();

        _videoRepository.GetVideosAsync(false).Returns(videoFiles);

        // Act
        var result = await _videoService.GetVideoFilesAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(videoVMs);
    }

    // Helper method to create a mock IFormFile
    private IFormFile CreateMockFile(string fileName, long fileSize)
    {
        var fileMock = Substitute.For<IFormFile>();
        fileMock.FileName.Returns(fileName);
        fileMock.Length.Returns(fileSize);
        return fileMock;
    }
}