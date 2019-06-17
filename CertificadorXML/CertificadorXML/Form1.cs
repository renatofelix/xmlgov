using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace CertificadorXML
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            

            


        }

        private void Button1_Click(object sender, EventArgs e)
        {

        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Label2_Click(object sender, EventArgs e)
        {

        }

        private void Button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Selecione o caminho do arquivo XML";

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tb_destino.Text = fbd.SelectedPath;
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Selecione o caminho do arquivo XML";

            if(fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tb_origem.Text = fbd.SelectedPath;
            }
       
        }
    }
}
