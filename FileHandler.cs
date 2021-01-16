using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Memory_Game
{
    public class FileHandler
    {
        public Image[] Images { get; private set; }

        public FileHandler()
        {
            this.Images = this.GetImagesFromFolder(GetFolderPath());
        }

        private Image[] GetImagesFromFolder(string folder)
        {
            if (folder == null) return null;

            List<Image> returnValues = new List<Image>();

            foreach (string file in Directory.GetFiles(folder))
                returnValues.Add(Image.FromFile(file));

            return returnValues.ToArray();
        }

        private string GetFolderPath()
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.Description = @"Select your card set!";
                fbd.ShowNewFolderButton = false;

                DialogResult result = fbd.ShowDialog();

                switch (result)
                {
                    case DialogResult.OK:
                        if (!string.IsNullOrWhiteSpace(fbd.SelectedPath))
                            return fbd.SelectedPath;
                        return this.NoPathSelectedWarning();

                    default:
                        return this.NoPathSelectedWarning();
                }
            }
        }

        private string NoPathSelectedWarning()
        {
            DialogResult result = MessageBox.Show(
                "No/an invalid path was selected!\r\n" +
                "Would you like to retry?",
                "Oops...",
                MessageBoxButtons.RetryCancel,
                MessageBoxIcon.Exclamation);

            switch (result)
            {
                case DialogResult.Retry:
                    return this.GetFolderPath();

                default:
                    return null;
            }
        }
    }
}