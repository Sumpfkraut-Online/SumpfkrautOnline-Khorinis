var Utilities = (function (module)
{

    if (typeof(module) === "undefined")
    {
        module = {};
    }

    module.CallbackHandler = function ()
    {
        var self = this;
        
        self._receivers = new Array();
        self._senderObjNamePairs = new Array();
        self._replacedCallbacks = new Array();
        
        self.addReceiver = function (r)
        {
            var index = -1;
            if (typeof(r) == "function")
            {
                self._receivers.push(r);
                index = self._receivers.length;
            }
            return index;
        }
        
        self.removeReceiver = function (r)
        {
            var index = self._receivers.indexOf(r);
            if (index > -1)
            {
                self._receivers.splice(index, 1);
            }
            return index;
        }
        
        self.addSender = function (sObj, sEvtName)
        {
            var index = -1;
            if ((typeof(sObj) == "object") && (sObj.hasOwnProperty(sEvtName)))
            {
                self._senderObjNamePairs.push(new Array(sObj, sEvtName));
                index = self._senderObjNamePairs.length;
                
                if (!(sObj[sEvtName] === null))
                {
                    // add replaced callback as new special receiver
                    self._replacedCallbacks.push({obj:sObj, evtName:sEvtName, 
                        callback:sObj[sEvtName]});
                }
                
                sObj[sEvtName] = self.callback;
            }
            return index;
        }
        
        self.indexOfReplacedCallback = function (sObj, sEvtName)
        {
            var index = -1;
            var i = 0;
            while (i < self._replacedCallbacks.length)
            {
                if ((self._replacedCallbacks[i].obj === sObj)
                    && (self._replacedCallbacks[i].evtName === sEvtName))
                {
                    index = i;
                    break;
                }
                i++;
            }
            return index;
        }
        
        self.removeSender = function (sObj, sEvtName)
        {
            var index = -1;
            var _senders = self_senderObjNamePairs;
            var i = 0;
            while (i < _senders.length)
            {
                if ((_senders[i][0] === sObj) 
                    && (_senders[i][1] == sEvtName))
                {
                    index = i;
                    break;
                }
                i++;
            }
            if (index > -1)
            {
                if ((typeof(sObj) == "object") 
                    && (sObj.hasOwnProperty(sEvtName))
                    && (sObj[sEvtName] === self.callback))
                {
                    // either nullify or replace with previous callback
                    var replndex = self.indexOfReplacedCallback(sObj, sEvtName);
                    if (replndex > -1)
                    {
                        sObj[sEvtName] = self._replacedCallbacks[replndex].callback;
                        self._replacedCallbacks.splice(replndex, 1);
                    }
                    else
                    {
                        sObj[sEvtName] = null;
                    }
                }
                _senders.splice(index, 1);
            }
            return index;
        }
        
        self.callback = function ()
        {
            // replaced callbacks have priority because they were first
            for (c in self._replacedCallbacks)
            {
                self._replacedCallbacks[c].callback.apply(null, arguments);
            }
            // invoke own callbacks
            for (r in self._receivers)
            {
                self._receivers[r].apply(null, arguments);
            }
        }
    }
    
    Object.defineProperty(module.CallbackHandler.prototype, "_className", 
    {
        value: "CallbackHandler",
        enumerable: true,
        configurable: false,
        writable: false
    });

    return module;
    
}(Utilities));