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
            string[] parameters = textBox2.Text.Split(";".ToCharArray());
            tt.ttype = comboBox1.SelectedIndex == 0 ? TaskType.GeneticAlgorithm : TaskType.ArtificialNeuralNetwork;
            client.addTask(tt, parameters);
            this.Hide();
        }
    }
}
