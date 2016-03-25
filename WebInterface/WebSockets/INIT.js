var WebSockets = (function (module)
{

    if (typeof(module) === "undefined")
    {
        module = {};
    }
    
    module.protocalTypes = 
    {
        undefined: 0,
        chatData: 1
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