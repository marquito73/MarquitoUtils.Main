using MarquitoUtils.Main.Class.Tools;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows;

namespace MarquitoUtils.Main.Class.Entities.Action
{
    /// <summary>
    /// Custom action, can executed later by the run()
    /// </summary>
    public class CustomAction
    {
        /// <summary>
        /// The class where the method is 
        /// </summary>
        public Type ActionClass { get; private set; }

        /// <summary>
        /// The method name
        /// </summary>
        public string MethodName { get; private set; }

        /// <summary>
        /// The parameters of the method if there are
        /// </summary>
        public List<object> Parameters { get; private set; }

        /// <summary>
        /// A custom action without parameters
        /// </summary>
        /// <param name="actionClass">The class where the method is located</param>
        /// <param name="methodName">The method name</param>
        public CustomAction(Type actionClass, string methodName)
        {
            this.ActionClass = actionClass;
            this.MethodName = methodName;
            List<object> parameters = new List<object>();
            this.Parameters = parameters;
        }

        /// <summary>
        /// A custom action with one parameter
        /// </summary>
        /// <param name="actionClass">The class where the method is located</param>
        /// <param name="methodName">The method name</param>
        /// <param name="parameter">Parameter</param>
        public CustomAction (Type actionClass, string methodName, object parameter)
        {
            this.ActionClass = actionClass;
            this.MethodName = methodName;
            List<object> parameters = new List<object>();
            parameters.Add(parameter);
            this.Parameters = parameters;
        }


        /// <summary>
        /// A custom action with parameters
        /// </summary>
        /// <param name="actionClass">The class where the method is located</param>
        /// <param name="methodName">The method name</param>
        /// <param name="parameters">Parameters</param>
        public CustomAction(Type actionClass, string methodName, List<object> parameters)
        {
            this.ActionClass = actionClass;
            this.MethodName = methodName;
            this.Parameters = parameters;
        }

        /// <summary>
        /// Execute the method of the class specified with the parameters pass
        /// </summary>
        /// <returns>The result of the method, return null if it's a void</returns>
        public object run()
        {
            object objReturnValue = null;
            System.Action action = new System.Action(() =>
            {
                objReturnValue = this.callMethod(this.ActionClass, this.MethodName, this.Parameters);
            });
            /*if (!AppDataPropManage.applicationIsClosing())
                Application.Current.Dispatcher.Invoke(action);*/

            return objReturnValue;
        }

        /// <summary>
        /// Can invoke method without parameters
        /// </summary>
        /// <param name="actionClass">The class</param>
        /// <param name="classMethodName">The method name</param>
        /// <returns>The result of the method, return null if it's a void</returns>
        private object callMethod(Type actionClass, string classMethodName)
        {
            return callMethod(actionClass, classMethodName, null);
        }

        /// <summary>
        /// Can invoke method with one parameter
        /// </summary>
        /// <param name="actionClass">The class</param>
        /// <param name="classMethodName">The method name</param>
        /// <param name="parameter">Parameter</param>
        /// <returns>The result of the method, return null if it's a void</returns>
        private object callMethod(Type actionClass, string classMethodName, object parameter)
        {
            List<object> parameters = new List<object>();
            parameters.Add(parameter);
            return callMethod(actionClass, classMethodName, parameters);
        }

        /// <summary>
        /// Can invoke method with parameters
        /// </summary>
        /// <param name="actionClass">The class</param>
        /// <param name="classMethodName">The method name</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>The result of the method, return null if it's a void</returns>
        private object callMethod(Type actionClass, string classMethodName, List<object> parameters)
        {
            object methodReturnVal = null;
            if (!Utility.IsNull(actionClass) && !Utility.IsNullOrEmpty(classMethodName))
            {
                MethodInfo serviceThreadMethod = null;
                if (!Utility.IsNull(parameters) && parameters.Count > 0)
                {
                    List<Type> types = new List<Type>();
                    foreach (object param in parameters)
                    {
                        types.Add(param.GetType());
                    }
                    serviceThreadMethod = actionClass.GetMethod(classMethodName,
                        BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static, null, types.ToArray(), null);
                    /*if (!AppDataPropManage.applicationIsClosing())
                        methodReturnVal = serviceThreadMethod.Invoke(actionClass, parameters.ToArray());*/
                }
                else
                {
                    object[] noParameters = new object[0];
                    serviceThreadMethod = actionClass.GetMethod(classMethodName,
                        BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static, null, new Type[] { }, null);
                    /*if (!AppDataPropManage.applicationIsClosing())
                        methodReturnVal = serviceThreadMethod.Invoke(actionClass, noParameters);*/
                }
            }

            return methodReturnVal;
        }
    }
}
