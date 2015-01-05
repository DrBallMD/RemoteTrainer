using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Globalization;
using AlgoLib;
namespace GATraining
{
    class Program
    {
        static ServiceReference1.Service1Client service = new ServiceReference1.Service1Client();
        static void Main(string[] args)
        {
            File.WriteAllBytes("GeneticLibrary.dll", service.GetAssembly());
            Assembly assembly = Assembly.LoadFile(Directory.GetCurrentDirectory()+"\\GeneticLibrary.dll");
            Type fooType = assembly.GetType("GeneticLibrary.Maximization");
            IGenetical foo = Activator.CreateInstance(fooType, -50, 50, 3, 0.04) as IGenetical;
            //List<int> result = new List<int>();
            //Maximization aa = new Maximization(-50, 50, 3, 0.04);
            for (int i = 1; i < 100; i++)
            {
                foo.NextGeneration();
                //result.Add(aa.GetBest());
                Console.WriteLine(foo.GetBest());

            }
            //result = result.Distinct().OrderByDescending(x => x).Take(4).ToList();
            //string[] arr = new string[result.Count];
            //for (int i = 0; i < result.Count; i++)
            //{
            //    arr[i] = aa.IntToBit(result[i]);
            //}
            //File.WriteAllLines("output.txt", arr);     
            Console.ReadKey();
            
            
        }
    }
}
