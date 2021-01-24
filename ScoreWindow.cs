using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Memory_Game
{
    public partial class ScoreWindow : Form
    {
        private readonly MySqlHandler _mySqlHandler;
        private readonly string _username;
        private readonly decimal _score;

        public ScoreWindow(Bitmap img, MySqlHandler msh, string username, decimal score)
        {
            InitializeComponent();
            ptbImage.BackgroundImage = img;
            this._mySqlHandler = msh;
            this._username = username;
            this._score = score;
        }

        private void btnAbort_Click(object sender, EventArgs e)
            => this.Close();

        private void btnSave_Click(object sender, EventArgs e)
        {
            MySqlCommand cmd = this._mySqlHandler.Prepare(
                SqlStatements.SaveUser,
                ("@score", this._score),
                ("@username", this._username),
                ("@game", ImageHandler.ImageToBytes(ptbImage.BackgroundImage, ImageFormat.Jpeg)));
            this._mySqlHandler.Execute(cmd);
            MessageBox.Show(@"Successfully saved the score!");
            this.Close();
        }
    }
}