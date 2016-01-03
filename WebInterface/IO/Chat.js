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
    
        self.onMessage = function (evt)
        {
            var jsonStr = evt.data.toString();
            var jsonObj = JSON.parse(jsonStr);
            
            if (typeof(jsonObj) != "object")
            {
                // no need to waste ressources when there 
                // is nothing valid to process
                return;
            }
            
            if ((typeof(jsonObj.sender) == "undefined") || (jsonObj.sender.length < 1))
            {
                jsonObj.sender = "SERVER";
            }
            
            self.output.writeln(jsonObj.sender + " > " + jsonObj.rawText);
        }
        
        self.onSubmit = function (evt)
        {
            if (typeof(self.input) != "undefined")
            {
                var rawText = self.input.read();
                
                var type = WebSockets.protocalTypes.chatData;
                var sender = "CLIENT";
                var cmds;
                
                // parse the raw text-input and add chatroom-indication-command
                // if necessary and not specified
                var startOfCmds = rawText.indexOf("/");
                cmds = self.stringToCommands(rawText);
                
                if (typeof(cmds) == "undefined")
                {
                    cmds = new Array();
                }
                if (startOfCmds != 0)
                {
                    // input doesn't begin with a command
                    // send everything before as global chat-message by default
                    cmds.splice(0, 0, ["g", rawText.substring(0, startOfCmds)]);
                }
                
                var jObj = 
                {
                    "type"      : type, 
                    "sender"    : sender,
                    "cmds"      : cmds
                };
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
        
        self.filterInvalidParam = function (val)
        {
            if (val == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        
        self.stringToCommands = function (rawText)
        {
            var arr = rawText.split(" ");
            arr = arr.filter(self.filterInvalidParam);
            return arr;
        }
    
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
    
    Object.defineProperty(module.Chat.prototype, "submit", 
    {
        get: function () { return this._submit },
        set: function (val) 
        {
            if ((typeof(this._submit) == "object") 
                && (this._submit._className == "CallbackHandler"))
            {
                this._submit.removeReceiver(this.onSubmit);
            }
        
            if (val._className == "CallbackHandler")
            {
                this._submit = val;
                this._submit.addReceiver(this.onSubmit);
            }
            else
            {
                this._submit = undefined;
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
    
    Object.defineProperty(module.Chat.prototype, "wsConnOnMessage", 
    {
        get: function () { return this._wsConnOnMessage; },
        set: function (val) 
        {
            if ((typeof(this._wsConnOnMessage) == "object") 
                && (this._wsConnOnMessage._className == "CallbackHandler"))
            {
                this._wsConnOnMessage.removeReceiver(this.onMessage);
            }
        
            if (val._className == "CallbackHandler")
            {
                this._wsConnOnMessage = val;
                this._wsConnOnMessage.addReceiver(this.onMessage);
            }
            else
            {
                this._wsConnOnMessage = undefined;
            }
        },
        enumerable: true,
        configurable: false
    });

    return module;
    
}(IO));