var output = document.getElementById("output");

function debugMessage (msg)
{
    output.innerHTML += msg + "<br>";
}

function sendDebugInput ()
{
    var msg = document.getElementById("wsInputTextArea").value;
    sendMessage(msg);
    debugMessage("Did send message to server: " + msg);
    document.getElementById("wsInputTextArea").value = "";
}