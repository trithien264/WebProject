//angular.module('ngMasterAg', ['toastr', 'ngAnimate'])

appMaster.factory('randomQuotes', function () {
      var quotes = [
      {
          title: 'Come to Freenode',
          message: 'We rock at <em>#angularjs</em>',
          options: {
              allowHtml: true
          }
      }      
    ];

      var types = ['success', 'error', 'info', 'warning'];

      return {
          quotes: quotes,
          types: types
      };
  })

  appMaster.controller('DemoCtrl', function ($scope, $templateCache, $templateRequest, randomQuotes, toastr, toastrConfig) {
  
      var openedToasts = [];

      $scope.toast = {
          title: '',
          message: ''
      };

      $scope.options = {
          autoDismiss: false,
          position: 'toast-top-right',
          type: 'success',
          timeout: '5000',
          extendedTimeout: '1000',
          html: false,
          closeButton: false,
          tapToDismiss: true,
          progressBar: false,
          closeHtml: '<button>&times;</button>',
          newestOnTop: true,
          maxOpened: 0,
          preventDuplicates: false,
          preventOpenDuplicates: false
      };

      $scope.refreshToast = function () {
          toastr.refreshTimer(openedToasts[openedToasts.length - 1]);
      }

      $scope.$watchCollection('options', function (newValue) {
          toastrConfig.autoDismiss = newValue.autoDismiss;
          toastrConfig.allowHtml = newValue.html;
          toastrConfig.extendedTimeOut = parseInt(newValue.extendedTimeout, 10);
          toastrConfig.positionClass = newValue.position;
          toastrConfig.timeOut = parseInt(newValue.timeout, 10);
          toastrConfig.closeButton = newValue.closeButton;
          toastrConfig.tapToDismiss = newValue.tapToDismiss;
          toastrConfig.progressBar = newValue.progressBar;
          toastrConfig.closeHtml = newValue.closeHtml;
          toastrConfig.newestOnTop = newValue.newestOnTop;
          toastrConfig.maxOpened = newValue.maxOpened;
          toastrConfig.preventDuplicates = newValue.preventDuplicates;
          toastrConfig.preventOpenDuplicates = newValue.preventOpenDuplicates;
          if (newValue.customTemplate) {
              toastrConfig.templates.toast = 'custom';
          } else {
              toastrConfig.templates.toast = 'directives/toast/toast.html';
          }
      });

      $scope.$watch('toast.customTemplate', function (newVal) {
          if ($templateCache.get('custom')) {
              $templateCache.remove('custom');
          }
          $templateCache.put('custom', newVal);
      });

      $scope.clearLastToast = function () {
          var toast = openedToasts.pop();
          toastr.clear(toast);
      };

      $scope.clearToasts = function () {
          toastr.clear();
      };

      $scope.openPinkToast = function () {
          openedToasts.push(toastr.info('I am totally custom!', 'Happy toast', {
              iconClass: 'toast-pink'
          }));
      };

      $scope.openRandomToast = function () {
          var type = Math.floor(Math.random() * 4);
          var quote = Math.floor(Math.random() * 7);
          var toastType = randomQuotes.types[type];
          var toastQuote = randomQuotes.quotes[quote];
          openedToasts.push(toastr[toastType](toastQuote.message, toastQuote.title, toastQuote.options));
      };

      $scope.openToast = function () {
          //openedToasts.push(toastr[$scope.options.type]($scope.toast.message, $scope.toast.title));
          debugger
          toastr[$scope.options.type]($scope.toast.message, $scope.toast.title)
      };

      $templateRequest('default.html').then(function (tpl) {
          $scope.toast.customTemplate = tpl;
      });
  });
