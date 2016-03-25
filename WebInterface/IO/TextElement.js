var IO = (function (module)
{

    if (typeof(module) === "undefined")
    {
        module = {};
    }
    
    // ------------------------
    // --- TextElement
    // ------------------------

    module.TextElement = function (contentAccess, newLine, maxLength)
    {
        var self = this;
        
        self.contentAccess = contentAccess;
        self.newLine = newLine;
        self.maxLength = maxLength;
        // relative line breaks at char-indices 
        // (always starts to count index number again after linebreak)
        self._relLineBreaks = new Array(0);
        self._endsWithNLString = false;
        
        self.onClear = undefined;
        self.onCut = undefined;
        self.onRead = undefined;
        self.onSubmit = undefined;
        self.onWrite = undefined;
        
        // io-methods
        // ------------------------
        
        self.clear = function ()
        {
            try
            {
                self.contentAccess[0][self.contentAccess[1]] = "";
                self._lineBreaks = new Array();
                
                if (typeof(self.onClear) == "function")
                {
                    self.onClear();
                }
            }
            catch (err)
            {
                alert("Invalid configuration of " + self._className 
                    + " in clear-function: " + err);
            }
        }
                
        self.cutAtMaxLength = function (cutLines = true, wholeLinesOnly = true)
        {
            try
            {
                if (self.contentAccess[0][self.contentAccess[1]].length <= self.maxLength)
                {
                    return 0;
                }
                
                var charDiff = self.contentAccess[0][self.contentAccess[1]].length 
                    - self.maxLength;
                if (charDiff < 1)
                {
                    return 0;
                }
                
                var from = to = 0;
                if (cutLines)
                {
                    var i = charCount = 0;
                    while (i < self._relLineBreaks.length)
                    {
                        charCount += self._relLineBreaks[i];
                        if (charCount >= charDiff)
                        {
                            break;
                        }
                        i++;
                    }
                    if (wholeLinesOnly)
                    {
                        // pointer goes to the end of the line before that line
                        if ((charCount > 0) && (!isNaN(self._relLineBreaks[i])))
                        {
                            to = charCount - self._relLineBreaks[i];
                        }
                        else
                        {
                            return 0;
                        }
                    }
                    else
                    {
                        // delete the same line completely
                        if (charCount <= 0)
                        {
                            to = self.contentAccess[0][self.contentAccess[1]].length - 1;
                        }
                        else
                        {
                            to = charCount;
                        }
                    }
                }
                else
                {
                    console.log("case 3");
                    to = charDiff;
                }
                    
                return self.cutFromTo(from, to);
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
                if (isNaN(from) || isNaN(to) || (to < from))
                {
                    // invalid parameters
                    return 0;
                }
                if (from < 0)
                {
                    from = 0;
                }
                if (to >= self.contentAccess[0][self.contentAccess[1]].length)
                {
                    to = self.contentAccess[0][self.contentAccess[1]].length;
                }
                
                if (from == 0)
                {
                    self.contentAccess[0][self.contentAccess[1]] = 
                        self.contentAccess[0][self.contentAccess[1]].substring(to);
                }
                else
                {
                    var strArr = self.contentAccess[0][self.contentAccess[1]].split('');
                    strArr.splice(from, to - from);
                    self.contentAccess[0][self.contentAccess[1]] = strArr.join('');
                }
                
                self.updateLineBreaks(from, to);
                
                if (typeof(self.onCut) == "function")
                {
                    self.onCut();
                }
                
                return (to - from + 1);
            }
            catch (err)
            {
                alert("Invalid configuration of " + self._className 
                    + " in cutFromTo-function: " + err);
                return 0;
            }
        }
        
        self.cutFromToLine = function (from, to) 
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
            
            return self.cutFromTo(cutStartIndex, cutEndIndex, true);
        }
        
        self.read = function ()
        {
            try
            {
                if (typeof(self.onRead) == "function")
                {
                    self.onRead();
                }
                
                return self.contentAccess[0][self.contentAccess[1]];
            }
            catch (err)
            {
                alert("Invalid configuration of " + self._className 
                    + " in read-function: " + err);
                return undefined;
            }
        }
        
        self.updateLineBreaks = function (cutFrom, cutTo)
        {
            if (isNaN(cutFrom) || isNaN(cutTo))
            {
                // recreate the linebreak-array from the getgo
                var renewedLineBreaks = Utilities.StringUtil.indicesOf(self.newLine, 
                    self.contentAccess[0][self.contentAccess[1]], true);
                var i = 0;
                while (i < renewedLineBreaks.length)
                {
                    renewedLineBreaks[i] += self.newLine.length;
                    i++;
                }
                    
                self._relLineBreaks = renewedLineBreaks;
            }
            else
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
        }
        
        self.write = function (msg)
        {
            try
            {               
                var newLineBreaks = Utilities.StringUtil.indicesOf(self.newLine, msg, false);
                
                if ((typeof(newLineBreaks) == "object") && (newLineBreaks.length > 0))
                {
                    
                    // correct line breaks to be at the end of newLine-string
                    var i = 0;
                    while (i < newLineBreaks.length)
                    {
                        newLineBreaks[i] += self.newLine.length;
                        i++;
                    }
                    
                    // there are new linebreaks in msg which must be dealt with
                    var endOfPrevLine = "";
                    if (!self._endsWithNLString)
                    {
                        // first prepare to append to existing line (splitting into previous
                        // -> endOfPreLine and new line -> msg)
                        endOfPrevLine = msg.substring(0, newLineBreaks[0]);
                        msg = msg.substring(newLineBreaks[0]);
                        self._relLineBreaks[self._relLineBreaks.length - 1] += 
                            newLineBreaks[0];
                        // remove already used linebreak from the new ones
                        newLineBreaks.splice(0, 1);
                    }
                    
                    if (msg.length == 0)
                    {
                        self._endsWithNLString = true;
                    }
                    else
                    {
                        self._endsWithNLString = false;
                        
                        var lineTail = msg.length;
                        if ((typeof(newLineBreaks) == "object") 
                            && (newLineBreaks.length > 0))
                        {
                            //                  |       |       == <br>
                            // index-space:  0 1 2 3 4 5 6 7 
                            // length-space: 1 2 3 4 5 6 7 8
                            //                          |    |  == tail
                            lineTail -= (newLineBreaks[newLineBreaks.length - 1]);
                        }
                        Array.prototype.push.apply(newLineBreaks, [lineTail]);
                    }
                    
                    Array.prototype.push.apply(self._relLineBreaks, newLineBreaks);
                }
                else
                {
                    // no new linebreaks to handle
                    if (self._endsWithNLString)
                    {
                        Array.prototype.push.apply(self._relLineBreaks, [msg.length]);
                    }
                    else
                    {
                        self._relLineBreaks[self._relLineBreaks.length - 1] += 
                            msg.length;
                    }
                    
                    self._endsWithNLString = false;
                }
                
                self.contentAccess[0][self.contentAccess[1]] += endOfPrevLine;
                self.contentAccess[0][self.contentAccess[1]] += msg;
                self.cutAtMaxLength(true, true);
                self.contentAccess[0].scrollTop = self.contentAccess[0].scrollHeight;
                
                if (typeof(self.onWrite) == "function")
                {
                    self.onWrite(endOfPrevLine + msg);
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
                self.write(self.newLine + msg);
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
    
    Object.defineProperty(module.TextElement.prototype, "contentAccess", 
    {
        get: function () { return this._contentAccess; },
        set: function (val) 
        {
            if (val.length < 2)
            {
                alert("Invalid contentAccess prodived to " + this._className 
                    + "-object: " + val);
                return;
            }
            this._contentAccess = val;
        },
        enumerable: true,
        configurable: false
    });
    
    Object.defineProperty(module.TextElement.prototype, "maxLength", 
    {
        get: function () { return this._maxLength; },
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

    return module;
    
}(IO));