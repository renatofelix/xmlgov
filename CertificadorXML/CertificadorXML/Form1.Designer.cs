namespace CertificadorXML
{
    partial class Form1
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_origem = new System.Windows.Forms.TextBox();
            this.tb_destino = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(263, 168);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Certificar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(53, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Origem do Arquivo";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(53, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Destino do Arquivo ";
            this.label2.Click += new System.EventHandler(this.Label2_Click);
            // 
            // tb_origem
            // 
            this.tb_origem.Location = new System.Drawing.Point(56, 50);
            this.tb_origem.Name = "tb_origem";
            this.tb_origem.Size = new System.Drawing.Size(282, 20);
            this.tb_origem.TabIndex = 3;
            this.tb_origem.TextChanged += new System.EventHandler(this.TextBox1_TextChanged);
            // 
            // tb_destino
            // 
            this.tb_destino.Location = new System.Drawing.Point(56, 129);
            this.tb_destino.Name = "tb_destino";
            this.tb_destino.Size = new System.Drawing.Size(282, 20);
            this.tb_destino.TabIndex = 4;
            this.tb_destino.TextChanged += new System.EventHandler(this.TextBox2_TextChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(350, 49);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(69, 20);
            this.button2.TabIndex = 5;
            this.button2.Text = "Selecionar";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(350, 129);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(69, 20);
            this.button3.TabIndex = 6;
            this.button3.Text = "Selecionar";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Button3_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 230);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.tb_destino);
            this.Controls.Add(this.tb_origem);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "CertificadorXML";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_origem;
        private System.Windows.Forms.TextBox tb_destino;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
    }
}

