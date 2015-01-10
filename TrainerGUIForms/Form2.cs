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
        float server_error;
        int task_id;
        public Form2(int id)
        {
            InitializeComponent();
            client = new Service1Client();
            network = client.getNeuralNetwork(id);
            server_error = client.getError(id);
            task_id = id;
        }
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
            ANeuralNetwork.StudyData[] sd = client.getStudyData(task_id);
            List<List<double>> inputs = new List<List<double>>();
            List<List<double>> outputs = new List<List<double>>();
            foreach (ANeuralNetwork.StudyData s in sd)
            {
                inputs.Add(s.input);
                outputs.Add(s.output);
            }
            double err = network.study(inputs, outputs, Convert.ToDouble(numericUpDown1.Value), Convert.ToInt32(numericUpDown2.Value));
            client.setNetworkResults(task_id, network, (float)err);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AddStudyDataForm frm = new AddStudyDataForm(task_id);
            frm.Show();
        }
      
    }
}
