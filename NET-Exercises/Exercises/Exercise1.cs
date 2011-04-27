using System;
using System.Linq;

namespace Exercises
{
    public class Exercise1
    {
        public static void Run()
        {
//            var source = Observable.Empty<int>();
//            var source = Observable.Throw<int>(new Exception("Oops"));
//            var source = Observable.Return(42);
//            var source = new[] {1, 2, 3, 4}.ToObservable();
//            var source = Observable.Range(1, 5);
//            var source = Observable.Never<int>();

            var source = Observable.GenerateWithTime(
                0, i => i < 5, i => i + 1, i => i*i, i => TimeSpan.FromSeconds(i)
                );

            IDisposable subscription = source.Subscribe(
                x => Console.WriteLine("OnNext: {0}", x),
                ex => Console.WriteLine("OnError: {0}", ex.Message),
                () => Console.WriteLine("OnCompleted")
                );

            Console.WriteLine("Press ENTER to unsubscribe ...");
            Console.ReadLine();

            subscription.Dispose();
        }
    }
}