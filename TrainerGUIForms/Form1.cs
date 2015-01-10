using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace TrainerGUIForms
{
    public partial class Form1 : Form
    {
        ServiceReference1.Service1Client service;
        ServiceReference1.AISTask task;
        BackgroundWorker worker;
        AlgoLib.IGenetical algorithm;
        int genCount;
        System.Threading.Thread thread;
        public Form1()
        {
            InitializeComponent();
            service = new ServiceReference1.Service1Client();
            this.comboBox1.DataSource = service.GetAvailableGALibs();
        }
        public Form1(ServiceReference1.Service1Client _service, ServiceReference1.AISTask _task)
        {
            InitializeComponent();
            service = _service;
            task = _task;
            this.comboBox1.DataSource = service.GetAvailableGALibs();
            worker = new BackgroundWorker();
            worker.WorkerSupportsCancellation = true;
            worker.WorkerReportsProgress = true;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            richTextBox1.Text += "Отправляем данные на сервер";
            System.Threading.Thread.Sleep(1000);
            thread = new System.Threading.Thread(SendData);
            thread.Start();
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < genCount; i++)
            {
                int percents = (i * 100) / genCount;
                algorithm.NextGeneration();
                worker.ReportProgress(percents);
            }
        }
        private void SendData()
        {
            service.setGeneticAlgorithm(task.id, GenXmlSerialization.BinSerialization.OgjectToByte(algorithm), (float)
0.01);
            this.Invoke((MethodInvoker)delegate
            {
                richTextBox1.Text += "Данные отправлены";
            });
            
        }
        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!File.Exists(comboBox1.SelectedItem.ToString()))
            {
                File.WriteAllBytes(comboBox1.SelectedItem.ToString(), service.GetAssembly(comboBox1.SelectedItem.ToString()));
            }
            genCount = (int)numericUpDown1.Value;
            algorithm = GenXmlSerialization.BinSerialization.ByteToObject(service.getGeneticAlgorithm(task.id)) as AlgoLib.IGenetical;
            worker.RunWorkerAsync();
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (thread != null)
            {
                if (thread.IsAlive)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
