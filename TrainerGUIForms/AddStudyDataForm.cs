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
    public partial class AddStudyDataForm : Form
    {
        Service1Client client;
        public AddStudyDataForm()
        {
            InitializeComponent();
            

        }
        int task_id;
        public AddStudyDataForm(int id)
        {
            InitializeComponent();
            task_id = id;
            client = new Service1Client();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string[] inputs_str = textBox1.Text.Split(";".ToCharArray());
            string[] outputs_str = textBox2.Text.Split(";".ToCharArray());
            ANeuralNetwork.StudyData sd = new ANeuralNetwork.StudyData();
            foreach (string s in inputs_str)
            {
                sd.input.Add(double.Parse(s));
            }
            foreach (string s in outputs_str)
            {
                sd.output.Add(double.Parse(s));
            }
            List<ANeuralNetwork.StudyData> lst = new List<ANeuralNetwork.StudyData>();
            lst.Add(sd);
            client.addStudyData(task_id, lst.ToArray());
            this.Hide();
        }
    }
}
