var NgJs = NgJs || {}; //x = a || b; (a, b đều là object) thì x = a nếu a khác null trái lại x = b.

NgJs.Control = (function () {
    //Main function
    NgJs_grid_init = function (options) {
        var defaults = {
            url: "",
            params: [{}],
            ngFactortyId: "ngFactortyId",
            paginationPageSize: 5,
            paginationPageSizes: [5, 10, 100],
            columnDefs: [],
            gridId: "ngDefaultGridID",
            gridObject: {},//Custom grid defined from User
            enableRowSelection: false,
            enableSelectAll: false,
            multiSelect: false,
            customOptions: {}
        };
        var options = $.extend(defaults, options);
        options = options = NgJs.Tool.addCtrlArgOptions(options); 


        //Button Search Click
        options.$scope.ngBtnSearchData = function () {
            options.params = [options.$scope.modelsData];
            NgJs_grid_callData(options);
        }
        //call to service
        NgJs_grid_callData(options);
    };

    //call to service
    NgJs_grid_callData = function (options) {

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

        //*** Begin Config Grid ***
        //Set option
        
        if (!angular.equals({}, options.customOptions))//custom Attribute for Grid
        {
            options.$scope[options.gridId] = options.customOptions;
        }
        else//Include option co san
        {
            options.$scope[options.gridId] = {
                paginationPageSizes: options.paginationPageSizes,
                paginationPageSize: options.paginationPageSize,
                enableRowSelection: options.enableRowSelection,
                enableSelectAll: options.enableSelectAll,
                multiSelect: options.multiSelect,
                columnDefs: options.columnDefs
            }
        }
        //*** End Config Grid ***
        
        

        options.$scope.fireFlag = true;

        var JsService = {};
        JsService.service = options.url;
        JsService.params = options.params;
        
        return options.$http({
            method: 'POST',
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
            url: NgJs.Service.Url.getUrl(),
            data: $.param({ JsService: JSON.stringify(JsService) })
        }).success(function (data, status, headers, config) {
            //get grid Object
            var _objGird;
            try {
                if (!angular.equals({}, options.gridObject))//Custom grid defined from User
                {
                    _objGird = options.gridObject;
                }
                else { //get object by gridID
                    _objGird = options.$scope[options.gridId]
                }
            } catch (e) {
            }

            if (data.AjaxError == 0) {
                if (haveLoading)
                    options.cfpLoadingBar.complete();
                options.$scope.fireFlag = false;                       

                if (data && data.AjaxError == 0) {
                    if (data.Result == null) {
                        _objGird.data = [];
                        _objGird.data.length = 0;
                    }
                    else {                        
                        _objGird.data = data.Result;                       
                    }
                }
            }
            else {
                _objGird.data = [];
                _objGird.data.length = 0;

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
        grid: this.NgJs_grid_init
    };

}).call(this);



