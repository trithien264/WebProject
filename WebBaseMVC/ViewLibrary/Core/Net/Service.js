﻿NgJs.using("Core.Util.Tool")
.using("Core.Net.Url");

(function () {
    //Main function
    NgJs_service_init = function (url, params, options) {
        if (!url) {
            alert("Url is requied!!!");
            return;
        }
        
        var defaults = {                 
        };
        var options = $.extend(defaults, options);

        options.url = url;       
        options.params = params;
        options = NgJs.Core.Util.Tool.addCtrlArgOptions(options, options.myCtrlArg);
        //call to service
        return NgJs_callService(options);
    };
     

    //call to service
    NgJs_callService = function (options) {

        //*** Begin Config Loading and Toas ***
        //loading tool
        var haveLoading = false;
        try {
            options.cfpLoadingBar.start();
            haveLoading = true;
        } catch (e) { }

        //toas tool
        var haveToastr = false;
        try {
            var a = options.toastrConfig.closeButton
            options.toastrConfig.allowHtml = true;
            haveToastr = true;
        } catch (e) { }
        //*** End Config Loading and Toas ***

        options.$scope.fireFlag = true;

        var JsService = {};
        JsService.service = options.url;
        JsService.params = options.params;

        return options.$http({
            method: 'POST',
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
            url: NgJs.Core.Net.Url.getUrl(),
            data: $.param({ JsService: JSON.stringify(JsService) })
        }).success(function (data, status, headers, config) {            
            //get grid Object
            if (data.AjaxError == 0) {
                if (haveLoading)
                    options.cfpLoadingBar.complete();
                options.$scope.fireFlag = false;
                return data.Result;
            }
            else {
                if (haveLoading)
                    options.cfpLoadingBar.complete();
                if (haveToastr) {    
                    //Show toast
                    options.toastrConfig.closeButton = true;
                    var objMessage = data.Message;
                    options.toastr["error"](objMessage, "Notification!!!");
                }

                options.$scope.fireFlag = false;
                return null;
            }
        }).error(function (data, status, headers, config) {
            if (haveLoading)
                options.cfpLoadingBar.complete();
            if (haveToastr) {
                options.toastrConfig.closeButton = true;
                options.toastr["error"](data, "Notification!!!");
            }
            options.$scope.fireFlag = false;
            return null;
        });
       
    };


    return {
        call: this.NgJs_service_init
    };

})();



