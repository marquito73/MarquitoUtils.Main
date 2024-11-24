using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarquitoUtils.Main.Class.Tools
{
    /// <summary>
    /// Utils class for Threading
    /// </summary>
    public class ThreadUtils
    {
        /// <summary>
        /// Get a cyclic thread, ready to start
        /// </summary>
        /// <param name="millisecondsDelay">Delay in milliseconds, 
        /// on which the thread make a cycle, as long as he isn't aborted</param>
        /// <param name="threadAction">The code to execute for this thread</param>
        /// <returns></returns>
        public static Thread GetCyclicThread(int millisecondsDelay,
            Action threadAction)
        {
            return new Thread(() =>
            {
                bool threadIsOk = true;

                while (threadIsOk)
                {
                    try
                    {
                        // Make action for the thread
                        threadAction();
                        // This thread is in loop, cadenced by specific time
                        Thread.Sleep(millisecondsDelay);
                    } catch (Exception ex)
                    {
                        // We go here when we ask to thread to stop
                        threadIsOk = false;
                    }
                }
            });
        }

        public static Thread GetThread(Action threadAction, Action<Exception>? errorAction = null)
        {
            return new Thread(() =>
            {
                bool threadIsOk = true;

                try
                {
                    // Make action for the thread
                    threadAction();
                }
                catch (Exception ex)
                {
                    // We go here when we ask to thread to stop
                    threadIsOk = false;

                    if (Utils.IsNotNull(errorAction))
                    {
                        errorAction(ex);
                    }
                }
            });
        }
    }
}
