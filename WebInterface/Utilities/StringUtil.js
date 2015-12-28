var Utilities.StringUtil = (function (module)
{

    if (typeof(module) === "undefined")
    {
        module = {};
    }

    module.indicesOf = function (searchStr, str, caseSensitive)
    {
        var startIndex = 0, searchStrLen = searchStr.length;
        var index, indices = [];
        if (!caseSensitive)
        {
            str = str.toLowerCase();
            searchStr = searchStr.toLowerCase();
        }
        while ((index = str.indexOf(searchStr, startIndex)) > -1)
        {
            indices.push(index);
            startIndex = index + searchStrLen;
        }
        return indices;
    };

    return module;

}(Utilities.StringUtil));
