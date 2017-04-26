(function () {
   
    return {

        addCtrlArgOptions: function (options,myArg) {
            var injects = myArg.callee.$inject;
            for (var i = 0; i < injects.length; ++i) {
                var key = injects[i];
                //var value = options.myCtrlArg.callee.arguments[i];
                var value = myArg[i]; //TOCHECK
                options[key] = value;
            }
            return options;
        }
        , setObjectByPath: function (obj, path, value) {
            var parts = path.split('.');
            var o = obj;
            if (parts.length > 1) {
                for (var i = 0; i < parts.length - 1; i++) {
                    if (!o[parts[i]])
                        o[parts[i]] = {};
                    o = o[parts[i]];
                }
            }

            o[parts[parts.length - 1]] = value;
        }
        , getObjectByPath: function (obj, value) {
            value = value.replace(/\[(\w+)\]/g, '.$1'); // convert indexes to properties
            value = value.replace(/^\./, '');           // strip a leading dot
            var a = value.split('.');
            for (var i = 0, n = a.length; i < n; ++i) {
                var k = a[i];
                if (k in obj) {
                    obj = obj[k];
                } else {
                    return;
                }
            }
            return obj;
        }

    }

})();



