function VideoViewModel() {
    //Make the self as 'this' reference
    let self = this;
    //Declare observable which will be bind with UI
    self.id = ko.observable("");
    self.fileName = ko.observable("");
    self.fileSize = ko.observable("");
    self.filePath = ko.observable("");
    self.fileSizeInMb = ko.observable("");

    // Observable to track the currently selected video
    self.selectedVideo = ko.observable();
    
    const VideoModel = {
        id: self.id,
        fileName: self.fileName,
        fileSize: self.fileSize,
        filePath: self.filePath,
        fileSizeInMb: self.fileSizeInMb
    };

    self.VideoModel = ko.observableArray(); // Contains the video details
    self.VideoModels = ko.observableArray(); // Contains the list of videos

    // Initialize the view-model
    $.ajax({
        url: 'Home/GetAllVideos',
        cache: false,
        type: 'GET',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            self.VideoModels(data); //Put the response in ObservableArray
        },
        error: function (err) {
            console.log(err);
        }
    });

    // Function to select the video and play it
    self.selectVideo = function(video) {
        self.selectedVideo(video);  // Set the clicked video as the selected video
        playVideo(video.filePath);  // Call the playVideo function to play the selected video
    };
}
const viewModel = new VideoViewModel();
ko.applyBindings(viewModel);