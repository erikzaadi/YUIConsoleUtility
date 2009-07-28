namespace YUICompressorWinForm
{
    partial class YUICompressorWinFormApp
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.buttonChooseNCompressCSS = new System.Windows.Forms.Button();
            this.buttonChooseNCompressJS = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // buttonChooseNCompressCSS
            // 
            this.buttonChooseNCompressCSS.Location = new System.Drawing.Point(12, 41);
            this.buttonChooseNCompressCSS.Name = "buttonChooseNCompressCSS";
            this.buttonChooseNCompressCSS.Size = new System.Drawing.Size(151, 23);
            this.buttonChooseNCompressCSS.TabIndex = 0;
            this.buttonChooseNCompressCSS.Text = "Choose and Compress CSS";
            this.buttonChooseNCompressCSS.UseVisualStyleBackColor = true;
            this.buttonChooseNCompressCSS.Click += new System.EventHandler(this.buttonChooseNCompressCSS_Click);
            // 
            // buttonChooseNCompressJS
            // 
            this.buttonChooseNCompressJS.Location = new System.Drawing.Point(12, 12);
            this.buttonChooseNCompressJS.Name = "buttonChooseNCompressJS";
            this.buttonChooseNCompressJS.Size = new System.Drawing.Size(151, 23);
            this.buttonChooseNCompressJS.TabIndex = 0;
            this.buttonChooseNCompressJS.Text = "Choose and Compress JS";
            this.buttonChooseNCompressJS.UseVisualStyleBackColor = true;
            this.buttonChooseNCompressJS.Click += new System.EventHandler(this.buttonChooseNCompressJS_Click);
            // 
            // YUICompressorWinFormApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(232, 94);
            this.Controls.Add(this.buttonChooseNCompressJS);
            this.Controls.Add(this.buttonChooseNCompressCSS);
            this.Name = "YUICompressorWinFormApp";
            this.Text = "YUI Compressor";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button buttonChooseNCompressCSS;
        private System.Windows.Forms.Button buttonChooseNCompressJS;
    }
}

