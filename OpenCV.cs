using System;
using OpenCvSharp;
using System.Drawing;

namespace ChessHelper
{
    class OpenCV
    {
        private readonly Writer console;

        public OpenCV(Writer console)
        {
            this.console = console;
            console.WriteLine("OpenCV loaded.");
        }


        internal int[][] ScanColor(Field field, System.Drawing.Point p)
        {
            Color RgbColor;
            int[][] colorCells = new int[8][];
            for (int j = 0; j < 8; j++)
                colorCells[j] = new int[8];

            System.Drawing.Size fieldSize = new System.Drawing.Size(field.Width, field.Width);
            using Bitmap fieldImage = new Bitmap(field.Width, field.Width, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            using Graphics fieldGraph = Graphics.FromImage(fieldImage);
            fieldGraph.CopyFromScreen(p.X + field.offsetX, p.Y + field.offsetY, 0, 0, fieldSize);
            using Mat gameScreen = OpenCvSharp.Extensions.BitmapConverter.ToMat(fieldImage);
         
            for (int j = 0; j < 8; j++)
            {
                for (int i = 0; i < 8; i++)
                {
                    var pixel = gameScreen.Get<Vec3b>(j * field.cellWidth+3, i * field.cellWidth+3);
                    RgbColor = Color.FromArgb(pixel.Item2, pixel.Item1, pixel.Item0);
                    string hex = RgbColor.R.ToString("X2") + RgbColor.G.ToString("X2") + RgbColor.B.ToString("X2");
                    Int32 iColor = int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
                    colorCells[j][i] = iColor;
                }
            }

            return colorCells;
        }
    }
}
