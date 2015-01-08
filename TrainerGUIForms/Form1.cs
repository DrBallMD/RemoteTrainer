using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace TrainerGUIForms
{
    public partial class Form1 : Form
    {
        ServiceReference1.Service1Client service;
        public Form1()
        {
            InitializeComponent();
            service = new ServiceReference1.Service1Client();
            this.comboBox1.DataSource = service.GetAvailableGALibs();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
