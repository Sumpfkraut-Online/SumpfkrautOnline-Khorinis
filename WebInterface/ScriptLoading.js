var ScriptLoading = (function (module)
{
    if (typeof(module) == "undefined")
    {
        module = {};
    }
    
    var jsSrcList = new Array 
    (
        "Utilities/INIT.js",
        "Utilities/StringUtil.js",
        "Utilities/CallbackHandler.js",
        "IO/INIT.js",
        "IO/TextElement.js",
        "IO/Chat.js",
        "WebSockets/INIT.js",
        "WebSockets/WSConnection.js",
        "Main.js"
    );

    var currJSIndex = 0;
    var onLoadedJS = function ()
    {
        if (typeof(this.onload) != "undefined")
        {
            currJSIndex++;
            loadAllJS();
            return;
        }
        if (this.readyState == "loaded")
        {
            currJSIndex++;
            loadAllJS();
            return;
        }
    };

    var loadAllJS = function ()
    {
        if ((jsSrcList.length > 0) && (currJSIndex < jsSrcList.length))
        {
            loadJS(jsSrcList[currJSIndex], onLoadedJS);
        }
    }

    var loadJS = function (src, callback)
    {
        var script = document.createElement("script");
        script.type = "text/javascript";
        if (typeof(script.onload) != "undefined")
        {
            script.onload = callback;
        }
        else
        {
            script.onreadystatechange = callback;
        }
        script.src = src;
        //document.getElementsByTagName("head")[0].appendChild(script);  
        document.head.appendChild(script);
    };
    
    loadAllJS();
    
    return module;
    
}(ScriptLoading));