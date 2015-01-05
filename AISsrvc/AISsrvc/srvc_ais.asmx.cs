
namespace AISsrvc
{
	using System;
	using System.Web;
	using System.Web.Services;
	using System.Collections.Generic;
	using AISdb;
	using System.Text;
	using System.IO;
	using System.Security.Cryptography;
	using System.Reflection;
	using System.Xml.Serialization;

	public class srvc_ais : System.Web.Services.WebService
	{
		private const string conn = "URI=file:aisdatabase.db";

		[WebMethod]
		public List<AISTask> getTaskList(){
			AISdb inst = AISdb.getInstance ();
			inst.open (conn);
			List<AISTask> tsks = inst.getTasks ();
			inst.close ();
			return tsks;
		}
		[WebMethod]
		public ANeuralNetwork.ANetwork getNetwork(int id){
			AISdb inst = AISdb.getInstance ();
			inst.open (conn);
			string fpath = inst.getTaskFilepath (id)+"network.xml";
			inst.close ();
			XmlSerializer xs = new XmlSerializer (typeof(ANeuralNetwork.ANetwork));
			System.IO.FileStream file = System.IO.File.OpenRead (fpath);
			ANeuralNetwork.ANetwork result = (ANeuralNetwork.ANetwork)xs.Deserialize (file);
			file.Close ();
			return result;
		}
		[WebMethod]
		public void setNewNeuralNetworkResults(int id, float newError, ANeuralNetwork.ANetwork net){
			AISdb inst = AISdb.getInstance ();
			inst.open (conn);
			inst.setTaskError (id, newError);
			string fpath = inst.getTaskFilepath (id)+"network.xml";
			inst.close ();
			XmlSerializer xs = new XmlSerializer (typeof(ANeuralNetwork.ANetwork));
			System.IO.FileStream file = System.IO.File.OpenWrite (fpath);
			xs.Serialize (file, net);
			file.Close ();
		}
		[WebMethod]
		public float getError(int id){
			float result = 0.0f;
			AISdb inst = AISdb.getInstance ();
			inst.open (conn);
			result = inst.getTaskError (id);
			inst.close ();
			return result;
		}
		[WebMethod]
		public void addStudyData(int id, List<ANeuralNetwork.StudyData> data){
			AISdb inst = AISdb.getInstance ();
			inst.open (conn);
			string fpath = inst.getTaskFilepath(id);
			inst.close ();
			XmlSerializer xs = new XmlSerializer (typeof(List<ANeuralNetwork.StudyData>));
			System.IO.FileStream file = System.IO.File.Open (fpath + "data.xml",FileMode.OpenOrCreate);
			List<ANeuralNetwork.StudyData> old_data;
			if (file.Length != 0) {
				old_data = (List<ANeuralNetwork.StudyData>)xs.Deserialize (file);
			} else {
				old_data = new List<ANeuralNetwork.StudyData> ();
			}
			old_data.AddRange (data);
			xs.Serialize (file, old_data);
			file.Close ();
		}
		[WebMethod]
		public List<ANeuralNetwork.StudyData> getStudyData(int id){
			AISdb inst = AISdb.getInstance ();
			inst.open (conn);
			string fpath = inst.getTaskFilepath(id);
			inst.close ();
			XmlSerializer xs = new XmlSerializer (typeof(List<ANeuralNetwork.StudyData>));
			System.IO.FileStream file = System.IO.File.OpenRead (fpath + "data.xml");
			List<ANeuralNetwork.StudyData> result = (List<ANeuralNetwork.StudyData>)xs.Deserialize (file);
			file.Close ();
			return result;
		}

		private string generatePath(string inp){
			return Convert.ToBase64String (new System.Security.Cryptography.MD5CryptoServiceProvider ().ComputeHash (Encoding.ASCII.GetBytes(DateTime.Now.Millisecond.ToString () + inp)));
		}
		[WebMethod]
		public void addTask(AISTask tt, List<string> parameters){
			switch (tt.ttype) {
			case TaskType.ArtificialNeuralNetwork:
				tt.fpath = "AIS/NN/" + generatePath (tt.author + tt.name) + "/";
				ANeuralNetwork.ANetwork nn = new ANeuralNetwork.ANetwork (Int32.Parse (parameters [0]), Int32.Parse (parameters [1]), Int32.Parse (parameters [2]), Int32.Parse (parameters [3]), Int32.Parse (parameters [4]));
				XmlSerializer xs = new XmlSerializer (nn.GetType ());
				Directory.CreateDirectory (tt.fpath);
				System.IO.FileStream file = System.IO.File.Create (tt.fpath + "network.xml");
				xs.Serialize (file, nn);
				file.Close();
				break;
			default:
				throw new Exception ("wrong type");
			}
			AISdb inst = AISdb.getInstance ();
			inst.open (conn);
			inst.addTask(tt);
			inst.close ();
		}
	}
}

