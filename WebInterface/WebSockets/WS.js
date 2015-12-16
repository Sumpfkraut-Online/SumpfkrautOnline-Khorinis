WebSockets.WS = (function (self) 
{

    if (typeof(self) === "undefined")
    {
        self = {};
    }
    
    var debugMessage = Debug.debugMessage;

    var wsUri = "ws://localhost:81/";

    var protocalTypes = 
    {
        Unknown:"Unknown",
        UserData:"UserData",
        ChatData:"ChatData",
        VobData:"VobData"
    };

    websocket = new WebSocket(wsUri);
    websocket.binaryType = "arraybuffer";
    websocket.onclose = function (evt) { self.onClose };
    websocket.onerror = function(evt) { self.onError(evt) };
    websocket.onopen = function (evt) { self.onOpen(evt) };
    websocket.onmessage = function (evt) { self.onMessage(evt) };

    var inputElement;
    self.setInputElement = function (input)
    {
        // if (typeof(input) === "undefined")
        // {
            // return false;
        // }
        // else if ()
        // inputElement = 
    }

    self.onClose = function (evt)
    {
        debugMessage('<span style="color: red;">' 
            + 'Closed websocket connection.</span> ' + evt.data);
    }

    self.onError = function (evt)
    {
        debugMessage('<span style="color: red;">ERROR:</span> ' + evt.data);
        if (websocket.readyState == 3)
        {
            debugMessage("Websocket-connection failed and/or closed.");
        }
    }

    self.onOpen = function (evt)
    {
        debugMessage("Websocket-connection to: " + wsUri);
    }

    self.onMessage = function (evt)
    {
        var jTxt = evt.data.toString();
        debugMessage(jTxt);
    }

    self.sendMessage = function (msg)
    {
        if (websocket.readyState == 1)
        {
            websocket.send(msg);
            return true;
        }
        else
        {
            debugMessage("No websocket-connection established!");
            return false;
        }
    }

    self.sendInputMessage = function ()
    {
        var source = document.getElementById("inputTextArea");
        if (typeof(source) != undefined)
        {
            var text = source.value;
            var jObj = {"protocolType":protocalTypes.ChatData,"rawText":text};
            var jTxt = JSON.stringify(jObj);
            // debugMessage(text);
            self.sendMessage(jTxt);
        }
    }

    self.sendDebugMessage = function ()
    {
        var jObj = {"uid":"A4215674B","t":1,"mov":3};
        var jTxt = JSON.stringify(jObj);
        debugMessage(jTxt);
        if (self.sendMessage(jTxt))
        {
            debugMessage("Sent test-message.");
        }
    }

    return self;

}(WebSockets.WS));