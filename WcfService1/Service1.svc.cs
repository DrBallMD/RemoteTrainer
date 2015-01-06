using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Web;
using System.Web.Hosting;


namespace WcfService1
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service1" в коде, SVC-файле и файле конфигурации.
    // ПРИМЕЧАНИЕ. Чтобы запустить клиент проверки WCF для тестирования службы, выберите элементы Service1.svc или Service1.svc.cs в обозревателе решений и начните отладку.
    public class Service1 : IService1
    {
        private string connstr = HostingEnvironment.ApplicationPhysicalPath + "ais.db";
        
        public List<AISdb.AISTask> getTasks()
        {
            AISdb.AISdb db = AISdb.AISdb.getInstance();
            db.open(connstr);
            List<AISdb.AISTask> result = db.getTasks();
            db.close();
            return result;
        }
        public string generatePath(string a)
        {
             return Convert.ToBase64String (new System.Security.Cryptography.MD5CryptoServiceProvider ().ComputeHash (Encoding.ASCII.GetBytes(DateTime.Now.Millisecond.ToString () + a))).Replace('\\','[').Replace('/',']');
        }
        
        /// <summary>
        /// Метод добавления задачи,
        /// </summary>
        /// <param name="tt"></param>
        /// <param name="parameters">для нейронных сетей 0 - нейроны на входе, 1 = скрытые слои, 2=кол-во нейронов в скрытых слоях 3= нейроны на выходе 4=активационная ф-я(нолик)</param>
        public void addTask(AISdb.AISTask tt, List<string> parameters)
        {
            switch (tt.ttype)
            {
                case AISdb.TaskType.ArtificialNeuralNetwork:
                    tt.fpath = HostingEnvironment.ApplicationPhysicalPath + "AIS\\NN\\" + generatePath(tt.name + tt.author) + "\\";
                    ANeuralNetwork.ANetwork nn = new ANeuralNetwork.ANetwork(Int32.Parse(parameters[0]), Int32.Parse(parameters[1]), Int32.Parse(parameters[2]), Int32.Parse(parameters[3]), Int32.Parse(parameters[4]));
                    XmlSerializer xs = new XmlSerializer(nn.GetType());
                    Directory.CreateDirectory(tt.fpath);
                    FileStream file = File.Create(tt.fpath + "network.xml");
                    xs.Serialize(file, nn);
                    file.Close();
                    break;
                case AISdb.TaskType.GeneticAlgorithm:
                    tt.fpath = HostingEnvironment.ApplicationPhysicalPath + "AIS\\GA\\" + generatePath(tt.name + tt.author) + "\\";
                    Directory.CreateDirectory (tt.fpath);
                    //TODO: Магия
                    break;
                default: throw new Exception("wrong type");
            }
            AISdb.AISdb db = AISdb.AISdb.getInstance();
            db.open(connstr);
            db.addTask(tt);
            db.close();
        }
        public ANeuralNetwork.ANetwork getNeuralNetwork(int id)
        {
             AISdb.AISdb inst = AISdb.AISdb.getInstance ();
             inst.open (connstr);
             string fpath = inst.getTaskFilepath (id)+"network.xml";
             inst.close ();
             XmlSerializer xs = new XmlSerializer (typeof(ANeuralNetwork.ANetwork));
             FileStream file = File.OpenRead (fpath);
             ANeuralNetwork.ANetwork result = (ANeuralNetwork.ANetwork)xs.Deserialize (file);
             file.Close ();
             return result;

        }
        public void setNetworkResults(int id, ANeuralNetwork.ANetwork net, float newError)
        {
             AISdb.AISdb inst = AISdb.AISdb.getInstance ();
             inst.open (connstr);
             inst.setTaskError (id, newError);
             string fpath = inst.getTaskFilepath (id)+"network.xml";
             inst.close ();
             XmlSerializer xs = new XmlSerializer (typeof(ANeuralNetwork.ANetwork));
             FileStream file = File.OpenWrite (fpath);
             xs.Serialize (file, net);
             file.Close ();
        }
        public float getError(int id)
        {
             float result = 0.0f;
             AISdb.AISdb inst = AISdb.AISdb.getInstance ();
             inst.open (connstr);
             result = inst.getTaskError (id);
             inst.close ();
             return result;
        }
        public List<ANeuralNetwork.StudyData> getStudyData(int id)
        {
             AISdb.AISdb inst = AISdb.AISdb.getInstance ();
            List<ANeuralNetwork.StudyData> result;  
            inst.open (connstr);
             string fpath = inst.getTaskFilepath(id);
             inst.close ();
             XmlSerializer xs = new XmlSerializer (typeof(List<ANeuralNetwork.StudyData>));
             FileStream file = File.Open (fpath + "data.xml",FileMode.OpenOrCreate);
             if (file.Length > 0)
             {
                 result = (List<ANeuralNetwork.StudyData>)xs.Deserialize(file);
             }
             else
             {
                 result = new List<ANeuralNetwork.StudyData>();
             }
             file.Close ();
             return result;
        }

        public void addStudyData(int id, List<ANeuralNetwork.StudyData> data)
        {
             AISdb.AISdb inst = AISdb.AISdb.getInstance ();
             inst.open (connstr);
             string fpath = inst.getTaskFilepath(id);
             inst.close ();
             XmlSerializer xs = new XmlSerializer (typeof(List<ANeuralNetwork.StudyData>));
             FileStream file = File.Open (fpath + "data.xml",FileMode.OpenOrCreate);
             List<ANeuralNetwork.StudyData> old_data;
             if (file.Length != 0) {
                 old_data = (List<ANeuralNetwork.StudyData>)xs.Deserialize(file);
             } else {
                 old_data = new List<ANeuralNetwork.StudyData> ();
             }
             old_data.AddRange (data);
             xs.Serialize (file, old_data);
             file.Close ();
        }
        public void updateTask(AISdb.AISTask tt)
        {
            AISdb.AISdb db = AISdb.AISdb.getInstance();
            db.open(connstr);
            db.updateTask(tt);
            db.close();
        }
        public void deleteTask(int id)
        {
            AISdb.AISdb db = AISdb.AISdb.getInstance();
            db.open(connstr);
            db.deleteTask(id);
            db.close();
        }
    }
}
