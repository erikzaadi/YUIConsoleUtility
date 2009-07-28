using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace YUICompressorWinForm
{
    public partial class YUICompressorWinFormApp : Form
    {
        public YUICompressorWinFormApp()
        {
            InitializeComponent();
        }

        private void buttonChooseNCompressJS_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Choose JavaScript File..";
            openFileDialog1.Filter = "Javascript Files (*.js)|*.js";

            if (openFileDialog1.ShowDialog(this) != DialogResult.OK)
                return;
            if (!File.Exists(openFileDialog1.FileName))
            {
                MessageBox.Show(this, "File not found!", Text);
                return;
            }
            File.WriteAllText(openFileDialog1.FileName,
                Yahoo.Yui.Compressor.JavaScriptCompressor.Compress(
                    File.ReadAllText(openFileDialog1.FileName),
                    false,
                    false,
                    true,
                    false,
                    80));
        }

        private void buttonChooseNCompressCSS_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Choose CSS File..";
            openFileDialog1.Filter = "CSS Files (*.css)|*.css";

            if (openFileDialog1.ShowDialog(this) != DialogResult.OK)
                return;
            if (!File.Exists(openFileDialog1.FileName))
            {
                MessageBox.Show(this, "File not found!", Text);
                return;
            }
            File.WriteAllText(openFileDialog1.FileName,
                Yahoo.Yui.Compressor.CssCompressor.Compress(
                    File.ReadAllText(openFileDialog1.FileName),
                    80,
                     Yahoo.Yui.Compressor.CssCompressionType.StockYuiCompressor));
        }
    }
}
