var WebSockets = (function (module)
{
    if (typeof(module) == "undefined")
    {
        module = {};
    }
    
    // ------------------------
    // --- WSProtocol-class
    // ------------------------
    
    module.WSProtocol = function (protocolType, sender, data, dataType)
    {
        this.protocolType = protocolType;
        this.sender = sender;
        this.data = data;
        this.dataType = dataType;
    }
    
    // accessors
    // ------------------------
    
    Object.defineProperty(module.WSProtocol.prototype, "_className", 
    {
        value: "WSProtocoll",
        enumerable: false,
        configurable: false,
        writable: false
    });
    
    Object.defineProperty(module.WSProtocol.prototype, "data", 
    {
        get: function () { return this._data; },
        set: function (val) { this._data = val; },
        enumerable: false,
        configurable: false,
    });
    
    Object.defineProperty(module.WSProtocol.prototype, "dataType", 
    {
        get: function () { return this._dataType; },
        set: function (val) 
        {
            if (val in module.dataTypes)
            {
                this._dataType = val;
            }
            else
            {
                this._dataType = module.dataTypes.undefined;
            }
        },
        enumerable: false,
        configurable: false,
    });
    
    Object.defineProperty(module.WSProtocol.prototype, "protocolType", 
    {
        get: function () { return this._protocolType; },
        set: function (val) 
        {
            if (val in module.protocalTypes)
            {
                this._protocolType = val;
            }
            else
            {
                this._protocolType = module.protocalTypes.undefined;
            }
        },
        enumerable: false,
        configurable: false,
    });
    
    Object.defineProperty(module.WSProtocol.prototype, "sender", 
    {
        get: function () { return this._sender; },
        set: function (val) { this._sender = val; },
        enumerable: false,
        configurable: false,
    });
    
    return module;
    
}(WebSockets));