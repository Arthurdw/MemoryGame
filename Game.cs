using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Memory_Game
{
    public partial class Form1 : Form
    {
        public FileHandler Fh { get; private set; }
        public List<Image> Field { get; private set; }
        public List<(int, int, Image)> Collected { get; private set; }
        public (int, int, Image) Latest { get; private set; }
        public bool AllowClick { get; private set; }
        public DateTime StartedAt { get; private set; }
        public DateTime TimeoutEnd { get; private set; }
        public int CouplesPerImage { get; private set; }
        public int CouplesFound { get; private set; }

        public Form1()
        {
            InitializeComponent();
            InitializeProgram();
        }

        private void InitializeProgram()
        {
            tlpData.BorderStyle = BorderStyle.FixedSingle;
            this.Collected = new List<(int, int, Image)>();
            this.Latest = (0, 0, null);
            CouplesPerImage = 2;
        }

        private void gameconfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Fh = new FileHandler();

            if (this.Fh.Images == null)
            {
                MessageBox.Show("No valid card source got selected!", "Oops...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.Collected.Clear();
            this.InitializeFieldWithCurrentFileHandler();
        }

        private void HandleCardClick(object sender, EventArgs e)
        {
            if (!this.AllowClick) return;

            PictureBox trigger = (PictureBox)sender;

            if (trigger.BackgroundImage != null) return;

            int col = tlpData.GetPositionFromControl(trigger).Column;
            int row = tlpData.GetPositionFromControl(trigger).Row;
            int idx = (row * tlpData.ColumnCount) + col;

            var (hasMiddle, middle) = this.GetMiddle();
            if (hasMiddle)
            {
                if (idx == middle) return;
                if (idx > middle) idx--;
            }

            trigger.BackgroundImage = this.Field[idx];

            if (this.Latest != (0, 0, null))
            {
                var (lCol, lRow, lImage) = this.Latest;
                if (lImage == trigger.BackgroundImage)
                {
                    this.Collected.Add((lCol, lRow, lImage));
                    this.Collected.Add((col, row, trigger.BackgroundImage));
                    lblCount.Text = (++this.CouplesFound).ToString();

                    if (this.Collected.Count == this.Field.Count)
                    {
                        TimePassed.Stop();
                        
                    }
                }
                else this.TimeoutEnd = DateTime.Now.AddSeconds(1.5);
                this.Latest = (0, 0, null);
            }
            else this.Latest = (col, row, trigger.BackgroundImage);
        }

        private void InitializeFieldWithCurrentFileHandler()
        {
            var (columnCount, rowCount) = this.GetColumnsAndRows();
            (tlpData.ColumnCount, tlpData.RowCount) = (columnCount, rowCount);

            foreach (ColumnStyle style in tlpData.ColumnStyles)
            {
                style.SizeType = SizeType.Absolute;
                style.Width = tlpData.Width / columnCount;
            }

            foreach (RowStyle style in tlpData.RowStyles)
            {
                style.SizeType = SizeType.Absolute;
                style.Height = tlpData.Height / rowCount;
            }

            this.Field = new List<Image>(this.Fh.Images.ToList().Count);

            for (int i = 0; i < this.CouplesPerImage; i++)
                this.Field.AddRange(this.Fh.Images.ToList());

            this.Field.Shuffle();
            this.CouplesFound = 0;
            lblCount.Text = 0.ToString();
            this.InitializePlayField(columnCount, rowCount);
            this.AllowClick = true;
            this.StartedAt = DateTime.Now;
            TimePassed.Start();
        }

        private (int, int) GetColumnsAndRows()
        {
            switch (this.Fh.Images.Length)
            {
                case 2:
                    return (2, 3);

                case 3:
                    return (2, 3);

                case 4:
                    return (3, 3);

                case 6:
                    return (3, 4);

                case 7:
                    return (3, 5);

                case 8:
                    return (4, 4);

                case 10:
                    return (5, 4);

                default:
                    return (5, 5);
            }
        }

        private void InitializePlayField(int columnCount, int rowCount)
        {
            var (hasMiddle, _) = this.GetMiddle();

            tlpData.Controls.Clear();
            for (int i = 0; i < (this.Field.Count + (hasMiddle ? 1 : 0)); i++)
            {

                PictureBox ptb = new PictureBox
                {
                    Dock = DockStyle.Fill,
                    BackgroundImageLayout = ImageLayout.Stretch,
                    BackColor = Color.FromArgb(128, 0, 0, 0),
                    Width = tlpData.Width / columnCount,
                    Height = tlpData.Height / rowCount
                };

                ptb.Click += this.HandleCardClick;
                tlpData.Controls.Add(ptb);
            }

            foreach (var (col, row, image) in this.Collected)
            {
                Control ctrl = tlpData.GetControlFromPosition(col, row);
                ctrl.BackgroundImage = image;
                tlpData.SetRow(ctrl, row);
            }
        }


        /// <summary>
        ///     Check if the current field contains a disabled middle cell.
        /// </summary>
        ///
        /// <returns>
        ///     If there is a disabled middle cell and if there is one its index.
        /// </returns>
        ///
        /// <example>
        /// <code>
        ///     >>> GetMiddle();
        ///     (true, 5)
        ///     >>> GetMiddle();
        ///     (false, -1)
        /// </code>
        /// </example>
        private (bool, int) GetMiddle()
        {
            int middle = -1;
            bool hasMiddle = false;
            float count = (tlpData.ColumnCount * tlpData.RowCount) / 2f;

            if (count % 1 != 0)
            {
                hasMiddle = true;
                middle = (int)Math.Floor(count);
            }

            return (hasMiddle, middle);
        }

        private void TimePassed_Tick(object sender, EventArgs e)
        {
            lblSec.Text = $@"{(DateTime.Now - this.StartedAt).Seconds} sec";
            if (!this.AllowClick && this.TimeoutEnd < DateTime.Now)
            {
                var (col, row) = this.GetColumnsAndRows();
                this.InitializePlayField(col, row);
            }

            this.AllowClick = this.TimeoutEnd < DateTime.Now;
        }
    }
}