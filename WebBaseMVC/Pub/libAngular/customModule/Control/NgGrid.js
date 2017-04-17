﻿var NgJs = NgJs || {}; //x = a || b; (a, b đều là object) thì x = a nếu a khác null trái lại x = b.

var NgJs = {
    addModuleOptions: function (options) {        
        var requires = options.MyApp.requires;
        for (var i = 0; i < requires.length; ++i) 
        {
            var key = requires[i];
            var value = angular.module(key, []);
            options[key] = value;
        }
        return options;
    }
    , addCtrlArgOptions: function (options) {
        var injects = options.myCtrlArg.callee.$inject;
        for (var i = 0; i < injects.length; ++i)
        {
            var key = injects[i];
            var value = options.myCtrlArg.callee.arguments[i];
            options[key] = value;
        }
        return options;
    }
    , grid: function (options) {

        var defaults = {
            url: "",
            params: [{}],
            columnDefs: [],
            gridId: "ngDefaultGridID",
            gridObject: {},//Truong hop grid do user tu dinh nghia
        };

                
        var options = $.extend(defaults, options);
        
        options = NgJs.addCtrlArgOptions(options);
        
        //options = NgJs.addModuleOptions(options); // add module defined from options

        //loading tool
        var haveLoading = false;
        try {
            debugger
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


        options.scope.fireFlag = true;

        var JsService = {};
        JsService.service = options.url;
        JsService.params = options.params;

        //return options.http({ method: 'GET', url: NgJs.Url.get(options.url, options.params) })
        return options.http({ method: 'POST', headers: { 'Content-Type': 'application/x-www-form-urlencoded' }, url: NgJs.Url.get(), data: $.param({ JsService: JSON.stringify(JsService) }) })
              .success(function (data, status, headers, config) {

                  if (data.AjaxError == 0) {
                      if (haveLoading)
                          options.cfpLoadingBar.complete();
                      options.scope.fireFlag = false;

                      if (data && data.AjaxError == 0) {
                          if (data.Result == null) {
                              options.scope[options.gridId].data = [];
                              options.scope[options.gridId].data.length = 0;
                          }
                          else {
                              _objGird.data = data.Result;
                          }
                      }
                  }
                  else {
                      options.scope[options.gridId].data = [];
                      options.scope[options.gridId].data.length = 0;

                      if (haveLoading)
                          options.cfpLoadingBar.complete();
                      if (haveToastr) {
                          //call Fun language       

                          //Show toast
                          options.toastrConfig.closeButton = true;
                          var objMessage = data.Message;

                          //change language
                          if (data.Message.toString().indexOf("n-lang") != -1) {
                              var lang = NgJs.Language.getLang();
                              options.http.get(NgJs.Url.getHost() + "/Pub/Common/Languages/" + lang + ".js", { cache: true }).success(function (obj_lang) {
                                  var iElement = angular.element(objMessage);
                                  var iElementHeader = angular.element("<n-lang code='MsgHeader'>Error Notification!!!<n-lang>");
                                  var msgLang = NgJs.Language._ChangeElemLang(options.scope, iElement, iElement[0].attributes, null, obj_lang);
                                  var msgHeader = NgJs.Language._ChangeElemLang(options.scope, iElementHeader, iElementHeader[0].attributes, null, obj_lang);
                                  options.toastr["error"](msgLang, msgHeader);
                              }).error(function (response) {
                                  options.toastr["error"](objMessage, "Notification!!!");
                              });
                          }
                          else {
                              options.toastr["error"](objMessage, "Notification!!!");
                          }

                      }

                      options.scope.fireFlag = false;
                      return null;
                  }
              })
              .error(function (data, status, headers, config) {
                  if (haveLoading)
                      options.cfpLoadingBar.complete();
                  if (haveToastr) {
                      options.toastrConfig.closeButton = true;
                      options.toastr["error"](data, "Notification!!!");
                  }
                  options.scope.fireFlag = false;
                  return null;
              });


    }
   
}

