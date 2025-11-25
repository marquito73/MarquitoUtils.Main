using System.Reflection;
using MarquitoUtils.Main.Common.Tools;
using MarquitoUtils.Main.Threading;
using static MarquitoUtils.Main.Common.Enums.EnumDataType;

namespace MarquitoUtils.Main.Common.Services
{
    /// <summary>
    /// Main service
    /// </summary>
    public class MainServiceImpl : MainService
    {
        public bool startThread(Type serviceType, string serviceMethodName)
        {
            return startThread(serviceType, serviceMethodName, ApartmentState.MTA);
        }

        public bool startThread(Type serviceType, string serviceMethodName, ApartmentState threadType)
        {
            return startThread(serviceType, serviceMethodName, threadType, null);
        }

        public bool startThread(Type serviceType, string serviceMethodName, object parameter)
        {
            List<object> parameters = new List<object>();
            parameters.Add(parameter);
            return startThread(serviceType, serviceMethodName, ApartmentState.MTA, parameters);
        }

        public bool startThread(Type serviceType, string serviceMethodName, ApartmentState threadType, object parameter)
        {
            List<object> parameters = new List<object>();
            parameters.Add(parameter);
            return startThread(serviceType, serviceMethodName, threadType, parameters);
        }

        public bool startThread(Type serviceType, string serviceMethodName, List<object> parameters)
        {
            return startThread(serviceType, serviceMethodName, ApartmentState.MTA, parameters);
        }

        public bool startThread(Type serviceType, string serviceMethodName, ApartmentState threadType, List<object> parameters)
        {
            bool state = false;
            if (!Utils.IsNull(serviceType) && !Utils.IsNullOrEmpty(serviceMethodName))
            {
                MethodInfo serviceThreadMethod = null;
                Thread serviceThread = null;
                if (!Utils.IsNull(parameters) && parameters.Count > 0)
                {
                    List<Type> types = new List<Type>();
                    foreach (object param in parameters)
                    {
                        types.Add(param.GetType());
                    }
                    serviceThreadMethod = serviceType.GetMethod(serviceMethodName, 
                        BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static, null, types.ToArray(), null);
                    serviceThread = new Thread(() => serviceThreadMethod.Invoke(serviceType, parameters.ToArray()));
                    serviceThread.SetApartmentState(threadType);
                }
                else
                {
                    serviceThreadMethod = serviceType.GetMethod(serviceMethodName, 
                        BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static, null, new Type[] { }, null);
                    serviceThread = new Thread(() => serviceThreadMethod.Invoke(serviceType, null));
                    serviceThread.SetApartmentState(threadType);
                }
                serviceThread.Start();
                //List<ThreadContext> lstThread = (List<ThreadContext>)AppDataPropManage.getValue((int)enumDataType.THREAD_LIST);
                List<ThreadContext> lstThread = new List<ThreadContext>();
                if (Utils.IsNull(lstThread))
                {
                    lstThread = new List<ThreadContext>();
                }
                ThreadContext threadContext = new ThreadContext(serviceThread, serviceType.ToString());
                lstThread.Add(threadContext);
                //AppDataPropManage.addDataSpecialValue(lstThread, enumDataType.THREAD_LIST);
                state = true;
            }
            return state;
        }
    }
}
