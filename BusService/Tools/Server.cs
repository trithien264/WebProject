using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BusService.Tools
{
    class Server
    {
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
                        /*else
                        {
                            try
                            {
                                args = JsonConvert.DeserializeObject<object[]>(dic["params"].ToString());
                            }
                            catch (Exception)
                            {
                                try
                                {
                                    args = new object[1];
                                    args[0] = JsonConvert.DeserializeObject<object>(dic["params"].ToString());
                                }
                                catch (Exception)
                                {
                                }
                               
                            }
                           
                        }*/
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
                int dotIndex = service.LastIndexOf(".");
                if (dotIndex <= 0 || dotIndex >= service.Length - 1)
                    throw new ApplicationException("Invalid service:" + service);
                string serviceId = service.Substring(0, dotIndex);
                string command = service.Substring(dotIndex + 1);

                object serviceObj = ObjectFactory.Instance.Get<object>(serviceId);
                if (serviceObj == null)
                    throw new ApplicationException("Service not found:" + serviceId);

                return baseCall(serviceObj, serviceId, command, args);
            }
            finally
            {

            }
        }

        protected virtual object baseCall(object serviceObj, string serviceId, string command, params object[] args)
        {
            try
            {
                return serviceObj.GetType().InvokeMember(
                    command
                    , BindingFlags.Default | BindingFlags.InvokeMethod
                    , null
                    , serviceObj
                    , args);


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

    public class ObjectFactory
    {
        public static readonly ObjectFactory Instance = new ObjectFactory();


        static object _lockGetLockObjId = new object();
        static Dictionary<string, object> _lockID = new Dictionary<string, object>();
        static object getLockObj(string objectID)
        {
            lock (_lockGetLockObjId)
            {
                if (!_lockID.ContainsKey(objectID))
                {
                    _lockID.Add(objectID, new object());
                }
                return _lockID[objectID];
            }
        }
        public T Get<T>(string objectID)
        {
            string cacheKey = objectID;


            lock (getLockObj(cacheKey))
            {

                string objectType = objectID;
                T ret = default(T);
                string dll = null;
                object[] args = null;
                string url = null;
                Dictionary<string, object> soapHeader = null;
                string dyDll = null;

                Dictionary<string, object> fields = null;

                ret = CreateObject<T>(objectType, dll, args, ref dyDll, objectType == objectID);




                return ret;
            }
        }

        public T CreateObject<T>(string objectType, string dll, object[] args, ref string dyDll, bool tryT)
        {
            T ret = default(T);
            Type t = GetType<T>(objectType, dll, ref dyDll, tryT);
            if (t != null)
            {
                FieldInfo fi = t.GetField("Instance", BindingFlags.Public | BindingFlags.Static);


                if (ret == null)
                {
                    try
                    {
                        ret = (T)Activator.CreateInstance(t, args);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Exception 'ObjectFactory.CreateObject': " + ex.InnerException.Message.ToString(), ex.InnerException == null ? ex : ex.InnerException);
                    }
                }
            }

            return ret;
        }
        public Type GetType<T>(string objectType, string dll, ref string dyDll, bool tryT)
        {
            Type t = null;
            string tType = tryT && typeof(T) != typeof(object) && typeof(T).FullName != objectType && !typeof(T).IsInterface && !typeof(T).IsAbstract ? typeof(T).FullName : null;
            Dictionary<string, int> repeatAsses = new Dictionary<string, int>();
            Assembly[] asses = null;
            Assembly v_ass = System.AppDomain.CurrentDomain.GetAssemblies().Where(es => es.FullName.Contains(objectType.Split('.')[0])).FirstOrDefault();
            if (v_ass != null)
            {
                asses = new Assembly[1];
                asses[0] = v_ass;
            }
            else
                asses = System.AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly ass in asses)
            {
                if (!ass.GlobalAssemblyCache)
                {
                    if (!repeatAsses.ContainsKey(ass.FullName))
                        repeatAsses.Add(ass.FullName, 0);
                    repeatAsses[ass.FullName]++;
                }
            }
            foreach (Assembly ass in asses)
            {
                //Not find. Net itself as a service object?
                if (!ass.GlobalAssemblyCache)
                {
                    t = ass.GetType(objectType, false);
                    if (t != null)
                    {
                        // TODO: Exclude those who are dynamic dll, but can not find, that obsolete, a new version
                        // TODO: If you need to reference in the app or dll third-party dll, do not want it into the bin directory, it is possible
                        // But with the version number, keep consistent

                        // This is usually used to find the way, if not cache, expired, there should be the latest version of the
                        if (repeatAsses[ass.FullName] == 1)
                            break;
                    }
                    else if (tType != null)
                    {
                        t = ass.GetType(tType, false);
                        if (t != null)
                        {
                            if (repeatAsses[ass.FullName] == 1)
                                break;
                        }
                    }
                }
            }


            if (t == null && tType != null)
                t = typeof(T);
            return t;
        }

    }
}
