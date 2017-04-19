var NgJs = NgJs || {}; //x = a || b; (a, b đều là object) thì x = a nếu a khác null trái lại x = b.
NgJs.Service.Url = (function () {
    var _valueSrv = '/WebBaseMVC/API/Run';
    this.getUrl = function () {
        var srvURL = _valueSrv;
        return srvURL;
    };   
    return {
        getUrl: this.getUrl     
    };

}).call(this);
