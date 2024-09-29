function VideoViewModel() {
    //Make the self as 'this' reference
    const self = this;
    
    self.selectedVideo = ko.observable(); // Observable to track the currently selected video
    self.VideoModels = ko.observableArray(); // Contains the list of videos
    // Indicates whether data is still loading
    self.isLoading = ko.observable(true);

    // Observable for the currently selected file name
    self.selectedFileName = ko.observable(""); // Empty by default
    
    // Initialize the view-model
    $.ajax({
        url: 'Home/GetAllVideos',
        cache: false,
        type: 'GET',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            self.VideoModels(data); //Put the response in ObservableArray
            self.isLoading(false); // Data has been loaded
        },
        error: function (err) {
            console.log(err);
            self.isLoading(false);
        }
    });

    // Function to select the video and play it
    self.selectVideo = function(video) {
        // Update the selected file name observable
        self.selectedFileName(video.fileNameWithoutExtension); // Update the file name to the selected video's file name
        self.selectedVideo(video);  // Set the clicked video as the selected video
        playVideo(video.filePath);  // Call the playVideo function to play the selected video
    };
}
const viewModel = new VideoViewModel();
ko.applyBindings(viewModel);