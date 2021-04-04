using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageHandler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void startConversionProcessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ((ToolStripMenuItem)sender).Enabled = false;

                ImageProcessor.ProcessImages(progressBar1, lblMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"There was an error while processing images. Error: {ex.Message}", "Image Handler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                progressBar1.Value = 0;
                lblMessage.Text = string.Empty;
                ((ToolStripMenuItem)sender).Enabled = true;
            }
        }

        private void clearOutputsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ((ToolStripMenuItem)sender).Enabled = false;

                ImageProcessor.CleanupOutputs(progressBar1, lblMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"There was an error while cleaning outputs. Error: {ex.Message}", "Image Handler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                progressBar1.Value = 0;
                lblMessage.Text = string.Empty;
                ((ToolStripMenuItem)sender).Enabled = true;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
