WebSockets = (function (module) 
{

    if (typeof(module) === "undefined")
    {
        module = {};
    }
    
    // ------------------------
    // --- WSConnection-class
    // ------------------------
    
    module.WSConnection = function (wsUri, output)
    {
        var self = this;
        
        self.wsUri = wsUri;
        self.output = output;
        
        // keep callback function in constructor closure to have access to self

        // websocket callbacks
        // ------------------------
        
        self._onClose = function (evt)
        {
            self.output.writeln('<span style="color: red;">' 
                + 'Closed websocket connection.</span> ' + evt.data);
                
            if (typeof(self.onClose) == "function")
            {
                self.onClose(evt);
            }
        };
        
        self._onError = function (evt)
        {
            self.output.writeln('<span style="color: red;">Error:</span> ' 
                + evt.data);
                
            if (self.websocket.readyState == 3)
            {
                self.output.writeln("Websocket-connection failed and/or closed.");
            }
            
            if (typeof(self.onError) == "function")
            {
                self.onError(evt);
            }
        };
        
        self._onMessage = function (evt)
        {
            // var jTxt = evt.data.toString();
            if (typeof(self.onMessage) == "function")
            {
                self.onMessage(evt);
            }
        };
        
        self._onOpen = function (evt)
        {
            self.output.writeln("Websocket-connection to: " + self.wsUri);
            
            if (typeof(self.onOpen) == "function")
            {
                self.onOpen(evt);
            }
        };
        
        // own eventlisteners which can be used to receive data 
        // from this websocket-connection
        // ------------------------
        
        self.onClose = null;
        self.onError = null;
        self.onMessage = null;
        self.onOpen = null;
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
    
    Object.defineProperty(module.WSConnection.prototype, "output", 
    {
        get: function () { return this._output; },
        set: function (val) { this._output = val; },
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
            this.output.writeln("Initializing WSConnection...");
            this.websocket = new WebSocket(this.wsUri);
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
                this.output.writeln("No websocket-connection established!");
                return false;
            }
        },
        enumerable: true,
        configurable: false,
        writable: false
    });
    
    return module;

}(WebSockets));