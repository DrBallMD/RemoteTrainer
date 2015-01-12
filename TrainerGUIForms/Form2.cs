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
        ANeuralNetwork.ANetwork network;
        float server_error;
        int task_id;
        System.Threading.Thread task;
        public Form2(int id)
        {
            InitializeComponent();
            client = new Service1Client();
            network = client.getNeuralNetwork(id);
            server_error = client.getError(id);
            numericUpDown1.Value = Convert.ToDecimal(server_error);
            task_id = id;
        }
        public Form2()
        {
            InitializeComponent();
            client = new Service1Client();
        }
        private void work()
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
        private void button1_Click(object sender, EventArgs e)
        {
            task = new System.Threading.Thread(work);
            task.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AddStudyDataForm frm = new AddStudyDataForm(task_id);
            frm.Show();
        }

        private void Form2_Deactivate(object sender, EventArgs e)
        {
            
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (task != null)
            {
                if (task.IsAlive)
                {
                    MessageBox.Show("Идет обучение!");
                    e.Cancel = true;
                }
            }
        }
      
    }
}
