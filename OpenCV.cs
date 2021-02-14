using System;
using OpenCvSharp;
using System.Drawing;
using Size = System.Drawing.Size;


namespace ChessHelper
{
    class OpenCV
    {
        public OpenCV()
        {
        }

        internal int[][] ScanColor(Field field, System.Drawing.Point p)
        {
            int[][] colorCells = new int[8][];
            for (int j = 0; j < 8; j++)
                colorCells[j] = new int[8];
            Size fieldSize = new Size(field.Width, field.Width);
            using Bitmap fieldImage = new Bitmap(field.Width, field.Width, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            using Graphics fieldGraph = Graphics.FromImage(fieldImage);
          
            fieldGraph.CopyFromScreen(p.X + field.offsetX, p.Y + field.offsetY, 0, 0, fieldSize);
            using Mat gameScreen = OpenCvSharp.Extensions.BitmapConverter.ToMat(fieldImage);
            for (int j = 0; j < 8; j++)
                for (int i = 0; i < 8; i++)
                    colorCells[j][i] = GetColorCell(gameScreen, field,i,j);
            
            return colorCells;
        }

        int GetColorCell(Mat gameScreen, Field field,int i,int j)
        {
            var pixel = gameScreen.Get<Vec3b>(j * field.cellWidth + 3, i * field.cellWidth + 3);
            Color RgbColor = Color.FromArgb(pixel.Item2, pixel.Item1, pixel.Item0);
            string hex = RgbColor.R.ToString("X2") + RgbColor.G.ToString("X2") + RgbColor.B.ToString("X2");
            return int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
        }


    }
}
