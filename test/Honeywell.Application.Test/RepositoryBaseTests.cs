using System.Linq.Expressions;
using DataFactory.Test;
using FluentAssertions;
using Honeywell.DataAccess.Repositories.Interfaces;
using Honeywell.Models;
using NSubstitute;

namespace Honeywell.Application.Test;

public class RepositoryBaseTests
{
    private readonly IRepositoryBase<VideoFile> _repository = Substitute.For<IRepositoryBase<VideoFile>>();

    [Fact]
    public void FindAll_TrackChangesFalse_ShouldCallAsNoTracking()
    {
        // Arrange
        const int expectedCount = 2;
        var videos = VideoFactory.GetVideos.Generate(expectedCount).AsQueryable();
        
        // Act
        _repository.FindAll(trackChanges: false).Returns(videos);
        var result = _repository.FindAll(false);
        
        // Assert
        result.Should().NotBeNull();
        result.Count().Should().Be(expectedCount);
        _repository.Received(1).FindAll(false);
    }
    
    [Fact]
    public void FindAll_TrackChangesTrue_ShouldCallAsTracking()
    {
        // Arrange
        const int expectedCount = 2;
        var videos = VideoFactory.GetVideos.Generate(expectedCount).AsQueryable();
        
        // Act
        _repository.FindAll(trackChanges: true).Returns(videos);
        var result = _repository.FindAll(true);
        
        // Assert
        result.Should().NotBeNull();
        result.Count().Should().Be(expectedCount);
        _repository.Received(1).FindAll(true);
    }

    [Fact]
    public void FindByCondition_TrackChangesFalse_ShouldCallAsNoTracking()
    {
        // Arrange
        Expression<Func<VideoFile, bool>> condition = x => x.FileType.Equals("video/mp4");

        const int expectedCount = 2;
        var videos = VideoFactory.GetVideosWithMp4.Generate(10)
            .Take(expectedCount)
            .AsQueryable();

        _repository.FindByCondition(condition, false).Returns(videos);
        var result = _repository.FindByCondition(condition, false);
        
        // Assert
        result.Should().NotBeNull();
        result.Count().Should().Be(expectedCount);
    }
    
    [Fact]
    public void FindByCondition_TrackChangesTrue_ShouldCallAsTracking()
    {
        // Arrange
        Expression<Func<VideoFile, bool>> condition = x => x.FileType.Equals("video/mp4");

        const int expectedCount = 2;
        var videos = VideoFactory.GetVideosWithMp4.Generate(10)
            .Take(expectedCount)
            .AsQueryable();

        _repository.FindByCondition(condition, true).Returns(videos);
        var result = _repository.FindByCondition(condition, true);
        
        // Assert
        result.Should().NotBeNull();
        result.Count().Should().Be(expectedCount);
    }
    
    [Fact]
    public void Create_ShouldCallCreate()
    {
        // Arrange
        var video = VideoFactory.GetVideos.Generate();
        
        // Act
        _repository.Create(video);
        
        // Assert
        _repository.Received(1).Create(video);
    }
    
    [Fact]
    public void Update_ShouldCallUpdate()
    {
        // Arrange
        var video = VideoFactory.GetVideos.Generate();
        
        // Act
        _repository.Update(video);
        
        // Assert
        _repository.Received(1).Update(video);
    }
    
    [Fact]
    public void Delete_ShouldCallDelete()
    {
        // Arrange
        var video = VideoFactory.GetVideos.Generate();
        
        // Act
        _repository.Delete(video);
        
        // Assert
        _repository.Received(1).Delete(video);
    }
    
    [Fact]
    public void SaveAsync_ShouldCallSaveAsync()
    {
        // Act
        _repository.SaveAsync();
        
        // Assert
        _repository.Received(1).SaveAsync();
    }
}