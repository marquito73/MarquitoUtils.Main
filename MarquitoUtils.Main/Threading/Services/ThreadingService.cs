using MarquitoUtils.Main.Common.Services;
using MarquitoUtils.Main.Common.Tools;

namespace MarquitoUtils.Main.Threading.Services
{
    /// <summary>
    /// Threading service
    /// </summary>
    public class ThreadingService : DefaultService, IThreadingService
    {
        public Thread PartitionDataProcess<T>(int numberOfThreads, List<T> dataToProcess,
            Action<List<T>> process, Action endAction = null)
        {
            List<List<T>> subFilesLists = dataToProcess
                .Select((s, i) => new { s, i })
                .GroupBy(x => x.i % numberOfThreads)
                .Select(g => g.Select(x => x.s).ToList())
                .ToList();

            List<Thread> threads = subFilesLists.Select(files =>
            {
                return new Thread(() =>
                {
                    process(files);
                });
            }).ToList();

            Thread manageProcessThread = new Thread(() =>
            {
                bool threadIsOk = true;

                try
                {
                    threads.ForEach(thread => thread.Start());

                    while (threadIsOk)
                    {
                        if (threads.None(thread => thread.IsAlive))
                        {
                            threadIsOk = false;
                        }
                    }

                    if (Utils.IsNotNull(endAction))
                    {
                        endAction();
                    }
                }
                catch (Exception ex)
                {
                    threadIsOk = false;
                    threads.ForEach(thread => thread.Interrupt());
                }
            });
            manageProcessThread.Start();

            return manageProcessThread;
        }
    }
}
