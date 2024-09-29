using DataFactory.Test;
using FluentAssertions;
using Honeywell.DataAccess.Repositories.Interfaces;
using NSubstitute;

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
}