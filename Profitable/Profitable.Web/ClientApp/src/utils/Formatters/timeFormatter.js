var months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
var days = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];

export const convertFullDateTime = (datetime) => {
    var day = days[datetime.getDay()];
    var hr = datetime.getHours();
    var min = datetime.getMinutes();
    if (min < 10) {
        min = "0" + min;
    }
    var ampm = "AM";
    if (hr > 12) {
        hr -= 12;
        ampm = "PM";
    }
    var date = datetime.getDate();
    var month = months[datetime.getMonth()];
    var year = datetime.getFullYear();
    return day + ", " + date + " " + month + " " + year + " " + hr + ":" + min + " " + ampm;
};
