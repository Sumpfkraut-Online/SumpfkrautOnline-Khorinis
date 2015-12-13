// Use a Node.js core library
var url = require('url');

// What our module will return when require'd
module.exports = function(url) {
    var parsed = url.parse(url);
    return parsed.hash;
};

alert("hasher");