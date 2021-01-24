using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Memory_Game
{
    public partial class ScoreWindow : Form
    {
        public ScoreWindow(Bitmap img)
        {
            InitializeComponent();
            InitializePage(img);
        }

        public void InitializePage(Bitmap img)
        {
            ptbImage.BackgroundImage = img;
            ptbImage.BackColor = Color.Red;
        }
    }
}
