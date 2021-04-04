using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageHandler
{
    public class ImageProcessor
    {
        public static async void ProcessImages(ProgressBar progressBar, Label lblMessage)
        {
            try
            {
                await Task.Run(() =>
                {
                    CleanupOutputs(progressBar, lblMessage);

                    lblMessage.BeginInvoke((MethodInvoker)delegate () { lblMessage.Text = "Processing images..."; });
                    lblMessage.BeginInvoke((MethodInvoker)delegate () { lblMessage.Update(); });
                    
                    var outputDirectories = new string[] { "1024|20", "1440|30", "2560|45", "320|8", "375|10", "425|10", "768|15" };
                    var fileEntries = Directory.GetFiles("../../images/input/");

                    var totalFiles = outputDirectories.Length * fileEntries.Length;

                    progressBar.BeginInvoke((MethodInvoker)delegate () { progressBar.Maximum = totalFiles; });

                    var index = 0;

                    foreach (var item in outputDirectories)
                    {
                        var info = item.Split(new string[] { "|" }, System.StringSplitOptions.RemoveEmptyEntries);

                        var outputFolder = info[0];
                        var percentage = int.Parse(info[1]);

                        foreach (var file in fileEntries)
                        {
                            progressBar.BeginInvoke((MethodInvoker)delegate () { progressBar.Value = index; });
                            index++;

                            ResizeImage(file, $"../../images/output/{outputFolder}", percentage);
                        }
                    }

                    
                    progressBar.BeginInvoke((MethodInvoker)delegate () { progressBar.Value = 0; });
                    lblMessage.BeginInvoke((MethodInvoker)delegate () { lblMessage.Text = string.Empty; });

                    MessageBox.Show("Files processed successfully", "Image Handler", MessageBoxButtons.OK, MessageBoxIcon.Information);
                });
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public static void CleanupOutputs(ProgressBar progressBar, Label lblMessage)
        {
            try
            {
                lblMessage.BeginInvoke((MethodInvoker)delegate () { lblMessage.Text = "Cleaning up outputs..."; });
                lblMessage.BeginInvoke((MethodInvoker)delegate () { lblMessage.Update(); });

                Thread.Sleep(1000);

                var outputDirectories = new string[] { "1024|20", "1440|30", "2560|45", "320|8", "375|10", "425|10", "768|15" };
                var index = 0;
                var length = 0;

                foreach (var item in outputDirectories)
                {
                    var info = item.Split(new string[] { "|" }, System.StringSplitOptions.RemoveEmptyEntries);
                    var outputFolder = info[0];
                    var directoryInfo = new DirectoryInfo($"../../images/output/{outputFolder}");

                    length += directoryInfo.GetFiles().Length;
                }

                progressBar.BeginInvoke((MethodInvoker)delegate () { progressBar.Maximum = length; });

                foreach (var item in outputDirectories)
                {
                    progressBar.BeginInvoke((MethodInvoker)delegate () { progressBar.Value = index; });

                    var info = item.Split(new string[] { "|" }, System.StringSplitOptions.RemoveEmptyEntries);
                    var outputFolder = info[0];
                    var directoryInfo = new DirectoryInfo($"../../images/output/{outputFolder}");

                    foreach (var file in directoryInfo.GetFiles())
                    {
                        file.Delete();

                        index++;
                        progressBar.BeginInvoke((MethodInvoker)delegate () { progressBar.Value = index; });

                    }
                }

                progressBar.BeginInvoke((MethodInvoker)delegate () { progressBar.Value = 0; });
                lblMessage.BeginInvoke((MethodInvoker)delegate () { lblMessage.Text = string.Empty; });
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }


        private static void ResizeImage(string inputPath, string outputPath, int percentage)
        {
            try
            {
                var split = inputPath.Split(new string[] { "/" }, System.StringSplitOptions.RemoveEmptyEntries);
                var fileName = split[split.Length - 1];
                var fileInfo = fileName.Split(new string[] { "." }, System.StringSplitOptions.RemoveEmptyEntries);

                fileName = fileInfo[0];
                var fileExtension = fileInfo[1];
                var imageFormat = ImageFormat.Png;

                switch (fileExtension)
                {
                    case "Png":
                        imageFormat = ImageFormat.Png;
                        break;
                    case "Bmp":
                        imageFormat = ImageFormat.Bmp;
                        break;
                    case "Gif":
                        imageFormat = ImageFormat.Gif;
                        break;
                    case "Jpeg":
                    case "Jpg":
                        imageFormat = ImageFormat.Jpeg;
                        break;
                    default:
                        imageFormat = ImageFormat.Png;
                        break;
                }


                var img = Image.FromFile(inputPath);

                int originalW = img.Width;
                int originalH = img.Height;

                int resizedW = (int)(originalW * percentage / 100);
                int resizedH = (int)(originalH * percentage / 100);

                Bitmap bmp = new Bitmap(resizedW, resizedH);
                Graphics graphic = Graphics.FromImage((Image)bmp);
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic.DrawImage(img, 0, 0, resizedW, resizedH);

                bmp.Save(outputPath + "/" + fileName + "." + fileExtension, imageFormat);

                bmp.Dispose();
                graphic.Dispose();
                img.Dispose();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
