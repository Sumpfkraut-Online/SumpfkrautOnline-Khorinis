//var wsUri = "ws://" + document.location.host + document.location.pathname + "/chat";
// var wsUri = "ws://" + document.location.host + "/";
var wsUri = "ws://localhost:81/";
debugMessage(wsUri);

var websocket = new WebSocket(wsUri);
websocket.binaryType = "arraybuffer";
websocket.onerror = function(evt) { onError(evt) };
websocket.onopen = function (evt) { onOpen(evt) };

function onError (evt)
{
    debugMessage('<span style="color: red;">ERROR:</span> ' + evt.data);
}

function onOpen (evt)
{
    debugMessage("Connected to " + wsUri);
    // debugMessage("GOTCHA!");
}

websocket.onmessage = function (evt) { onMessage(evt) };

function onMessage (evt)
{
//    var obj = JSON.parse(evt.data);
//    var str = JSON.stringify(obj, undefined, 2);
    
//    var arr = Array.prototype.slice.call(evt.data.valueOf());

//    var arr = evt.data.split(",");
    
//    debugMessage(evt.toString());
//    debugMessage(evt.data.toString());
//    debugMessage(typeof(evt.data));
//    debugMessage(evt.data instanceof ArrayBuffer);
//    debugMessage(sizeof(evt.data));
//    debugMessage(typeof(obj));
//    debugMessage(str);
//    debugMessage(arr[0]);
//    debugMessage(evt.data);
//    debugMessage(sizeof(evt.data));
    
//    var arrayBuffer;
//    var fileReader = new FileReader();
//    fileReader.onload = function() {
//        arrayBuffer = this.result;
//    };
//    fileReader.readAsArrayBuffer(evt.data);
//    debugMessage(fileReader.result);

    

//    var dataBytes = new Uint8Array(evt.data);
//    debugMessage(dataBytes.length);
//    for (var i = 0; i < dataBytes.length; i++) 
//    {
//        debugMessage(i + ": " + dataBytes[i]);
//    }
//    debugMessage(ab2str(dataBytes));

    var txt = evt.data.toString();
    debugMessage(txt);
    // var obj = JSON.parse(evt.data);
    // debugMessage(txt);
    // debugMessage(obj);
    // debugMessage(obj.attr_1);
    
    
}

function sendMessage (msg)
{
    websocket.send(msg);
}

function sendDebugMessage ()
{
    var jObj = {"uid":"A4215674B","t":1,"mov":3};
//    var jTxt = JSON.parse(jObj);
    var jTxt = JSON.stringify(jObj);
    debugMessage(jTxt);
    websocket.send(jTxt);
    debugMessage("Sent test-message.");
}
