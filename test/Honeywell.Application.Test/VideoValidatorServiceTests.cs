
using Honeywell.Utility.Settings;
using Microsoft.AspNetCore.Http;
using Service.Contracts;

namespace Honeywell.Application.Test;

public class VideoValidatorServiceTests
{
    private readonly VideoValidatorService _validator = new();
    
    [Fact]
    public void ValidateVideo_FileSizeTooLarge_ShouldReturnError()
    {
        // Arrange
        var largeFile = CreateMockFile("video.mp4", SystemConstants.MaxVideoSize + 1); // File size larger than 200MB

        // Act
        var result = _validator.ValidateVideo(largeFile);

        // Assert
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(e => e == $"The file size of file name: {largeFile.FileName} is too large, {SystemConstants.MaxVideoSize}MB maximum.");
    }
    
    [Fact]
    public void ValidateVideo_InvalidExtension_ShouldReturnError()
    {
        // Arrange
        var invalidExtensionFile = CreateMockFile("video.wmv", 100 * 1024 * 1024); // 100MB file, but wrong extension

        // Act
        var result = _validator.ValidateVideo(invalidExtensionFile);

        // Assert
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(e => e == $"The file type of file name: {invalidExtensionFile.FileName} is not allowed, {SystemConstants.DefaultVideoExtension} files only.");
    }
    
    [Fact]
    public void ValidateVideo_ValidFile_ShouldReturnSuccess()
    {
        // Arrange
        var validFile = CreateMockFile("video.mp4", 100 * 1024 * 1024); // 100MB file with valid extension

        // Act
        var result = _validator.ValidateVideo(validFile);

        // Assert
        result.Success.Should().BeTrue();
        result.Errors.Should().BeEmpty();// No errors should be present
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