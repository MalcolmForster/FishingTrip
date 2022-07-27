// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bun2dling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function setDays() {
    const inputs = document.querySelectorAll("#checkDays input[type='checkbox']:checked");
    const daysToShow = [];   

    for (let i = 0; i < inputs.length; i++) {
        daysToShow[i] = (inputs[i].getAttribute('value')).substring(0,3);
    }

    return daysToShow;

}

function toggleForecast(daysToShow) {
    const avaliableData = document.getElementsByName("forecastDiv");
    for (let i = 0; i < avaliableData.length; i++) {
        var s = avaliableData[i].getAttribute('id');
        if (daysToShow.includes(s)) {
            avaliableData[i].style.display = "block";
        } else {
            avaliableData[i].style.display = "none";
        }
    }
}

function indexLoad() {
    const daysToShow = setDays();
    toggleForecast(daysToShow);
}

function newForecastDiv() {

    spot = document.getElementById('newSpotInput').value;
    document.write(spot);
    //Method to see if this spot (and its forecast) exist in the favSpotForecasts databases
    //the _Common/getFavConditions(string website, string spot, string[] days) method can be used
    

    

}