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
        self._relLineBreaks = new Array(0);
        self._endsWithNLString = false;
        
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
                
        self.cutAtMaxLength = function (cutLines = true, wholeLinesOnly = true)
        {
            try
            {
                if (self.domObject[self.txtAccess].length <= self.maxLength)
                {
                    return 0;
                }
                
                var charDiff = self.domObject[self.txtAccess].length - self.maxLength;
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
                            console.log("case 1a");
                            // console.log(self.domObject[self.txtAccess].length + " " 
                                // + charDiff + " " + charCount);
                            // console.log(self._relLineBreaks[i]);
                            // console.log(to + " " + charCount + " "  + self._relLineBreaks[i]);
                        }
                        else
                        {
                            console.log("case 1b");
                            return 0;
                        }
                    }
                    else
                    {
                        // delete the same line completely
                        if (charCount <= 0)
                        {
                            console.log("case 2a");
                            to = self.domObject[self.txtAccess].length - 1;
                        }
                        else
                        {
                            console.log("case 2b");
                            to = charCount;
                        }
                    }
                }
                else
                {
                    console.log("case 3");
                    to = charDiff;
                }
                
                // console.log(self._relLineBreaks);
                // console.log(from + " " + to + " " + charDiff + " " + self.maxLength 
                    // + " " + self.domObject[self.txtAccess].length);
                    
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
                if (to >= self.domObject[self.txtAccess].length)
                {
                    to = self.domObject[self.txtAccess].length;
                }
                
                if (from == 0)
                {
                    self.domObject[self.txtAccess] = self.domObject[self.txtAccess].substring(
                        to);
                }
                else
                {
                    var strArr = self.domObject[self.txtAccess].split('');
                    strArr.splice(from, to - from);
                    self.domObject[self.txtAccess] = strArr.join('');
                }
                
                self.updateLineBreaks(from, to);
                
                return (to - from + 1);
                
                // if (cutWholeLines)
                // {
                    // var newFrom, newTo;
                    // var i = charCount = 0;
                    // while (i < self._relLineBreaks.length)
                    // {
                        // charCount += self._relLineBreaks[i];
                        // // !!! TO DO !!!
                        // if (from >= charCount)
                        // {
                            // if (preserveLines)
                            // {
                                // // cut beginning from first char after next linebreak
                                // newFrom = charCount + 1;
                            // }
                            // else
                            // {
                                // // cut out the whole line if original cut begins there
                                
                            // }
                            // break;
                        // }
                        // i++;
                    // }
                // }
                // else
                // {
                    // self.domObject[self.txtAccess] = self.domObject[self.txtAccess].substring(
                        // from, to);
                    // self.updateLineBreaks(from, to);
                // }
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
                return self.domObject[self.txtAccess];
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
                    self.domObject[self.txtAccess], true);
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
                
                // console.log(msg);
                // console.log("1) " + newLineBreaks);
                
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
                    
                    // if ((newLineBreaks[newLineBreaks.length - 1] + self.newLine.length)
                        // == msg.length)
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
                        // console.log(msg.length + " " + lineTail + " " + msg);
                        // console.log(typeof(newLineBreaks) + " " + newLineBreaks.length 
                            // + " " + newLineBreaks);
                        Array.prototype.push.apply(newLineBreaks, [lineTail]);
                    }
                    
                    // console.log(newLineBreaks);
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
                
                // console.log("2) " + newLineBreaks);
                // console.log(self._relLineBreaks);
                // console.log(endOfPrevLine + " - " + msg);
                self.domObject[self.txtAccess] += endOfPrevLine;
                self.domObject[self.txtAccess] += msg;
                self.cutAtMaxLength(true, true);
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
    
    Object.defineProperty(module.TextElement.prototype, "domObject", 
    {
        get: function () { return this._domObject; },
        set: function (val) { this._domObject = val; },
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
    
    Object.defineProperty(module.TextElement.prototype, "txtAccess", 
    {
        get: function () { return this._txtAccess; },
        set: function (val) { this._txtAccess = val; },
        enumerable: true,
        configurable: false
    });

    return module;
    
}(IO));