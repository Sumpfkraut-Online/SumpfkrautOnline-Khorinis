var Utilities = (function (module)
{

    if (typeof(module) === "undefined")
    {
        module = {};
    }

    module.arrayBufferToString = function (buf)
    {
      return String.fromCharCode.apply(null, new Uint16Array(buf));
    }

    module.stringToArrayBuffer = function (str)
    {
      var buf = new ArrayBuffer(str.length*2); // 2 bytes for each char
      var bufView = new Uint16Array(buf);
      for (var i=0, strLen=str.length; i<strLen; i++)
      {
        bufView[i] = str.charCodeAt(i);
      }
      return buf;
    }

    return module;

}(Utilities));