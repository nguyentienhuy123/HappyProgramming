"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = ${ user } : ${ message };
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error("log1", err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var sender = document.getElementById("senderInput").value;
    var receiver = document.getElementById("receiverInput").value;
    var receiver2 = document.getElementById("receiverInput2").value;
    var message = document.getElementById("messageInput").value;

    if (receiver != "") {

        connection.invoke("SendMessageToGroup", sender, receiver, message, receiver, receiver2).catch(function (err) {
            return console.error(err.toString());
            console.log('send mess');
        });
    }
    else {
        connection.invoke("SendMessage", sender, message).catch(function (err) {
            return console.error(err.toString());
        });
    }
    event.preventDefault();
});

document.getElementById("sendUser").addEventListener("click", function (event) {
    var sender = document.getElementById("senderInput").value;
    var receiver = document.getElementById("receiverInput").value;
    var message = document.getElementById("messageInput").value;

    connection.invoke("SendMessageToUser", receiver, message).catch(function (err) {
        return console.error(err.toString());
    });
});

//document.getElementById("joinGroup").addEventListener("click", function (event) {
//    var receiver = document.getElementById("receiverInput").value;
//    debugger;
//    if (receiver != "") {

//        connection.invoke("AddToGroup", receiver).catch(function (err) {
//            console.log('join group');

//            return console.error(err.toString());
//        });
//    }
//    event.preventDefault();
//});

//var joinGroupButton = document.getElementById("joinGroup");
var receiverInput = document.getElementById("receiverInput");

function joinGroup() {
    var receiver = receiverInput.value;
    if (receiver != "") {
        connection.invoke("AddToGroup", receiver).catch(function (err) {
            return console.error(err.toString());
        });
    }
}

// Trigger the click event programmatically
joinGroup();


// add to group
$(document).ready(function () {
    var receiver = receiverInput.value;
    connection.invoke("AddToGroup", receiver).catch(function (err) {
        return console.error(err.toString());
    });
});