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
    // bla["bli.blubb"] = function () { console.log("BLABLIBLUBB"); };
    // bla.bli.blubb();
    
    // alert(typeof(1.1));
    // alert(typeof(1));
    
    
    // // potential listener-functions
    // var callMeBaby = function (evt) { console.log("Ya called me, Baby!"); };
    // var callMeWilhelm = function (evt) { console.log("Ya called me, Wilhelm!"); };
    // var callMeMaybe = function (evt)
    // {
        // console.log("Ya called me, maybe...");
    // };

    // // first listener assigned
    // document.getElementById("chatInput").onclick = callMeBaby;

    // // create 2 CallbackHandler-objects
    // // they use the same event window.onclick
    // // myButton.onclick is registered on anotherCH exlusively
    // // the previous listener window.onclick = callMeBaby will be included in someCH
    // // someCH.callback (the new listener) will replaced by anotherCH.callback as well
    // var someCH = new Utilities.CallbackHandler();
    // someCH.addSender(document.getElementById("chatInput"), "onclick");
    // var anotherCH = new Utilities.CallbackHandler();
    // anotherCH.addSender(document.getElementById("chatInput"), "onclick");
    // anotherCH.addSender(document.getElementById("chatOutput"), "onclick");

    // // register callback-function on the CallbackHandler-objects
    // someCH.addReceiver(callMeWilhelm);
    // anotherCH.addReceiver(callMeMaybe);
    
    
    // console.log(Utilities.StringUtil.indicesOf("<br>", "Bla<br>Mhmhmhmh<br>Iiiiihhh", true));
    // console.log("0123456".substring(2, 4));
    
    // document.onevent = function (evt)
    // {
        // console.log("GOTCHA");
    // }
    
    // var event;
    // if (document.createEvent)
    // {console.log("!");
        // event = document.createEvent("HTMLEvents");
        // event.initEvent("event", true, true);
    // } 
    // else 
    // {
        // event = document.createEventObject();
        // event.eventType = "event";
    // }

    // event.eventName = "event";

    // if (document.createEvent) 
    // {
        // document.dispatchEvent(event);
    // } 
    // else 
    // {
        // document.fireEvent("on" + event.eventType, event);
    // }
    
    
    
    module.generalWsUri = "ws://localhost:81/";
    
    module.chatInput = new IO.TextElement(
        new Array(document.getElementById("chatTextArea"), "value"), 
        "\n", 20000);
    module.chatOutput = new IO.TextElement(
        new Array(document.getElementById("chatOutput"), "innerHTML"), 
        "<br>", 20000);
    module.chatSubmit = document.getElementById("chatSubmit");
    module.chatSubmitHandler = new Utilities.CallbackHandler();
    module.chatSubmitHandler.addSender(module.chatSubmit, "onclick");
    
    // try websocket handshake with given websocket uri (wsUri)
    module.wsConnection = new WebSockets.WSConnection(module.generalWsUri, 
        module.chatOutput);
    module.wsConnection.init();
    
    module.chat = new IO.Chat();
    module.chat.wsConnection = module.wsConnection;
    module.chat.output = module.chatOutput;
    module.chat.input = module.chatInput;
    module.chat.submitHandler = module.chatSubmitHandler;
    
    return module;
    
}(Main));