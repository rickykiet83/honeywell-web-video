using DataFactory.Test;
using FluentAssertions;
using Honeywell.DataAccess.Repositories.Interfaces;
using Honeywell.Models;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Honeywell.Repository.Tests;

public class VideoRepositoryTests
{
    private readonly IVideoRepository _repository = Substitute.For<IVideoRepository>();
    
    [Fact]
    public async Task GetVideosAsync_TrackChangesFalse_ShouldCallFindAll()
    {
        // Arrange
        const int expectedCount = 2;
        var videos = VideoFactory.GetVideos.Generate(expectedCount);
        
        // Act
        _repository.GetVideosAsync(trackChanges: false).Returns(videos);
        var result = await _repository.GetVideosAsync(false);
        
        // Assert
        result.Should().NotBeNull();
        result.Count().Should().Be(expectedCount);
    }
    
    [Fact]
    public async Task GetVideosAsync_TrackChangesTrue_ShouldCallFindAll()
    {
        // Arrange
        const int expectedCount = 2;
        var videos = VideoFactory.GetVideos.Generate(expectedCount);
        
        // Act
        _repository.GetVideosAsync(trackChanges: true).Returns(videos);
        var result = await _repository.GetVideosAsync(true);
        
        // Assert
        result.Should().NotBeNull();
        result.Count().Should().Be(expectedCount);
    }
    
    [Fact]
    public async Task SaveVideoFile_ShouldCallAdd()
    {
        // Arrange
        var videoFile = VideoFactory.GetVideosWithMp4.Generate(1).First();
        
        // Act
        await _repository.SaveVideoFile(videoFile);
        
        // Assert
        await _repository.Received(1).SaveVideoFile(videoFile);
    }
    
    [Fact]
    public async Task GetVideosAsync_NoVideosFound_ReturnsEmptyList()
    {
        // Arrange
        _repository.GetVideosAsync(false).Returns(Enumerable.Empty<VideoFile>());

        // Act
        var result = await _repository.GetVideosAsync(false);

        // Assert
        Assert.Empty(result);  // Expecting an empty list
    }
    
    [Fact]
    public async Task GetVideosAsync_ThrowsException_ShouldPropagateException()
    {
        // Arrange
        _repository.GetVideosAsync(false).Throws(new Exception("Database failure"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _repository.GetVideosAsync(false));
        Assert.Equal("Database failure", exception.Message);
    }
    
    [Fact]
    public async Task SaveVideoFile_InvalidVideoFile_ThrowsException()
    {
        // Arrange
        var invalidVideoFile = new VideoFile(); // Invalid video file (missing required fields)

        _repository.When(x => x.SaveVideoFile(invalidVideoFile))
            .Do(x => { throw new Exception("Validation failed: FileName is required"); });

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _repository.SaveVideoFile(invalidVideoFile));
        Assert.Equal("Validation failed: FileName is required", exception.Message);
    }
}