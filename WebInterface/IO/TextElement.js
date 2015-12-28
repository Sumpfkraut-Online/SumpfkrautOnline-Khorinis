var IO = (function (module)
{

    if (typeof(module) === "undefined")
    {
        module = {};
    }
    
    // ------------------------
    // --- TextElement
    // ------------------------

    module.TextElement = function (domObject, txtAccess, newLine, maxLength)
    {
        var self = this;
        
        self.domObject = domObject;
        self.txtAccess = txtAccess;
        self.newLine = newLine;
        self.maxLength = maxLength;
        // relative line breaks at char-indices 
        // (always starts to count index number again after linebreak)
        self._relLineBreaks = new Array();
        
        // io-methods
        // ------------------------
        
        self.clear = function ()
        {
            try
            {
                self.domObject[self.txtAccess] = "";
                self._lineBreaks = new Array();
            }
            catch (err)
            {
                alert("Invalid configuration of " + self._className 
                    + " in clear-function: " + err);
            }
        }
        
        self.updateLinebreaks = function (cutFrom, cutTo)
        {
            if ((typeof(cutFrom) == "number") && (typeof(cutTo) == "number"))
            {
                // update existing linebreak-array
                var cutDiff = cutTo - cutFrom;
                if (cutDiff < 0)
                {
                    // negative range is invalid
                    return;
                }
                
                var cutStartIndex, cutEndIndex;
                var charCount = 0;
                var i = 0;
                
                while (i < self._relLineBreaks.length)
                {
                    charCount += self._relLineBreaks[i];
                    if (charCount >= cutFrom)
                    {
                        cutStartIndex = i;
                        break;
                    }
                    i++;
                }
                
                i = cutEndIndex = cutStartIndex;
                while (i < self._relLineBreaks.length)
                {
                    charCount += self._relLineBreaks[i];
                    if (charCount > cutTo)
                    {
                        break;
                    }
                    cutEndIndex = i;
                    i++;
                }
                
                self._relLineBreaks.splice(cutStartIndex, 
                    (cutEndIndex - cutStartIndex) + 1);
            }
            else
            {
                // recreate the linebreak-array from teh getgo
                self._relLineBreaks = Utilities.StringUtil.indicesOf(self.newLine, 
                    self.domObject[self.txtAccess], true);
            }
        }
        
        self.cutAtMaxLength = function (preserveLines)
        {
            try
            {
                if (self.domObject[self.txtAccess].length > self.maxLength)
                {
                    
                }
            }
            catch (err)
            {
                alert("Invalid configuration of " + self._className 
                    + " in cutAtMaxLength-function: " + err);
                return -1;
            }
        }
        
        self.cutFromTo = function (from, to)
        {
            try
            {
                if (to < from)
                {
                    return 0;
                }
                if (from < 0)
                {
                    from = 0;
                }
                if (to >= self.domObject[self.txtAccess].length)
                {
                    to = self.domObject[self.txtAccess].length;
                }
                
                self.domObject[self.txtAccess] = self.domObject[self.txtAccess].substring(
                    from, to);
                self.updateLineBreaks(from, to);
                
                return (to - from + 1);
            }
            catch (err)
            {
                alert("Invalid configuration of " + self._className 
                    + " in cutFromTo-function: " + err);
                return 0;
            }
        }
        
        self.cutFromToLine (from, to) 
        {
            if ((typeof(from) != "number") || (typeof(to) != "number") 
                || ((to - from) < 0))
            {
                // no valid parameters
                return 0;
            }
            
            var cutStartIndex, cutEndIndex;
            var i = charCount = 0;
            while (i < self._relLineBreaks.length)
            {
                charCount += self._relLineBreaks[i];
                cutEndIndex = charCount;
                if (i == from)
                {
                    cutStartIndex = charCount;
                }
                if (i == to)
                {
                    // reached last line to cut out
                    break;
                }
                i++;
            }
            
            return self.cutFromTo(cutStartIndex, cutEndIndex);
        }
        
        self.read = function ()
        {
            try
            {
                return self.domObject[self.txtAccess];
            }
            catch (err)
            {
                alert("Invalid configuration of " + self._className 
                    + " in read-function: " + err);
                return undefined;
            }
        }
        
        self.write = function (msg)
        {
            try
            {
                self.domObject[self.txtAccess] = self.domObject[self.txtAccess] 
                    + msg;
                var newLineBreaks = Utilities.StringUtil.indicesOf("\n", msg, );
                if ((typeof(newLineBreaks) == "array") && (newLineBreaks.length > 0))
                {
                    Array.prototype.push.apply(self._lineBreaks, newLineBreaks);
                }
            }
            catch (err)
            {
                alert("Invalid configuration of " + self._className 
                    + " in write-function: " + err);
            }
        }
        
        self.writeln = function (msg)
        {
            try
            {
                self.domObject[self.txtAccess] = self.domObject[self.txtAccess] 
                    + self.newLine + msg;
            }
            catch (err)
            {
                alert("Invalid configuration of " + self._className 
                    + " in writeln-function: " + err);
            }
        }
    }
    
    // accessors
    // ------------------------
    
    Object.defineProperty(module.TextElement.prototype, "_className", 
    {
        value: "TextElement",
        enumerable: true,
        configurable: false,
        writable: false
    });
    
    Object.defineProperty(module.TextElement.prototype, "domObject", 
    {
        get: function () { return this._domObject; },
        set: function (val) { this._domObject = val; },
        enumerable: true,
        configurable: false
    });
    
    Object.defineProperty(module.TextElement.prototype, "maxLength", 
    {
        get: function () { return this.maxLength; },
        set: function (val) { this._maxLength = val; },
        enumerable: true,
        configurable: false
    });
    
    Object.defineProperty(module.TextElement.prototype, "newLine", 
    {
        get: function () { return this._newLine; },
        set: function (val) { this._newLine = val; },
        enumerable: true,
        configurable: false
    });
    
    Object.defineProperty(module.TextElement.prototype, "txtAccess", 
    {
        get: function () { return this._txtAccess; },
        set: function (val) { this._txtAccess = val; },
        enumerable: true,
        configurable: false
    });

    return module;
    
}(IO));