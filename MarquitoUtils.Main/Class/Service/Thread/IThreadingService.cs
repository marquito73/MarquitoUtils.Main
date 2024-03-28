using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarquitoUtils.Main.Class.Service.Threading
{
    public interface IThreadingService
    {
        public Thread PartitionDataProcess<T>(int numberOfThreads, List<T> dataToProcess,
            Action<List<T>> process, Action endAction = null);
    }
}
