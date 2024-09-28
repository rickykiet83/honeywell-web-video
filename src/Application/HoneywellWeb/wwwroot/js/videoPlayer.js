function playVideo(filePath) {
    // Get the video player and source elements
    const videoPlayer = document.getElementById('videoPlayer');
    const videoSource = document.getElementById('videoSource');

    // Update the video source
    videoSource.src = filePath;

    // Load the new video
    videoPlayer.load();

    // Play the video
    videoPlayer.play();
}