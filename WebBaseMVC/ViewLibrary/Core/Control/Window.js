NgJs.using("Core.Util.Tool");

(function () {
    //Main function
    openWin = function (templateUrl, options) {

        var defaults = {
            myGrid: {}
            , myGridRow: {}
        };
        
        var options = $.extend(defaults, options);
        options = NgJs.Core.Util.Tool.addCtrlArgOptions(options, options.myCtrlArg);
        
        var modalInstance = options.$modal.open({
            templateUrl: templateUrl,
            controller: ['$http', '$modalInstance', 'grid', 'row', options.callModalCtrl],
            scope: options.$scope,
            backdrop: "static",
            resolve: {
                grid: function () {
                    return options.myGrid;
                },
                row: function () {
                    return options.myGridRow;
                }
            }
        });
        return modalInstance;
    };

   
    
    return {
        open: this.openWin
    };

})();


