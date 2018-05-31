namespace Lempel_Ziv
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
            this.Compactar = new System.Windows.Forms.Button();
            this.Descompactar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Compactar
            // 
            this.Compactar.Location = new System.Drawing.Point(13, 13);
            this.Compactar.Name = "Compactar";
            this.Compactar.Size = new System.Drawing.Size(125, 23);
            this.Compactar.TabIndex = 0;
            this.Compactar.Text = "Compactar...";
            this.Compactar.UseVisualStyleBackColor = true;
            this.Compactar.Click += new System.EventHandler(this.Compactar_Click);
            // 
            // Descompactar
            // 
            this.Descompactar.Location = new System.Drawing.Point(13, 78);
            this.Descompactar.Name = "Descompactar";
            this.Descompactar.Size = new System.Drawing.Size(125, 23);
            this.Descompactar.TabIndex = 1;
            this.Descompactar.Text = "Descompactar...";
            this.Descompactar.UseVisualStyleBackColor = true;
            this.Descompactar.Click += new System.EventHandler(this.Descompactar_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(150, 113);
            this.Controls.Add(this.Descompactar);
            this.Controls.Add(this.Compactar);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button Compactar;
        private System.Windows.Forms.Button Descompactar;
    }
}

