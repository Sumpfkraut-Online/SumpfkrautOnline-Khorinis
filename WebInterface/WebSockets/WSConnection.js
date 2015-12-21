WebSockets = (function (module) 
{

    if (typeof(module) === "undefined")
    {
        module = {};
    }
    
    module.WSConnection = function (wsUri, input, output, submit)
    {        
        // var wsUri = "ws://localhost:81/";
        this.wsUri = wsUri;
        this.input = input;
        this.output = output;
        this.submit = submit;
    }
    
    // ------------------------
    // accessors
    // ------------------------
    
    Object.defineProperty(module.WSConnection.prototype, "input", 
    {
        get: function () { return this._input; },
        set: function (val) { this._input = val; },
        enumerable: false,
        configurable: false,
    });
    
    Object.defineProperty(module.WSConnection.prototype, "output", 
    {
        get: function () { return this._output; },
        set: function (val) { this._output = val; },
        enumerable: false,
        configurable: false,
    });
    
    Object.defineProperty(module.WSConnection.prototype, "submit", 
    {
        get: function () { return this._submit; },
        set: function (val) 
        {
            this._submit = val;
            this._submit.callbackTarget = this;
            this._submit.onclick = this.sendInputMessage;
        },
        enumerable: false,
        configurable: false,
    });
    
    Object.defineProperty(module.WSConnection.prototype, "websocket", 
    {
        get: function () { return this._websocket; },
        set: function (val) { this._websocket = val; },
        enumerable: false,
        configurable: false,
    });
    
    Object.defineProperty(module.WSConnection.prototype, "wsUri", 
    {
        get: function () { return this._wsUri; },
        set: function (val) { this._wsUri = val; },
        enumerable: false,
        configurable: false,
    });
    
    // ------------------------
    // triggered on websocket events
    // ------------------------
    
    Object.defineProperty(module.WSConnection.prototype, "_onClose", 
    {
        value: function (evt) 
        { 
            var target = this.callbackTarget;
            target.output.writenl('<span style="color: red;">' 
                + 'Closed websocket connection.</span> ' + evt.data); 
        },
        enumerable: false,
        configurable: false,
        writable: false
    });
    
    Object.defineProperty(module.WSConnection.prototype, "_onError", 
    {
        value: function (evt) 
        {
            var target = this.callbackTarget;
            target.output.writenl('<span style="color: red;">ERROR:</span> ' 
                + evt.data);
            if (target.websocket.readyState == 3)
            {
                target.output.writenl("Websocket-connection failed and/or closed.");
            }
            // this.calledObj.output.writenl('<span style="color: red;">ERROR:</span> ' 
                // + evt.data);
            // if (this.calledObj.websocket.readyState == 3)
            // {
                // this.calledObj.output.writenl("Websocket-connection failed and/or closed.");
            // }
        },
        enumerable: false,
        configurable: false,
        writable: false
    });
    
    Object.defineProperty(module.WSConnection.prototype, "_onOpen", 
    {
        value: function (evt) 
        {
            var target = this.callbackTarget;
            target.output.writenl("Websocket-connection to: " + target.wsUri);
        },
        enumerable: false,
        configurable: false,
        writable: false
    });
    
    Object.defineProperty(module.WSConnection.prototype, "_onMessage", 
    {
        value: function (evt) 
        {
            var target = this.callbackTarget;
            var jTxt = evt.data.toString();
            target.output.writenl(jTxt);
        },
        enumerable: false,
        configurable: false,
        writable: false
    });
    
    // ------------------------
    // additional methods
    // ------------------------
    
    Object.defineProperty(module.WSConnection.prototype, "init", 
    {
        value: function () 
        {
            this.output.writenl("Initializing WSConnection...");
            this.websocket = new WebSocket(this.wsUri);
            this.websocket.callbackTarget = this;
            this.websocket.binaryType = "arraybuffer";
            this.websocket.onclose = this._onClose;
            this.websocket.onerror = this._onError;
            this.websocket.onopen = this._onOpen;
            this.websocket.onmessage = this._onMessage;
        },
        enumerable: false,
        configurable: false,
        writable: false
    });
    
    Object.defineProperty(module.WSConnection.prototype, "sendMessage", 
    {
        value: function (msg) 
        { 
            if (this.websocket.readyState == 1)
            {
                this.websocket.send(msg);
                return true;
            }
            else
            {
                this.output.writenl("No websocket-connection established!");
                return false;
            }
        },
        enumerable: false,
        configurable: false,
        writable: false
    });
    
    Object.defineProperty(module.WSConnection.prototype, "sendInputMessage", 
    {
        value: function (msg) 
        {
            var target = this.callbackTarget;
            
            if (typeof(target.input) != "undefined")
            {
                var text = target.input.read();
                var jObj = {"protocolType":WebSockets.protocalTypes.ChatData,"rawText":text};
                var jTxt = JSON.stringify(jObj);
                target.sendMessage(jTxt);
            }
            else
            {
                alert("GOTCHA");
            }
        },
        enumerable: false,
        configurable: false,
        writable: false
    });
    
    return module;

}(WebSockets));