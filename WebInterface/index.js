var Main = (function (module)
{
    if (typeof(module) == "undefined")
    {
        module = {};
    }

    var chatInput = new IO.TextElement(document.getElementById("chatTextArea"), 
        "value", "\n");
    var chatOutput = new IO.TextElement(document.getElementById("chatOutput"), 
        "innerHTML", "<br>");
    var chatSubmit = document.getElementById("chatSubmit");
    
    module.wsConnection = new WebSockets.WSConnection("ws://localhost:81/", 
        chatInput, chatOutput, chatSubmit);
    // document.getElementById("chatSubmit").onclick = module.wsConnection.sendInputMessage;
    module.wsConnection.init();
    
    return module;
    
}(Main));