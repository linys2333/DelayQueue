using System;
using System.Diagnostics;
using System.Threading;

namespace DelayQueue.Extensions
{
    /// <summary>
    /// Monitor扩展
    /// </summary>
    public static class MonitorExt
    {
        /// <summary>
        /// 锁等待，返回剩余时间
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="timeout">超时时间，如果是Infinite则无限期等待</param>
        /// <returns></returns>
        public static TimeSpan Wait(object obj, TimeSpan timeout)
        {
            // Monitor.Wait阻塞并释放锁，将当前线程置于等待队列，直至Monitor.Pulse通知其进入就绪队列，
            // 或者超时未接到通知，自动进入就绪队列。
            // timeout是进入就绪队列之前等待的时间，返回false表示已超时。
            // 进入就绪队列后会尝试获取锁，但直至拿到锁之前都不会返回值。

            var sw = Stopwatch.StartNew();
            Monitor.Wait(obj, timeout);
            sw.Stop();

            return timeout - sw.Elapsed;
        }
    }
}