function VideoViewModel() {
    console.log('VideoViewModel');

    //Make the self as 'this' reference
    let self = this;
    //Declare observable which will be bind with UI
    self.Id = ko.observable("");
    self.FileName = ko.observable("");
    self.FileSize = ko.observable("");
    self.FilePath = ko.observable("");
    self.FileSizeInMb = ko.observable("");
    
    const VideoModel = {
        Id: self.Id,
        FileName: self.FileName,
        FileSize: self.FileSize,
        FilePath: self.FilePath,
        FileSizeInMb: self.FileSizeInMb
    };

    self.Videos = ko.observableArray(); // Contains the list of videos

    // Initialize the view-model
    $.ajax({
        url: 'Home/GetAllVideos',
        cache: false,
        type: 'GET',
        contentType: 'application/json; charset=utf-8',
        data: {},
        success: function (data) {
            self.Videos(data); //Put the response in ObservableArray
        }
    });
}
const viewModel = new VideoViewModel();
ko.applyBindings(viewModel);