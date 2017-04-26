NgJs=(function () {
    _rootLib = "http://localhost/WebBaseMVC/ViewLibrary";

    /**Tool Lib**/
    String.prototype.replaceAll = function (search, replacement) {
        var target = this;
        return target.split(search).join(replacement);
    };

    jQuery.loadScript = function (path, callback) {
        var s_url = (_rootLib + "." + path).replaceAll(".", "/") + ".js";
        jQuery.ajax({
            url: s_url,
            dataType: 'script',
            success: callback,
            async: false
        });
    };
  

    _setObjectByPath = function (obj, path, value) {
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

    _getObjectByPath = function (obj, value) {
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

    /***** Class ******/
    function _NgJs() {
        function using(path) {
           
            $.loadScript(path, function (data) {                
                var func = eval(data);                
                _setObjectByPath(NgJs, path, func);
                return func;
            });
            
            /*obj = getByPath(NgJs, path);
            if (obj)
                return obj;*/
            return this;
        }

        function define(nmOrFn, fn) {
            var realFn = nmOrFn;
            if (typeof (nmOrFn) == "string") {
                this._sjsName = nmOrFn;
                realFn = fn;
            }
            this._fn = realFn;//fn;
            _defineSjs(this);
        }

        //define contructor
        this.define = define;
        this.using = using;
    }



    function define(nmOrFn, fn) {
        new _sjs().define(nmOrFn, fn);
    }

    function using(js) {        
        return new _NgJs().using(js);
    }

    return {
        using: using
        , define: define
    };
}).call(this);
