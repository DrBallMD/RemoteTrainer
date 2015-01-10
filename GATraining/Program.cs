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
        static ServiceReference1.AISTask task;
        static List<string> parametrs;
        static void Main(string[] args)
        {
            #region addTask-worked
            //task = new ServiceReference1.AISTask();
            //parametrs = new List<string>() { "-50", "50", "3", "0,04" };
            //task.author = "drballmd";
            //task.description = "GeneticLibrary.Maximization";
            //task.name = "test_ga";
            //task.ttype = ServiceReference1.TaskType.GeneticAlgorithm;
            //service.addTask(task, parametrs.ToArray());
            #endregion
            File.WriteAllBytes("GeneticLibrary.dll", service.GetAssembly("GeneticLibrary.dll"));
            task = service.getTasks().Where(x => x.id == 9).First();
            IGenetical aa =  GenXmlSerialization.BinSerialization.ByteToObject(service.getGeneticAlgorithm(task.id)) as AlgoLib.IGenetical;
            aa.NextGeneration();
            aa.NextGeneration();
            service.setGeneticAlgorithm(task.id, GenXmlSerialization.BinSerialization.OgjectToByte(aa), (float)0.001);
            Console.ReadKey();
            
            
        }
        #region notnow
        ////File.WriteAllBytes("GeneticLibrary.dll", service.GetAssembly());
        
        //Type fooType = assembly.GetType("GeneticLibrary.Maximization");
        //IGenetical foo = Activator.CreateInstance(fooType, -50, 50, 3, 0.04) as IGenetical;
        ////List<int> result = new List<int>();
        ////Maximization aa = new Maximization(-50, 50, 3, 0.04);
        //for (int i = 1; i < 100; i++)
        //{
        //    foo.NextGeneration();
        //    //result.Add(aa.GetBest());
        //    Console.WriteLine(foo.GetBest());

        //}
        ///IGenetical aa = GenXmlSerialization.XmlSerialization.AdvancedObjectDeserialize<IGenetical>(Directory.GetCurrentDirectory() + "\\fs.xml");
        //result = result.Distinct().OrderByDescending(x => x).Take(4).ToList();
        //string[] arr = new string[result.Count];
        //for (int i = 0; i < result.Count; i++)
        //{
        //    arr[i] = aa.IntToBit(result[i]);
        //}
        //File.WriteAllLines("output.txt", arr);     
        #endregion
    }
}
