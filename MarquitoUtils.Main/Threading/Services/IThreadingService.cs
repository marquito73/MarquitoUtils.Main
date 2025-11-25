namespace MarquitoUtils.Main.Threading.Services
{
    /// <summary>
    /// Threading service
    /// </summary>
    public interface IThreadingService
    {
        /// <summary>
        /// Manage data to process on differents threads (multi threading)
        /// </summary>
        /// <typeparam name="T">Type of data</typeparam>
        /// <param name="numberOfThreads">The number of threads</param>
        /// <param name="dataToProcess">The data to process</param>
        /// <param name="process">The process method</param>
        /// <param name="endAction">The end action method</param>
        /// <returns>The thread, who manage all process thread (abort this thread abort all process thread)</returns>
        public Thread PartitionDataProcess<T>(int numberOfThreads, List<T> dataToProcess,
            Action<List<T>> process, Action endAction = null);
    }
}
