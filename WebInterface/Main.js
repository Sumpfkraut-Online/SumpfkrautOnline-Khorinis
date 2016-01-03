var Main = (function (module)
{
    if (typeof(module) == "undefined")
    {
        module = {};
    }
    
    // create basic io-elements including submit for later text-forwarding to server
    module.chatInput = new IO.TextElement(
        new Array(document.getElementById("chatTextArea"), "value"), 
        "\n", 20000);
    module.chatOutput = new IO.TextElement(
        new Array(document.getElementById("chatOutput"), "innerHTML"), 
        "<br>", 20000);
    module.chatSubmit = document.getElementById("chatSubmit");
    module.chatSubmitHandler = new Utilities.CallbackHandler();
    module.chatSubmitHandler.addSender(module.chatSubmit, "onclick");
    
    // create websocket-connection
    module.generalWsUri = "ws://localhost:81/";
    module.wsConnection = new WebSockets.WSConnection(module.generalWsUri, 
        module.chatOutput);
    module.wsConnection.init();
    
    // creating chat
    module.chat = new IO.Chat();
    module.chat.wsConnection = module.wsConnection;
    module.chat.output = module.chatOutput;
    module.chat.input = module.chatInput;
    module.chat.submitHandler = module.chatSubmitHandler;
    
    return module;
    
}(Main));