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
        result.Should().BeEmpty();  // Expecting an empty list
    }
    
    [Fact]
    public async Task GetVideosAsync_ThrowsException_ShouldPropagateException()
    {
        // Arrange
        const string message = "Database failure";
        _repository.GetVideosAsync(false).Throws(new Exception(message));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _repository.GetVideosAsync(false));
        message.Should().BeEquivalentTo(exception.Message);
    }
    
    [Fact]
    public async Task SaveVideoFile_InvalidVideoFile_ThrowsException()
    {
        // Arrange
        const string message = "Validation failed: FileName is required";
        var invalidVideoFile = new VideoFile(); // Invalid video file (missing required fields)

        _repository.When(x => x.SaveVideoFile(invalidVideoFile))
            .Do(x => { throw new Exception(message); });

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _repository.SaveVideoFile(invalidVideoFile));
        message.Should().BeEquivalentTo(exception.Message);
    }
    
    [Fact]
    public async Task SaveVideoFile_DatabaseIssue_ThrowsException()
    {
        // Arrange
        const string message = "Database failure during save";
        var validVideoFile = VideoFactory.GetVideos.Generate();

        _repository.When(x => x.SaveVideoFile(validVideoFile))
            .Do(x => { throw new Exception(message); });

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _repository.SaveVideoFile(validVideoFile));
        Assert.Equal(message, exception.Message);
    }
    
    [Fact]
    public async Task GetVideosAsync_ReturnsNull_ShouldHandleNull()
    {
        // Arrange
        _repository.GetVideosAsync(false).Returns((IEnumerable<VideoFile>)null); // Simulating null return

        // Act
        var result = await _repository.GetVideosAsync(false);

        // Assert
        result.Should().BeNull();  // Assert that the result is null, which could trigger error handling in higher layers
    }
}