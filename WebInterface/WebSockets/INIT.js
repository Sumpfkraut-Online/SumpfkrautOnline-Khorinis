var WebSockets = (function (module)
{

    if (typeof(module) === "undefined")
    {
        module = {};
    }
    
    module.protocalTypes = 
    {
        undefined: "undefined",
        userData: "userData",
        chatData: "chatData",
        vobData: "vobData"
    };
    
    module.dataTypes = 
    {
        undefined: "undefined",
        json: "json",
        blob: "blob",
        bin: "bin"
    };
    
    return module;
    
}(WebSockets));