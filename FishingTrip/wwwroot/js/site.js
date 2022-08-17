// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bun2dling-and-minification
// for details on configuring this project to bundle and minify static web assets.

function setDays() {
    const inputs = document.querySelectorAll("#checkDays input[type='checkbox']:checked");
    const daysToShow = [];   

    for (let i = 0; i < inputs.length; i++) {
        daysToShow[i] = (inputs[i].getAttribute('value')).substring(0,3);
    }

    return daysToShow;
}

function displayDay(day,daysToShow) {
    if (daysToShow.includes(day.getAttribute('id'))) {
        day.style.display = "block";
    } else {
        day.style.display = "none";
    }
}

function toggleForecast(daysToShow) {
    const avaliableData = document.getElementsByName("forecastDiv");

    for (let i = 0; i < avaliableData.length; i++) {
        displayDay(avaliableData[i], daysToShow);
    }
}

const timer = ms => new Promise(res => setTimeout(res, ms))

async function myLoop() {

    await timer(5000);
    searches = document.getElementsByName("emptySearchSpot");
    favourites = document.getElementsByName("emptyFavSpot");

    if (searches.length > 0) {
        for (let i = 0; i < searches.length; i++) {
            refreshDiv("#" + searches[i].value + "_Searched");
        }
    }

    if (favourites.length > 0) {
        for (let i = 0; i < favourites.length; i++) {
            refreshDiv("#" + favourites[i].value + "_Favourite");
        }
    }   

    const daysToShow = setDays();
    toggleForecast(daysToShow);

    searches = document.getElementsByName("emptySearchSpot");
    favourites = document.getElementsByName("emptyFavSpot");

    if (searches.length > 0 || favourites.length > 0) {
        myLoop();
    }
} 

function refreshDiv(divID) {
    console.log(" " + divID + " >*");
    $(divID).load(" " + divID + " >*");    
}

function indexLoad() {
    const daysToShow = setDays();
    toggleForecast(daysToShow);
    myLoop();
}

function newForecastDiv() {

    spot = document.getElementById('newSpotInput').value;
    if (document.getElementById("newSpotParent")) {
        alert("found");
    }
    var div = document.createElement("div");
    var loader = document.createElement("div");
    loader.appendChild(document.createElement("div"));
    loader.appendChild(document.createElement("div"));
    loader.appendChild(document.createElement("div"));
    loader.appendChild(document.createElement("div"));
    loader.setAttribute('class', 'lds-ellipse');

    div.setAttribute('class', 'forecastDiv');
    div.setAttribute('id', 'spot' + spot );
    div.appendChild(loader);
    
    document.getElementById("newSpotParent").appendChild(div);
}

