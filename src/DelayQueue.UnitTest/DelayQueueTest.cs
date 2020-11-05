using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DelayQueue.UnitTest
{
    public class DelayQueueTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestDelayQueue()
        {
            var delayQueue = new DelayQueue<DelayItem<Action>>();

            // 输出列表
            var outputs = new Dictionary<string, DateTime>();
            outputs.Add("00", DateTime.Now);

            // 添加任务
            var item1 = new DelayItem<Action>(TimeSpan.FromSeconds(5), () => { outputs.Add("50", DateTime.Now); });
            var item2 = new DelayItem<Action>(TimeSpan.FromSeconds(2), () => { outputs.Add("20", DateTime.Now); });
            delayQueue.TryAdd(item1);
            delayQueue.TryAdd(item2);
            delayQueue.TryAdd(item2);

            delayQueue.TryAdd(new DelayItem<Action>(TimeSpan.FromSeconds(12), () => { outputs.Add("120", DateTime.Now); }));
            delayQueue.TryAdd(new DelayItem<Action>(TimeSpan.FromSeconds(2), () => { outputs.Add("21", DateTime.Now); }));

            Assert.AreEqual(4, delayQueue.Count);

            // 获取任务
            while (delayQueue.Count > 0)
            {
                if (delayQueue.TryTake(out var task))
                {
                    task.Item.Invoke();
                }
            }

            Console.WriteLine(string.Join(Environment.NewLine, outputs.Select(o => $"{o.Key}, {o.Value:HH:mm:ss.ffff}")));

            Assert.AreEqual(2, Calc(outputs.Skip(1).First().Value, outputs.First().Value));
            Assert.AreEqual(2, Calc(outputs.Skip(2).First().Value, outputs.First().Value));
            Assert.AreEqual(5, Calc(outputs.Skip(3).First().Value, outputs.First().Value));
            Assert.AreEqual(12, Calc(outputs.Skip(4).First().Value, outputs.First().Value));
        }

        private static int Calc(DateTime dt1, DateTime dt2)
        {
            // 毫秒数是存在误差的，这里统计秒数
            return (int)(CutOffMillisecond(dt1) - CutOffMillisecond(dt2)).TotalSeconds;
        }

        /// <summary>
        /// 截掉毫秒部分
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static DateTime CutOffMillisecond(DateTime dt)
        {
            return new DateTime(dt.Ticks - (dt.Ticks % TimeSpan.TicksPerSecond), dt.Kind);
        }
    }
}