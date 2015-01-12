using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TrainerGUIForms.ServiceReference1;
using System.IO;
namespace TrainerGUIForms
{
    public partial class AddTaskForm : Form
    {
        ServiceReference1.Service1Client client;
        public AddTaskForm()
        {
            InitializeComponent();
            client = new Service1Client();
            
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void AddTaskForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            AISTask tt = new AISTask();
            tt.name = textBox3.Text;
            tt.author = textBox1.Text;
            tt.description = richTextBox1.Text;
            //string[] parameters = textBox2.Text.Split(";".ToCharArray());
            string[] parameters = {};
            tt.ttype = comboBox1.SelectedIndex == 0 ? TaskType.GeneticAlgorithm : TaskType.ArtificialNeuralNetwork;
            if (tt.ttype == TaskType.ArtificialNeuralNetwork)
            {
                List<string> p = new List<string>();
                p.Add(Convert.ToInt32(numericUpDown1.Value).ToString());
                p.Add(Convert.ToInt32(numericUpDown3.Value).ToString());
                p.Add(Convert.ToInt32(numericUpDown2.Value).ToString());
                p.Add(Convert.ToInt32(numericUpDown4.Value).ToString());
                p.Add(comboBox2.SelectedIndex.ToString());
                parameters = p.ToArray();
                string[] allinpts = File.ReadAllLines(textBox4.Text);
                string[] alloutputs = File.ReadAllLines(textBox5.Text);
                tt.current_error = 1.0f/Convert.ToSingle(numericUpDown4.Value);
            }
            int id = client.addTask(tt, parameters);
            if (tt.ttype == TaskType.ArtificialNeuralNetwork)
            {
                string[] allinpts = File.ReadAllLines(textBox4.Text);
                string[] alloutputs = File.ReadAllLines(textBox5.Text);
                List<ANeuralNetwork.StudyData> stdy = genStudyDtata(allinpts, alloutputs);
                client.addStudyData(id,stdy.ToArray());
            }
            this.Hide();
        }
        private List<ANeuralNetwork.StudyData> genStudyDtata(string[] inpts, string[] outputs)
        {
            List<ANeuralNetwork.StudyData> result = new List<ANeuralNetwork.StudyData>();
            int i =0;
            foreach(string str in  inpts){
               ANeuralNetwork.StudyData sd = new ANeuralNetwork.StudyData();
                foreach(string num in str.Split(";".ToCharArray())){
                    sd.input.Add(Convert.ToDouble(num));
                }
                foreach (string num in outputs[i].Split(";".ToCharArray()))
                {
                    sd.output.Add(Convert.ToDouble(num));
                }
                i++;
                result.Add(sd);
            }
            return result;
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 1)
            {
                groupBox1.Enabled = true;
            }
            else
            {
                groupBox1.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox4.Text = openFileDialog1.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox5.Text = openFileDialog1.FileName;
            }
        }
    }
}
