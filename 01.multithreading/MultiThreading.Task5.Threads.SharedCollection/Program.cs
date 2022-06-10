/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        private static List<int> _sharedCollection = new List<int>();

        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();



            var task1 = Task.Run(() =>
            {
                for (int i = 0; i <= 10; i++)
                {
                    _sharedCollection.Add(i);
                }
            });

            var task2 = task1.ContinueWith(antecedent =>
            {
                for (int i = 0; i < _sharedCollection.Count; i++)
                {
                    Console.Write("[");
                    for (int j = 0; j <= i; j++)
                    {
                        Console.Write($"{_sharedCollection[j]}, ");
                    }
                    Console.Write("]");
                }
            });

            Task.WaitAll(task2);

            Console.ReadLine();
        }
    }
}
