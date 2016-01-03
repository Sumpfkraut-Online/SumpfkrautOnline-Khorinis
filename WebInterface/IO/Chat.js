var IO = (function (module)
{

    if (typeof(module) === "undefined")
    {
        module = {};
    }
    
    // ------------------------
    // --- Chat
    // ------------------------

    module.Chat = function ()
    {
        var self = this;
    
        self.onSubmit = function (evt)
        {
            if (typeof(self.input) != "undefined")
            {
                var text = self.input.read();
                var jObj = {"protocolType":WebSockets.protocalTypes.chatData,"rawText":text};
                var jTxt = JSON.stringify(jObj);
                self.wsConnection.sendMessage(jTxt);
            }
            else
            {
                var warnMsg = "Warning: self.input is undefined: Cannot send input message"
                    + " to websocket-server!";
                if (typeof(self.output) != "undefined")
                {
                    self.output.writeln(warnMsg);
                }
                else
                {
                    alert(warnMsg);
                }
            }
        };
    }
    
    Object.defineProperty(module.Chat.prototype, "_className", 
    {
        value: "Chat",
        enumerable: true,
        configurable: false,
        writable: false
    });
    
    // accessors
    // ------------------------
    
    Object.defineProperty(module.Chat.prototype, "input", 
    {
        get: function () { return this._input; },
        set: function (val) { this._input = val; },
        enumerable: true,
        configurable: false
    });
    
    Object.defineProperty(module.Chat.prototype, "output", 
    {
        get: function () { return this._output; },
        set: function (val) { this._output = val; },
        enumerable: true,
        configurable: false
    });
    
    Object.defineProperty(module.Chat.prototype, "submitHandler", 
    {
        get: function () { return this._submitHandler; },
        set: function (val) 
        {
            if ((typeof(this._submitHandler) == "object") 
                && (this._submitHandler._className == "CallbackHandler"))
            {
                this._submitHandler.removeReceiver(this.onSubmit);
            }
        
            if (val._className == "CallbackHandler")
            {
                this._submitHandler = val;
                this._submitHandler.addReceiver(this.onSubmit);
            }
            else
            {
                this._submitHandler = undefined;
            }
        },
        enumerable: true,
        configurable: false
    });
    
    Object.defineProperty(module.Chat.prototype, "wsConnection", 
    {
        get: function () { return this._wsConnection; },
        set: function (val) { this._wsConnection = val; },
        enumerable: true,
        configurable: false
    });

    return module;
    
}(IO));