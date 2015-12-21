var WebSockets = (function (module)
{

    if (typeof(module) === "undefined")
    {
        module = {};
    }
    
    module.protocalTypes = 
    {
        Unknown:"Unknown",
        UserData:"UserData",
        ChatData:"ChatData",
        VobData:"VobData"
    };
    
    return module;
    
}(WebSockets));