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
    public partial class MainForm : Form
    {
        private AISTask[] tasklist;
        ServiceReference1.Service1Client client;
        public MainForm()
        {
            InitializeComponent();
            client = new Service1Client();
        }

        private void обновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            tasklist = client.getTasks();
            foreach (AISTask tt in tasklist)
            {
                dataGridView1.Rows.Add(tt.name, tt.author, tt.ttype == TaskType.GeneticAlgorithm ? "Генетический алгоритм" : "Искуственная нейронная сеть", tt.id, tt.description);
            }
           
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                richTextBox1.Text = (string)dataGridView1.SelectedRows[0].Cells[4].Value;
            }
        }

        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddTaskForm frm = new AddTaskForm();
            frm.Show();
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            
            
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            AISTask tt = tasklist.Where(x => x.id == (Int32)dataGridView1.SelectedRows[0].Cells[3].Value).First();
            switch (tt.ttype)
            {
                case TaskType.ArtificialNeuralNetwork:
                    Form2 frm = new Form2(tt.id);
                    frm.Show();
                    break;
                default:
                    break;
            }
        }
    }
}
