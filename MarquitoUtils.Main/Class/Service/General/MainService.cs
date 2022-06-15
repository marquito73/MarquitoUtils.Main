namespace MarquitoUtils.Main.Class.Service.General
{
    /// <summary>
    /// Main service
    /// </summary>
    public interface MainService
    {
        /// <summary>
        /// Can launch a thread without parameters
        /// </summary>
        /// <param name="classType">Method class</param>
        /// <param name="classMethodName">Method name</param>
        /// <returns>A boolean value indicate the thread is launched with success</returns>
        public bool startThread(Type classType, string classMethodName);

        /// <summary>
        /// Can launch a thread without parameters
        /// </summary>
        /// <param name="classType">Method class</param>
        /// <param name="classMethodName">Method name</param>
        /// <param name="threadType">The thread type, choose MTA if you want the thread can modify UI controls</param>
        /// <returns>A boolean value indicate the thread is launched with success</returns>
        public bool startThread(Type classType, string classMethodName, ApartmentState threadType);

        /// <summary>
        /// Can launch a thread with one parameter
        /// </summary>
        /// <param name="classType">Method class</param>
        /// <param name="classMethodName">Method name</param>
        /// <param name="parameter">Parameter</param>
        /// <returns>A boolean value indicate the thread is launched with success</returns>
        public bool startThread(Type classType, string classMethodName, object parameter);

        /// <summary>
        /// Can launch a thread with one parameter
        /// </summary>
        /// <param name="classType">Method class</param>
        /// <param name="classMethodName">Method name</param>
        /// <param name="threadType">The thread type, choose MTA if you want the thread can modify UI controls</param>
        /// <param name="parameter">Parameter</param>
        /// <returns>A boolean value indicate the thread is launched with success</returns>
        public bool startThread(Type classType, string classMethodName, ApartmentState threadType, object parameter);

        /// <summary>
        /// Can launch a thread with parameters
        /// </summary>
        /// <param name="classType">Method class</param>
        /// <param name="classMethodName">Method name</param>
        /// <param name="parameters">Parameters (order is very important)</param>
        /// <returns>A boolean value indicate the thread is launched with success</returns>
        public bool startThread(Type classType, string classMethodName, List<object> parameters);

        /// <summary>
        /// Can launch a thread with parameters
        /// </summary>
        /// <param name="classType">Method class</param>
        /// <param name="classMethodName">Method name</param>
        /// <param name="threadType">The thread type, choose MTA if you want the thread can modify UI controls</param>
        /// <param name="parameters">Parameters (order is very important)</param>
        /// <returns>A boolean value indicate the thread is launched with success</returns>
        public bool startThread(Type classType, string classMethodName, ApartmentState threadType, List<object> parameters);
    }
}
