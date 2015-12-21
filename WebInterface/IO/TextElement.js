var IO = (function (module)
{

    if (typeof(module) === "undefined")
    {
        module = {};
    }

    module.TextElement = function (domObject, txtAccess, newLine)
    {
        this.domObject = domObject;
        this.txtAccess = txtAccess;
        this.newLine = newLine;
    }
    
    // ------------------------
    // accessors
    // ------------------------
    
    Object.defineProperty(module.TextElement.prototype, "domObject", 
    {
        get: function () { return this._domObject; },
        set: function (val) { this._domObject = val; },
        enumerable: false,
        configurable: false
    });
    
    Object.defineProperty(module.TextElement.prototype, "newLine", 
    {
        get: function () { return this._newLine; },
        set: function (val) { this._newLine = val; },
        enumerable: false,
        configurable: false
    });
    
    Object.defineProperty(module.TextElement.prototype, "txtAccess", 
    {
        get: function () { return this._txtAccess; },
        set: function (val) { this._txtAccess = val; },
        enumerable: false,
        configurable: false
    });
    
    // ------------------------
    // io-methods
    // ------------------------
    
    Object.defineProperty(module.TextElement.prototype, "clear", 
    {
        value: function () 
        {
            try
            {
                this.domObject[this.txtAccess] = "";
            }
            catch (err)
            {
                alert("Invalid configuration of TextElement " + err);
            }
        },
        enumerable: false,
        configurable: false,
        writable: false
    });
    
    Object.defineProperty(module.TextElement.prototype, "read", 
    {
        value: function () 
        {
            try
            {
                return this.domObject[this.txtAccess];
            }
            catch (err)
            {
                alert("Invalid configuration of TextElement " + err);
                return undefined;
            }
        },
        enumerable: false,
        configurable: false,
        writable: false
    });
    
    Object.defineProperty(module.TextElement.prototype, "write", 
    {
        value: function (msg) 
        {
            try
            {
                this.domObject[this.txtAccess] = this.domObject[this.txtAccess] 
                    + msg;
            }
            catch (err)
            {
                alert("Invalid configuration of TextElement " + err);
                return undefined;
            }
        },
        enumerable: false,
        configurable: false,
        writable: false
    });
    
    Object.defineProperty(module.TextElement.prototype, "writenl", 
    {
        value: function (msg) 
        {
            try
            {
                this.domObject[this.txtAccess] = this.domObject[this.txtAccess] 
                    + this.newLine + msg;
            }
            catch (err)
            {
                alert("Invalid configuration of TextElement " + err);
                return undefined;
            }
        },
        enumerable: false,
        configurable: false,
        writable: false
    });

    return module;
    
}(IO));