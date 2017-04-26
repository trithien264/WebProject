NgJs=(function () {
    var _jsReMap = {};
    var _cssReMap = {};
    function _getMapPath(path, ext) {
        if (!(path.indexOf("/") >= 0)) {            
            if (path.indexOf(".") == 0) {
                if (!_current_loading_using || !_current_loading_using.js) {
                    exExist(path + ' loading error,no current load using');
                }
                var currentJs = _current_loading_using.js.split(".");
                var folderLevel;
                for (folderLevel = 1; folderLevel < path.length; folderLevel++) {
                    if (path[folderLevel] != ".") {
                        break;
                    }
                }
                currentJs.length = currentJs.length - folderLevel;
                path = currentJs.join(".") + "." + path.substr(folderLevel);
            }

            var reMap = ext == "css" ? _cssReMap : ext == "js";
            if (reMap && reMap[path]) {
                return reMap[path];
            }           
        }
        return path;
    }

    function _NgJs() {
        this._usingList = [];

        function using(js, noArg) {            
            var serviceConfig;            
            this._usingList.push({ js: _getMapPath(js, "js"),noArg: noArg});
            return this;
        }

        this.using = using;
    }

    function using(js, noArg) {
        debugger
        return new _NgJs().using(js, noArg);
    }

    return {
        using: using
    };
}).call(this);
