using DelayQueue.Interfaces;
using System;

namespace DelayQueue
{
    /// <summary>
    /// 默认延时对象
    /// </summary>
    public class DelayItem<T> : IDelayItem
    {
        /// <summary>
        /// 过期时间戳，绝对时间
        /// </summary>
        public readonly long TimeoutMs;

        /// <summary>
        /// 延时对象
        /// </summary>
        public readonly T Item;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeoutSpan">过期时间，相对时间</param>
        /// <param name="item">延时对象</param>
        public DelayItem(TimeSpan timeoutSpan, T item)
        {
            TimeoutMs = (long)timeoutSpan.TotalMilliseconds + GetTimestamp();
            Item = item;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeoutMs">过期时间戳，绝对时间</param>
        /// <param name="item">延时对象</param>
        public DelayItem(long timeoutMs, T item)
        {
            TimeoutMs = timeoutMs;
            Item = item;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }

            if (obj is DelayItem<T> value)
            {
                return TimeoutMs.CompareTo(value.TimeoutMs);
            }

            throw new ArgumentException($"Object is not a {nameof(DelayItem<T>)}");
        }

        public TimeSpan GetDelaySpan()
        {
            var delayMs = Math.Max(TimeoutMs - GetTimestamp(), 0);
            return TimeSpan.FromMilliseconds(delayMs);
        }

        /// <summary>
        /// 获取当前时间戳
        /// </summary>
        /// <returns></returns>
        private long GetTimestamp()
        {
            return new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();
        }
    }
}