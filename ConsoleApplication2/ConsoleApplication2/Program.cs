using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main()
        {
            const int iterations = 10000;
            while (true)
            {
                Run("Uncontended AsyncLock ", () => TestAsyncLockAsync(iterations, false));
                Run("Uncontended AwCritSec ", () => TestAwaitableCriticalSectionAsync(iterations, false));
                Run("Contended   AsyncLock ", () => TestAsyncLockAsync(iterations, true));
                Run("Contended   AwCritSec ", () => TestAwaitableCriticalSectionAsync(iterations, true));
                Console.WriteLine();
            }
        }

        static void Run(string name, Func<Task> test)
        {
            var sw = Stopwatch.StartNew();
            test().Wait();
            sw.Stop();
            Console.WriteLine("{0}: {1}ms", name, sw.ElapsedMilliseconds);
        }

        static async Task TestAsyncLockAsync(int iterations, bool isContended)
        {
            var mutex = new AsyncLock();
            if (isContended)
            {
                var waits = new Task<IDisposable>[iterations];
                using (await mutex.LockAsync())
                    for (int i = 0; i < iterations; i++)
                        waits[i] = mutex.LockAsync();
                for (int i = 0; i < iterations; i++)
                    using (await waits[i]) { }
            }
            else
            {
                for (int i = 0; i < iterations; i++)
                    using (await mutex.LockAsync()) { }
            }
        }

        static async Task TestAwaitableCriticalSectionAsync(int iterations, bool isContended)
        {
            var mutex = new AwaitableCriticalSection();
            if (isContended)
            {
                var waits = new Task<IDisposable>[iterations];
                using (await mutex.EnterAsync())
                    for (int i = 0; i < iterations; i++)
                        waits[i] = mutex.EnterAsync();
                for (int i = 0; i < iterations; i++)
                    using (await waits[i]) { }
            }
            else
            {
                for (int i = 0; i < iterations; i++)
                    using (await mutex.EnterAsync()) { }
            }
        }
    }
}
