﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MarquitoUtils.Main.Class.Entities.Threading
{
    /// <summary>
    /// Can contain a thread with his context
    /// </summary>
    public class ThreadContext
    {
        /// <summary>
        /// A thread
        /// </summary>
        public Thread Thread { get; set; }
        /// <summary>
        /// Context of the thread
        /// </summary>
        public string Context { get; set; }

        /// <summary>
        /// A thread context
        /// </summary>
        /// <param name="thread">A thread</param>
        /// <param name="context">Context of the thread</param>
        public ThreadContext(Thread thread, string context)
        {
            this.Thread = thread;
            this.Context = context;
        }
    }
}
