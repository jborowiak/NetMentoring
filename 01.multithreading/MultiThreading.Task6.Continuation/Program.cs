/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            var task = Task.Run(() =>
                {
                    Console.WriteLine("Parent task start");
                    Console.WriteLine($"Parent task thread: {Thread.CurrentThread.ManagedThreadId}");
                    Console.WriteLine("Parent task end");
                })
                .ContinueWith(ant =>
                {
                    Console.WriteLine($"Child task is thread pool: {Thread.CurrentThread.ManagedThreadId}");
                    Console.WriteLine($"Child task thread: {Thread.CurrentThread.Name}");
                    Console.WriteLine("Continuation");
                }, TaskContinuationOptions.None);

            task.Wait();


            var tokenSource2 = new CancellationTokenSource();
            CancellationToken ct = tokenSource2.Token;

            var taskD = Task.Run(() =>
                {
                    Console.WriteLine("Parent task start");
                    bool moreToDo = true;
                    while (moreToDo)
                    {
                        // Poll on this property if you have to do
                        // other cleanup before throwing.
                        if (ct.IsCancellationRequested)
                        {
                            // Clean up here, then...
                            Console.WriteLine("Parent task cancel requested");
                            ct.ThrowIfCancellationRequested();
                        }
                    }
                    Console.WriteLine($"Parent task thread: {Thread.CurrentThread.ManagedThreadId}");
                    Console.WriteLine("Parent task end");
                }, tokenSource2.Token)
                .ContinueWith(ant =>
                {
                    Console.WriteLine($"Child task is thread pool: {Thread.CurrentThread.ManagedThreadId}");
                    Console.WriteLine($"Child task thread: {Thread.CurrentThread.Name}");
                    Console.WriteLine("Continuation");
                }, TaskContinuationOptions.None);

            tokenSource2.Cancel();

            taskD.Wait();

            Console.ReadLine();
        }
    }
}
