using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snowfall_Gavrilova
{
    public partial class SnowfallAnimationForm : Form
    {
        private Timer timer;
        private List<PictureBox> snowflakes;
        private Random rnd = new Random();

        private const int MAX_SNOWFLAKES = 35;
        private const int MIN_SNOWFLAKES = 10;
        private static readonly int[] SNOWFLAKE_SIZES = { 10, 15, 25, 40 };
        private const int SPEED = 10; // Скорость движения

        public SnowfallAnimationForm()
        {
            InitializeComponent();
            InitializeSnowFlakes();
        }

        private void InitializeSnowFlakes()
        {
            this.BackColor = Color.Black;
            this.TopMost = true;

            snowflakes = new List<PictureBox>();
            timer = new Timer();
            timer.Interval = 40;
            timer.Tick += new EventHandler(SnowflakeMove);
            timer.Start();

            for (var i = 0; i < MAX_SNOWFLAKES; i++)
            {
                CreateSnowflake();
            }
        }

        private void CreateSnowflake()
        {
            PictureBox snowflake = new PictureBox();
            snowflake.Image = global::Snowfall_Gavrilova.Properties.Resources.snowflake;

            // Выбираем случайный размер из нашего массива
            int size = SNOWFLAKE_SIZES[rnd.Next(SNOWFLAKE_SIZES.Length)];

            // Масштабируем изображение до соответствующего размера
            snowflake.Image = ResizeImage(global::Snowfall_Gavrilova.Properties.Resources.snowflake, size);

            // Генерируем случайную позицию внутри границ формы
            int x = rnd.Next(0, this.ClientRectangle.Width - size);
            int y = rnd.Next(0, this.ClientRectangle.Height - size);
            snowflake.Location = new Point(x, y);

            snowflake.BackColor = Color.Transparent;
            snowflake.SizeMode = PictureBoxSizeMode.AutoSize;

            this.Controls.Add(snowflake);
            snowflakes.Add(snowflake);

            MoveSnowflakeDown(snowflake);
        }

        private Bitmap ResizeImage(Image image, int newSize)
        {
            var bitmap = new Bitmap(newSize, newSize);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.DrawImage(image, 0, 0, newSize, newSize);
            }
            return bitmap;
        }

        private void SnowflakeMove(object sender, EventArgs e)
        {
            foreach (PictureBox snowflake in snowflakes)
            {
                MoveSnowflakeDown(snowflake);
            }
        }

        private void MoveSnowflakeDown(PictureBox snowflake)
        {
            Point location = snowflake.Location;
            location.Y += SPEED;

            if (location.Y > this.ClientRectangle.Height)
            {
                location.Y = -snowflake.Height;
                location.X = rnd.Next(this.ClientRectangle.Width - snowflake.Width);
            }

            if (location.Y < 0)
            {
                location.Y = 0;
            }

            snowflake.Location = location;
        }
    }
}
