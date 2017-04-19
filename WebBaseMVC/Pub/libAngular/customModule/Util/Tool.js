﻿var NgJs = NgJs || {}; //x = a || b; (a, b đều là object) thì x = a nếu a khác null trái lại x = b.

NgJs.Tool = (function () {

    addCtrlArgOptions = function (options) {
        var injects = options.myCtrlArg.callee.$inject;
        for (var i = 0; i < injects.length; ++i) {
            var key = injects[i];            
            //var value = options.myCtrlArg.callee.arguments[i];
            var value = options.myCtrlArg[i]; //TOCHECK
            options[key] = value;
        }
        return options;
    };

    return {
        addCtrlArgOptions: this.addCtrlArgOptions
    };

}).call(this);



