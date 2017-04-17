(function() {
  var app, deps;

  deps = ['angularBootstrapNavTree', 'toastr', 'chieffancypants.loadingBar', 'ngTouch', 'ui.grid', 'ui.grid.pagination', 'ngMaterial'];

  if (angular.version.full.indexOf("1.2") >= 0) {
    deps.push('ngAnimate');
  }
    

  app = angular.module('ngMasterAg', deps);

  app.controller('MainPageCtrl', function ($scope, $timeout, $http, toastr, toastrConfig, cfpLoadingBar) {
      $scope.my_tree = tree = {};

      var optionsGrid = {
          gridId: "ngGridMenu",
          url: "WebBase.Base.Rights.RightsMenu.GetUserRights",
          params: [{}],
          http: $http,
          toastr: toastr,
          toastrConfig: toastrConfig,
          cfpLoadingBar: cfpLoadingBar,
          scope: $scope,
          columnDefs: [
                { name: 'user_id' },
                { name: 'user_nm' },
                { name: 'user_desc' },
                { name: 'email' }
          ]
      }

      NgJs.grid(optionsGrid);

      var callService = NgJs.callService({
          url: "WebBase.Base.Rights.RightsMenu.treeJSONMenu",
          params: [{}],
          http: $http,
          toastr: toastr,
          toastrConfig: toastrConfig,
          cfpLoadingBar: cfpLoadingBar,
          scope: $scope
      });

      callService.then(function (data) {          
          LoadTree(data);
      });

   
      var LoadTree = function (data) {
          var tree;
          $scope.my_tree_handler = function (branch) {              
              optionsGrid.params = [{ menu_id: branch.menu_id }];              
              NgJs.grid(optionsGrid);
          };
         


          //debugger
          $scope.my_data = [JSON.parse(data.data.Result)];
          $scope.my_tree.expand_all();
          $scope.my_tree.select_first_branch()
          //$scope.my_data = treedata_avm; 
        
      }     
      $scope.my_data = [];
    
      
  });

}).call(this);