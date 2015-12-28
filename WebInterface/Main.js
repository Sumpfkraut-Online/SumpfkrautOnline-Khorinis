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