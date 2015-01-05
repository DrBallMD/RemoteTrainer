using System;
using System.Data;
using Mono.Data.Sqlite;
using System.Collections.Generic;

namespace AISdb
{
	public class AISdb
	{
		private static AISdb instance;

		private AISdb(){}
		private IDbConnection connection;
		private IDbCommand cmd;

		public static AISdb getInstance(){
			if (instance == null) {
				instance = new AISdb ();
			}
			return instance;
		}

		public void open(string connection_string){
			connection = (IDbConnection)new SqliteConnection (connection_string);
			connection.Open ();
			cmd = connection.CreateCommand ();
			cmd.CommandText = "CREATE TABLE IF NOT EXISTS AISTasks (ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, NAME TEXT NOT NULL, AUTHOR TEXT NOT NULL, DESCRIPTION TEXT, TYPE INTEGER, ERROR REAL NOT NULL, FILEPATH TEXT NOT NULL);";
			cmd.ExecuteNonQuery ();
		}

		public void addTask(AISTask newtask){
			//throw new Exception ("not implemented yet");
			cmd.CommandText = "INSERT INTO AISTasks(NAME, AUTHOR, DESCRIPTION, TYPE, ERROR, FILEPATH) VALUES( @pName, @pAuthor, @pDescription, @pType, @pError, @pFilepath);";
			cmd.Parameters.Add(new SqliteParameter("@pName",newtask.name));
			cmd.Parameters.Add (new SqliteParameter ("@pAuthor", newtask.author));
			cmd.Parameters.Add (new SqliteParameter ("@pDescription", newtask.description));
			cmd.Parameters.Add (new SqliteParameter ("@pType", newtask.ttype==TaskType.GeneticAlgorithm?0:1));
			cmd.Parameters.Add (new SqliteParameter ("@pError", newtask.current_error));
			cmd.Parameters.Add (new SqliteParameter ("@pFilepath", newtask.fpath));
			cmd.ExecuteNonQuery ();
			cmd.Parameters.Clear ();
		}

		public void updateTask(AISTask updatedTask){
			cmd.CommandText = "UPDATE AISTasks SET NAME = @pName, AUTHOR = @pAuthor, DESCRIPTION = @pDescription, ERROR = @pError WHERE ID = @pId;";
			cmd.Parameters.Add(new SqliteParameter("@pName",updatedTask.name));
			cmd.Parameters.Add (new SqliteParameter ("@pAuthor", updatedTask.author));
			cmd.Parameters.Add (new SqliteParameter ("@pDescription", updatedTask.description));
			cmd.Parameters.Add (new SqliteParameter ("@pError", updatedTask.current_error));
			cmd.Parameters.Add (new SqliteParameter ("@pId", updatedTask.id));
			cmd.ExecuteNonQuery ();
			cmd.Parameters.Clear ();
		}
		public void deleteTask(int id){
			cmd.CommandText = "DELETE FROM AISTasks WHERE ID= @pId;";
			cmd.Parameters.Add(new SqliteParameter("@pId",id));
			cmd.ExecuteNonQuery ();
			cmd.Parameters.Clear ();
		}

		public float getTaskError(int id){
			cmd.CommandText = "SELECT ERROR FROM AISTasks WHERE ID = @pId;";
			cmd.Parameters.Add (new SqliteParameter ("@pId",id));
			float result = (float)cmd.ExecuteScalar ();
			cmd.Parameters.Clear ();
			return result;
		}

		public void setTaskError(int id, float newError){
			cmd.CommandText = "UPDATE AISTasks SET ERROR = @pError WHERE ID = @pId;";
			cmd.Parameters.Add (new SqliteParameter ("@pError", newError));
			cmd.Parameters.Add(new SqliteParameter("@pId",id));
			cmd.ExecuteNonQuery ();
			cmd.Parameters.Clear ();
		}

		public string getTaskFilepath(int id){
			cmd.CommandText = "SELECT FILEPATH FROM AISTasks WHERE ID = @pId;";
			cmd.Parameters.Add (new SqliteParameter ("@pId",id));
			string result = (string)cmd.ExecuteScalar ();
			cmd.Parameters.Clear ();
			return result;
		}

		public List<AISTask> getTasks(){
			cmd.CommandText = "SELECT * FROM AISTasks;";
			List<AISTask> result = new List<AISTask> ();

			IDataReader rd = cmd.ExecuteReader ();
			while (rd.Read()) {
				AISTask tsk = new AISTask ();
				tsk.id = rd.GetInt32(0);
				tsk.name = rd.GetString(1);
				tsk.author = rd.GetString(2);
				tsk.description = rd.GetString(3);
				tsk.ttype = rd.GetInt32(4)==0?TaskType.GeneticAlgorithm:TaskType.ArtificialNeuralNetwork;
				tsk.current_error = rd.GetFloat(5);
				tsk.fpath= rd.GetString(6);
				result.Add(tsk);
			}
			rd.Close ();
			rd = null;
			return result;
		}

		public void close(){
			cmd.Dispose ();
			cmd = null;
			connection.Dispose ();
			connection = null;
		}
	}
}

