var IO = (function (module)
{

    if (typeof(module) === "undefined")
    {
        module = {};
    }
    
    // ------------------------
    // --- TextElement
    // ------------------------

    module.Chat = function (input, output)
    {
        var self = this;
        self._input = input;
        self._output = output;
    }
    
    // accessors
    // ------------------------
    
    Object.defineProperty(module.WSConnection.prototype, "input", 
    {
        get: function () { return this._input; },
        set: function (val) { this._input = val; },
        enumerable: false,
        configurable: false
    });
    
    Object.defineProperty(module.WSConnection.prototype, "output", 
    {
        get: function () { return this._output; },
        set: function (val) { this._output = val; },
        enumerable: false,
        configurable: false
    });
    
    // callbacks
    // ------------------------
    
    Object.defineProperty(module.WSConnection.prototype, "onSubmit", 
    {
        value: function () 
        {
            
        },
        enumerable: false,
        configurable: false,
        writable: false
    });

    return module;
    
}(IO));