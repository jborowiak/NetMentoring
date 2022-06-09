/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            var task = Task.Run(
                () =>
                {
                    int[] randomArray = new int[10];
                    for (int i = 0; i < randomArray.Length; i++)
                    {
                        randomArray[i] = new Random().Next(100);
                    }
                    ShowArray(randomArray);
                    return randomArray;
                })
                .ContinueWith(
                antecedent =>
                {
                    var multiplier = new Random().Next(10);
                    for (int i = 0; i < antecedent.Result.Length; i++)
                    {
                        antecedent.Result[i] = antecedent.Result[i] * multiplier;
                    }
                    ShowArray(antecedent.Result);
                    return antecedent;
                }, TaskContinuationOptions.OnlyOnRanToCompletion)
                .Unwrap().ContinueWith(
                antecedent =>
                {
                    var sortedArray = antecedent.Result.OrderBy(x => x).ToArray();
                    ShowArray(sortedArray);
                    return sortedArray;
                }, TaskContinuationOptions.OnlyOnRanToCompletion)
                .ContinueWith(
                antecedent =>
                {
                    var sum = antecedent.Result.Sum();
                    var avg = sum / antecedent.Result.Length;
                    Console.WriteLine("Average value = " + avg);
                    return avg;
                }, TaskContinuationOptions.OnlyOnRanToCompletion);

            task.Wait();
        }

        static void ShowArray(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write($"{array[i]} | ");
            }
            Console.WriteLine();
        }
    }
}
