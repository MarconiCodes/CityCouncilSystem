"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/ChatHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
    //var cid = $.connection.hub.id;
    //document.getElementById("message").value = cid;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var collection = document.getElementsByName("Content");
    var message = collection[0].value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    //var cid = $.connection.hub.id;
    //document.getElementById("message").value = cid;
    event.preventDefault();
});