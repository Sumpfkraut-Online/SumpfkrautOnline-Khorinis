WebSockets = (function (module) 
{

    if (typeof(module) === "undefined")
    {
        module = {};
    }
    
    // ------------------------
    // --- WSConnection-class
    // ------------------------
    
    module.WSConnection = function (wsUri, input, output, submit)
    {
        var self = this;
        
        // var wsUri = "ws://localhost:81/";
        self.wsUri = wsUri;
        self.input = input;
        self.output = output;
        self.submit = submit;
        
        // keep callback function in constructor closure to have access to self

        // websocket callbacks
        // ------------------------
        
        self.onClose = function (evt)
        {
            self.output.writenl('<span style="color: red;">' 
                + 'Closed websocket connection.</span> ' + evt.data);
        };
        
        self.onError = function (evt)
        {
            self.output.writenl('<span style="color: red;">Error:</span> ' 
                + evt.data);
            if (self.websocket.readyState == 3)
            {
                self.output.writenl("Websocket-connection failed and/or closed.");
            }
        };
        
        self.onMessage = function (evt)
        {
            var jTxt = evt.data.toString();
            self.output.writenl(jTxt);
        };
        
        self.onOpen = function (evt)
        {
            self.output.writenl("Websocket-connection to: " + self.wsUri);
        };
        
    }
    
    // accessors
    // ------------------------
    
    Object.defineProperty(module.WSConnection.prototype, "_className", 
    {
        value: "WSConnection",
        enumerable: true,
        configurable: false,
        writable: false
    });
    
    Object.defineProperty(module.WSConnection.prototype, "input", 
    {
        get: function () { return this._input; },
        set: function (val) { this._input = val; },
        enumerable: true,
        configurable: false,
    });
    
    Object.defineProperty(module.WSConnection.prototype, "output", 
    {
        get: function () { return this._output; },
        set: function (val) { this._output = val; },
        enumerable: true,
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
        enumerable: true,
        configurable: false,
    });
    
    Object.defineProperty(module.WSConnection.prototype, "websocket", 
    {
        get: function () { return this._websocket; },
        set: function (val) { this._websocket = val; },
        enumerable: true,
        configurable: false,
    });
    
    Object.defineProperty(module.WSConnection.prototype, "wsUri", 
    {
        get: function () { return this._wsUri; },
        set: function (val) { this._wsUri = val; },
        enumerable: true,
        configurable: false,
    });
    
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
            this.websocket.onclose = this.onClose;
            this.websocket.onerror = this.onError;
            this.websocket.onopen = this.onOpen;
            this.websocket.onmessage = this.onMessage;
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
        enumerable: true,
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
                var jObj = {"protocolType":WebSockets.protocalTypes.chatData,"rawText":text};
                var jTxt = JSON.stringify(jObj);
                target.sendMessage(jTxt);
            }
            else
            {
                var warnMsg = "Warning: target.input is undefined: Cannot send input message"
                    + " to websocket-server!";
                if (typeof(target.output) != "undefined")
                {
                    target.output.writeln(warnMsg);
                }
                else
                {
                    alert(warnMsg);
                }
            }
        },
        enumerable: true,
        configurable: false,
        writable: false
    });
    
    return module;

}(WebSockets));