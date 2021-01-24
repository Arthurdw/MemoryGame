using System.Drawing;
using System.Windows.Forms;

namespace Memory_Game
{
    public partial class ImagePreview : Form
    {
        public ImagePreview(Image img)
        {
            InitializeComponent();
            ptbPreview.Image = img;
        }
    }
}