﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bun2dling-and-minification
// for details on configuring this project to bundle and minify static web assets.

//const { each } = require("jquery");

//import { ajax, get } from "jquery";

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

////Z: \Programming Projects\FishingTrip\FishingTrip\Pages\Shared\_Common.cs
////Z: \Programming Projects\FishingTrip\FishingTrip\wwwroot\js\site.js
function refreshDiv(divID) {
    console.log(divID);;
    $(divID).load(location.href +" "+divID);

}

async function indexLoad() {
    const daysToShow = setDays();
    toggleForecast(daysToShow);
    //console.log(document.getElementsByName("emptySearchSpot")[0].value);    
    if(document.getElementsByName("emptySearchSpot").length > 0) {
        setTimeout(() => {
            for(let i = 0; i < document.getElementsByName("emptySearchSpot").length; i++) {
                //console.log("Refreshing");
                //console.log(refreshSearchSpot(document.getElementsByName('emptySearchSpot')[i].value));
                refreshDiv("#"+document.getElementsByName("emptySearchSpot")[i].value + "_Searched");                
            }
            indexLoad();
        }, 5000);        
    };
    //spotForecastScript();
}

function forecastDicToDiv() {

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


    //document.write(spot);
    //Method to see if this spot (and its forecast) exist in the favSpotForecasts databases
    //the _Common/getFavConditions(string website, string spot, string[] days) method can be used

    // First check is to see if spot exits on the current favourite list

    //$.ajax({
    //    type: 'get',
    //    url: '?handler=FindSpot',
    //    data: { spot: spot },
    //    contentType: 'application/json',
    //    success: function(spot) {
    //        alert(spot);
    //    },
    //    //error: function (error) {
    //    //    alert("Error: + error");
    //    //}
    //})



    //new method to 
    

    

}

