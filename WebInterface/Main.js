var Main = (function (module)
{
    if (typeof(module) == "undefined")
    {
        module = {};
    }
    
    // var arr = new Array(1, true, 2, false, 3, undefined);
    // arr.splice(1, 1);
    // console.log(arr);
    
    // var bla = function (a, b, c, d, e, f, g, h)
    // {
        // console.log("arguments.length: " + arguments.length);
        // console.log(a + " " + b + " " + c + " " + d + " " + e 
            // + " " + f + " " + g + " " + h);
        // for (arg in arguments)
        // {
            // console.log(arg);
        // }
    // };
    // var blubb = function ()
    // {
        // bla.apply(null, arguments);
    // }
    // blubb(1, 2, 3, 4, 5, 6, true, false);
    
    // var bla = {};
    // alert(typeof(bla) == "object");
    
    // var windowOnClickCH = new Utilities.CallbackHandler();
    // windowOnClickCH.addSender(window, "onclick");
    // var callMeBaby = function () { console.log("Ya called me, Baby!"); };
    // var callMeWilhelm = function () { console.log("Ya called me, Wilhelm!"); };
    // windowOnClickCH.addReceiver(callMeBaby);
    // windowOnClickCH.addReceiver(callMeWilhelm);
    
    // var anotherCH = new Utilities.CallbackHandler();
    // anotherCH.addSender(window, "onclick");
    // var callMeMaybe = function () { console.log("Ya called me, maybe..."); };
    // anotherCH.addReceiver(callMeMaybe);
    
    
    // potential listener-functions
    var callMeBaby = function (evt) { console.log("Ya called me, Baby!"); };
    var callMeWilhelm = function (evt) { console.log("Ya called me, Wilhelm!"); };
    var callMeMaybe = function (evt)
    {
        console.log("Ya called me, maybe...");
        // console.log(evt.target);
        // console.log(evt.currentTarget);
        // console.log(evt.currentTarget === window.onclick);
        // console.log(evt.type);
        // console.log(arguments.callee);
    };

    // first listener assigned
    window.onclick = callMeBaby;

    // create 2 CallbackHandler-objects
    // they use the same event window.onclick
    // myButton.onclick is registered on anotherCH exlusively
    // the previous listener window.onclick = callMeBaby will be included in windowOnClickCH
    // windowOnClickCH.callback (the new listener) will replaced by anotherCH.callback as well
    var windowOnClickCH = new Utilities.CallbackHandler();
    windowOnClickCH.addSender(window, "onclick");
    var anotherCH = new Utilities.CallbackHandler();
    anotherCH.addSender(window, "onclick");
    anotherCH.addSender(document.getElementById("chatOutput"), "onclick");

    // register callback-function on the CallbackHandler-objects
    windowOnClickCH.addReceiver(callMeWilhelm);
    anotherCH.addReceiver(callMeMaybe);
    
    
    
    var chatInput = new IO.TextElement(document.getElementById("chatTextArea"), 
        "value", "\n");
    var chatOutput = new IO.TextElement(document.getElementById("chatOutput"), 
        "innerHTML", "<br>");
    var chatSubmit = document.getElementById("chatSubmit");
    
    // try websocket handshake with given websocket uri (wsUri)
    module.wsConnection = new WebSockets.WSConnection("ws://localhost:81/", 
        chatInput, chatOutput, chatSubmit);
    module.wsConnection.init();
    
    return module;
    
}(Main));