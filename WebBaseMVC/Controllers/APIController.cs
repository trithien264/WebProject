using BusService;
using BusService.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using WebBaseMVC.Areas.Admin.Models.Framework;

namespace WebBaseMVC.Controllers
{
    public class APIController : Controller
    {
        WebBaseDbContext dbcontext = null;
        public JsonResult Run(string JsService)
        {            
            return Json(new HttpService().Run(JsService));
        }

        

	}

    public class HttpService
    {
        WebBaseDbContext dbcontext = null;
        public Dictionary<string,object> Run(string JsService)
        {
            dbcontext = new WebBaseDbContext();   
            List<base_user> user = dbcontext.base_user.ToList();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Result", user);
            dic.Add("AjaxError", 0);
            return dic;
        }
    }

    public class ServiceCaller
    {
        public enum CallType
        {
            PermissionCall          //Including the rights, the service of the service call
            ,
            TransactionCall         //A service call is made only for the transaction
            ,
            BaseCall          //Only the most internal call (reflection call)
        }

        public static readonly ServiceCaller Instance = new ServiceCaller();

        public Dictionary<string, object> CallToDic(ServiceCaller.CallType callType, string jsonCall)
        {
            Dictionary<string, object> ret = new Dictionary<string, object>();

            object jsonObject = null;
            try
            {
                jsonObject = Tool.ToObject(jsonCall);
            }
            catch (Exception ex)
            {
                ret["AjaxError"] = "4";
                ret["Message"] = "Tool.ToObject failure,json format invalid(" + jsonCall + " message:" + ex.Message + ")";
                return ret;
            }


            bool isMultiple = false;     //Is not more than one call
            ArrayList items = null;
            if (jsonObject is ArrayList)
            {
                isMultiple = true;
                items = jsonObject as ArrayList;
            }
            else if (jsonObject is Dictionary<string, object>)
            {
                items = new ArrayList(new object[] { jsonObject });
            }

            List<Dictionary<string, object>> multipleRet = new List<Dictionary<string, object>>();
            if (items != null && items.Count > 0)
            {
                foreach (Dictionary<string, object> dic in items)
                    multipleRet.Add(CallToDic(callType, dic));
            }
            else
            {
                ret["AjaxError"] = "4";
                ret["Message"] = "service format(json) invalid(" + jsonCall + ")";
            }


            if (isMultiple)
            {
                ret["AjaxError"] = 0;
                ret["Result"] = multipleRet;
                ret["IsMultiple"] = true;
            }
            else
            {
                ret = multipleRet[0];
                ret["IsMultiple"] = false;
            }

            return ret;
        }

        public Dictionary<string, object> CallToDic(ServiceCaller.CallType callType, Dictionary<string, object> dic)
        {
            Dictionary<string, object> ret = new Dictionary<string, object>();
            if (dic != null)
            {
                string service = null;
                object[] args = null;
                // Together because the external service is not necessarily such a form, it may be a direct service name (such as PCIUserList)
                // So leave the interface here
                if (dic.ContainsKey("service"))
                {
                    service = dic["service"].ToString();
                    if (dic.ContainsKey("params"))
                    {
                        ArrayList tmp = dic["params"] as ArrayList;
                        if (tmp != null)
                        {
                            args = new object[tmp.Count];
                            tmp.CopyTo(args);
                        }
                    }
                    //Development test, you can specify the delay to test the network, database busy
                    if (dic.ContainsKey("callDelay"))
                        System.Threading.Thread.Sleep(int.Parse(dic["callDelay"].ToString()));

                    ret = CallToDic(callType, service, args);
                }
                else
                {
                    ret["AjaxError"] = 4;
                    ret["Message"] = "service call description invalid(must provide service!)";
                }
            }
            else
            {
                ret["AjaxError"] = 4;
                ret["Message"] = "service call description is null(not a json object)";
            }
            return ret;
        }

        public Dictionary<string, object> CallToDic(ServiceCaller.CallType callType, string service, params object[] args)
        {
            Dictionary<string, object> ret = new Dictionary<string, object>();
            try
            {
                object result = Call(callType, service, args);
                ret["AjaxError"] = 0;
                ret["Result"] = result;
                ret["Params"] = args;
            }
            catch (BusNeedLoginException lex)
            {
                ret["AjaxError"] = 1;
                ret["Message"] = "Need Login(" + lex.Message + ")";

                /* TODO
                 * if (LogHelper.Instance.GetLogLevel() == LogHelper.LogLevel.High)
                    Tool.Warn("Service call (login required)", "Info", lex.Message);*/
            }
            catch (BusNoPermissionException pex)
            {
                ret["AjaxError"] = 2;
                ret["Message"] = "No Pemission:" + pex.Message;
                /* TODO
                 * if (LogHelper.Instance.GetLogLevel() == LogHelper.LogLevel.High)
                    Tool.Warn("Service Invocation (No Privileges)", "Info", pex.Message);*/
            }
            catch (BusInfoException pex)
            {
                ret["AjaxError"] = 3;
                ret["Message"] = pex.Message;

            }
            catch (ApplicationException aex)
            {
                ret["AjaxError"] = 4;
                ret["Message"] = aex.Message;
                /* TODO
                 * Tool.Error("Application Exception", "Message", aex.Message, "ApplicationException", aex.InnerException);
                 */
            }
            catch (Exception ex)
            {
                ret["AjaxError"] = 5;
                ret["Message"] = "[Unrecognized exception]: " + ex.Message;
                /*TODO
                 * Tool.Error("Service call error (uncaught)", "ex", ex);*/
            }

            ret["Service"] = service;
            return ret;
        }

