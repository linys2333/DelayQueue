# DelayQueue

C#版延时队列，参考Java DelayQueue实现。

## NuGet

| .NET Standard 2.0+ |
| ----- |
| Install-Package [DelayQueue.Net](https://www.nuget.org/packages/DelayQueue.Net) |

## 使用

``` csharp

// 定义延时队列
// 这里使用默认延时对象DelayItem<T>，也可以自定义实现IDelayItem接口
var delayQueue = new DelayQueue<DelayItem<int>>();

// 定义延时对象
var delayItem1 = new DelayItem<int>(TimeSpan.FromSeconds(1), 1);
var delayItem2 = new DelayItem<int>(TimeSpan.FromSeconds(2), 2);

// 添加进延时队列
delayQueue.TryAdd(delayItem1);
delayQueue.TryAdd(delayItem2);

// 开始输出
Console.WriteLine(DateTime.Now);

while (delayQueue.Count > 0)
{
    if (delayQueue.TryTake(out var item))
    {
        Console.WriteLine($"{DateTime.Now}, {item.Item}");
    }
}

// 控制台输出
11/5 星期四 14:17:06
11/5 星期四 14:17:07, 1
11/5 星期四 14:17:08, 2

```

## 更新日志

v1.0.2更新内容：  
1、添加默认延时对象DelayItem。

v1.0.3更新内容：  
1、优化入队出队的校验逻辑。
