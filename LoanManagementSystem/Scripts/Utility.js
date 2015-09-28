$(document).ready(function () {
    var datepc = $('.date-picker');
    if(datepc!=null){
        datepc.datepicker({
            format: 'yyyy.M.dd',
            //startDate: '-3d',
            //showOn: "both",
            buttonImageOnly: true,
            buttonImage: "calendar.gif",
            buttonText: "Calendar",
            regional: ["en-US"]
        });
    }
});