        public object Call(CallType type, string service, params object[] args)
        {
            return call(type, service, args);
        }

        protected virtual object call(CallType type, string service, params object[] args)
        {
            try
            {
                int paramsIndex = service.IndexOf("$");
                if (paramsIndex > 0)
                {
                    AppEventHanlder.Instance.SetServiceVarContent(service.Substring(paramsIndex + 1));
                    service = service.Substring(0, paramsIndex);

                }
                // To PermissionCall as a new Session, so have to empty ServiceVarContent
                // multiple service batches call, the first does not affect the second
                else if (type == CallType.PermissionCall)
                {
                    AppEventHanlder.Instance.SetServiceVarContent(null);
                }


                int dotIndex = service.LastIndexOf(".");
                if (dotIndex <= 0 || dotIndex >= service.Length - 1)
                    throw new ApplicationException("Invalid service:" + service);
                string serviceId = service.Substring(0, dotIndex);
                string command = service.Substring(dotIndex + 1);
                //TODO: permissionCall should be allowed to judge the first authority, or the object has been instantiated
                object serviceObj = ObjectFactory.Instance.Get(serviceId);
                if (serviceObj == null)
                    throw new ApplicationException("Service not found:" + serviceId);


                if (type == CallType.PermissionCall)//"0")
                {
                    if (RightsAccessAttribute.Instance.HasRights(serviceObj, command))//Check Permission Attribute class and method                     
                        return transactionCall(serviceObj, serviceId, command, args);
                    else
                        return permissionCall(serviceObj, serviceId, command, args);
                }
                else if (type == CallType.TransactionCall)//"1")
                    return transactionCall(serviceObj, serviceId, command, args);
                else
                    return baseCall(serviceObj, serviceId, command, args);


            }
            finally
            {

            }
        }

        protected virtual object permissionCall(object serviceObj, string serviceId, string command, params object[] args)
        {
            /*string objKind = "Ajax";

            string objId = serviceId + "." + command;
            string userId = AuthenticateHelper.Instance.UserID;
            string userIP = AppEventHanlder.Instance.UserHost;


            if (DBRightsProvider.Instance.HasServiceRight("SupperAdmin", serviceId, command, userIP, userId) || DBRightsProvider.Instance.HasServiceRight("RightMenu", serviceId, command, userIP, userId))//UserInterFace || SupperAdmin || RightsMenu
            {
                return transactionCall(serviceObj, serviceId, command, args);
            }

            if (userId == null)
                throw new NSNeedLoginException(objKind + "." + objId);
            else
                throw new NSNoPermissionException(userId + ":" + objKind + "." + objId);*/


        }


        protected virtual object transactionCall(object serviceObj, string serviceId, string command, params object[] args)
        {
            /*DBHelper.Instance.SetDefaultTran();
            try
            {
                object ret = baseCall(serviceObj, serviceId, command, args);
                NService.DDD.DomainContext.Instace.Submit();
                //if (AppEventHanlder.Instance.UserHost != "172.19.6.86")
                DBHelper.Instance.CommitTran();
                //if (AppEventHanlder.Instance.UserHost == "172.19.6.86")
                //    DBHelper.Instance.RollbackTran();
                //DealOtherDeal();
                //2011.10.19 Changed to CommitTran implementation
                //if (HttpContext.Current.Items["__OTHERDEALS"] != null && HttpContext.Current.Items["__OTHERDEALS"].ToString() == "1")
                //    dealAllEx(false);    / / Start the implementation of processing order (already in the implementation will not be implemented)
                // If you just interrupt here, then the trouble? In particular, there are a lot of DealOtherDeal, may lose a lot
                // In addition there are problems in the order? Such as the first off error, the second off normal, it is possible that the program did not think the first customs clearance than the first?
                return ret;
            }
            catch
            {
                NService.DDD.DomainContext.Instace.Reset();
                DBHelper.Instance.RollbackTran();
                throw;
            }*/
        }


        protected virtual object baseCall(object serviceObj, string serviceId, string command, params object[] args)
        {
            try
            {

                if (serviceObj is IService)
                {
                    return (serviceObj as IService).Call(command, args);
                }
                else
                {
                    return serviceObj.GetType().InvokeMember(
                        command
                        , BindingFlags.Default | BindingFlags.InvokeMethod
                        , null
                        , serviceObj
                        , args);
                }
            }
            catch (TargetInvocationException tex)
            {
                Exception innerEx = tex.InnerException;
                if (innerEx is BusNeedLoginException
                    || innerEx is BusNoPermissionException
                    || innerEx is BusErrorException)
                    throw innerEx;

                else if (innerEx is BusInfoException)
                {
                    throw new BusInfoException(innerEx.Message.ToString(), innerEx);
                }
                else if (innerEx is ApplicationException)
                {
                    throw new ApplicationException("Message error: " + serviceId + "." + command + ":" + innerEx.InnerException.Message, innerEx.InnerException == null ? null : innerEx.InnerException);
                }
                else
                {
                    throw new ApplicationException("Message error: " + innerEx.Message.ToString(), innerEx);
                }


            }
            catch (MissingMethodException ex)
            {
                string argInfo = "";
                if (args != null)
                {
                    foreach (object arg in args)
                        argInfo += (argInfo.Length > 0 ? "," : "") + (arg == null ? "null" : arg.GetType().Name);
                }
                //Tool.Error("Exception Server.ServiceCaller.baseCall",ex.Message,"Params",argInfo);
                throw new ApplicationException("Exception Server.ServiceCaller.baseCall: " + ex.Message + "; Params:" + argInfo);
            }
        }

    }

    public interface IService
    {
        object Call(string command, object[] args);
    }
}