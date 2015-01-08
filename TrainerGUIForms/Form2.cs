using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TrainerGUIForms.ServiceReference1;

namespace TrainerGUIForms
{
    public partial class Form2 : Form
    {
        static ServiceReference1.Service1Client client;
        BackgroundWorker worker;
        ANeuralNetwork.ANetwork network;
        AISTask task;
        public Form2()
        {
            InitializeComponent();
            client = new Service1Client();
            worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.WorkerSupportsCancellation = true;
            worker.WorkerReportsProgress = true;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (progressBar1.Value < 100)
                progressBar1.Value = 100;
            //ANeuralNetwork.
            //client.setNetworkResults(task.id,network,network.
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            task = client.getTasks().Where(x => x.id == 3).First();
                //new AISTask();
            //task.id = 6;
            //task.author = "test";
            //task.description = "neurotest";
            //task.name = "ffd";
            //task.ttype = TaskType.ArtificialNeuralNetwork;
            string[] parametrs = new string[] { "10", "1", "8", "10", "0" };
            //client.updateTask(
            //client.addTask(task, parametrs);
            //worker.RunWorkerAsync();
            int end = (int)numericUpDown2.Value;
            double error = (double)numericUpDown1.Value;
            int id = task.id;
                //client.getTasks().Last().id;
            network = client.getNeuralNetwork(id);
            double newError = network.study(genInputs(10, 10), genOutputs(10, 10), error, end);
            client.setNetworkResults(id, network, (float)newError);
        }
        public List<List<double>> genInputs(int n, int m)
        {
            List<List<double>> result = new List<List<double>>();
            for (int i = 0; i < n; i++)
            {
                List<double> l = new List<double>();
                for (int j = 0; j < m; j++)
                {
                    l.Add(j % (i + 1) == 0 ? 1.0 : 0.0);
                }
                result.Add(l);
            }
            return result;
        }
        public List<List<double>> genOutputs(int n, int m)
        {
            List<List<double>> result = new List<List<double>>();
            for (int i = 0; i < n; i++)
            {
                List<double> l = new List<double>();
                for (int j = 0; j < m; j++)
                {
                    l.Add(j == i ? 1.0 : 0.0);
                }
                result.Add(l);
            }
            return result;
        }
    }
}
