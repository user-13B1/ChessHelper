using System;
using System.Collections.Generic;
using System.Text;
using Point = OpenCvSharp.Point;
using System.Drawing;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading;


namespace ChessHelper
{
    class ChessBoard
    {
        private Writer console;
        private OpenCV openCV;
        private Autoit autoIt;
        Field field;
        bool whiteGame;
        static object locker = new object();
        

        public ChessBoard(Writer console, OpenCV openCV, Autoit autoIt)
        {
            this.console = console;
            this.openCV = openCV;
            this.autoIt = autoIt;
            field = new Field(75,1,70,40,60,600);   //размеры поля для тренировки
            var col = autoIt.GetPixelColor(field.offsetX + field.cellFigureOffsetX, field.offsetY + field.cellFigureOffsetY);
            if (col == 16316664 || col == 5657426)
            {
                if (col == 16316664)
                    whiteGame = false;
                else
                    whiteGame = true;
            }
            else
                console.WriteLine("Ошибка определения цвета игровых фигур.");
        }

        internal void TestTaskScanColor()
        {
            
            int[][] colorCells = openCV.ScanColor(field, autoIt.GetPosWindow());
            
            for (int j = 0; j < 8; j++)
            {
                for (int i = 0; i < 8; i++)
                {
                   
                    if (colorCells[j][i] == 16250755 || colorCells[j][i] == 12307269)
                    {
                        console.WriteLine(colorCells[j][i]);
                        console.WriteLine(String.Format(GetCharCoord(i, j, whiteGame)));
                    }
                   
                }
            }

        }





        string GetCharCoord(int i,int j, bool white) => (i,white) switch
        {
            (0, true) => "a" + (8 - j),
            (1, true) => "b" + (8 - j),
            (2, true) => "c" + (8 - j),
            (3, true) => "d" + (8 - j),
            (4, true) => "e" + (8 - j),
            (5, true) => "f" + (8 - j),
            (6, true) => "g" + (8 - j),
            (7, true) => "h" + (8 - j),
            (7, false) => "a" + (j + 1),
            (6, false) => "b" + (j + 1),
            (5, false) => "c" + (j + 1),
            (4, false) => "d" + (j + 1),
            (3, false) => "e" + (j + 1),
            (2, false) => "f" + (j + 1),
            (1, false) => "g" + (j + 1),
            (0, false) => "h" + (j + 1)
        };
       
        internal string UpdateMoveHystory()
        {
            int yellowCell = 0;
            string startMovePos = null;
            string endMovePos = null;
            int x, y;


            //int[][] colorCells = new int[8][];
            //for (int j = 0; j < 8; j++)
            //    colorCells[j] = new int[8];


            //for (int j = 0; j < 8; j++)
            //{
            //    for (int i = 0; i < 8; i++)
            //    {
            //        x = i * field.cellWidth + field.offsetX;
            //        y = j * field.cellWidth + field.offsetY;
            //        colorCells[j][i] = autoIt.GetPixelColor(x, y);
            //        if (colorCells[j][i] != 15658706 && colorCells[j][i] != 7771734)
            //            yellowCell++;

            //    }
            //}

            int[][] colorCells = openCV.ScanColor(field, autoIt.GetPosWindow());

            if (yellowCell != 2)
            {
                console.WriteLine($"yellow {yellowCell}");
                return null;
            }

            for (int j = 0; j < 8; j++)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (colorCells[j][i] != 15658706 && colorCells[j][i] != 7771734)
                    {
                        if (colorCells[j][i] == 16250755 || colorCells[j][i] == 12307269)
                        {
                            var colorCellCenter = autoIt.GetPixelColor(i * field.cellWidth + field.offsetX + field.cellFigureOffsetX, j * field.cellWidth + field.offsetY + field.cellFigureOffsetY);
                            if (colorCellCenter == 16316664 || colorCellCenter == 5657426)    //Если стоит фигура на клетке
                            {
                                endMovePos = String.Format(GetCharCoord(i,j,whiteGame));

                            }
                            else
                            {
                                if (startMovePos == null)
                                    startMovePos = String.Format( GetCharCoord(i, j, whiteGame));
                                else
                                {
                                    console.WriteLine("Рокировка.");

                                }
                            }

                        }
                        else
                        {
                            console.WriteLine("Ошибка обновления расстановки фигур");
                            return null;
                        }


                    }
                }
            }

            if (startMovePos == null || endMovePos == null)
                return null;

          
            string lastwMove = startMovePos + endMovePos;
            return lastwMove;
        }
    }

    public enum Figure
    {
        empty,
        Pawn,
        Rook,
        Knight,
        Elephant,
        Queen,
        King,
        pawn,
        rook,
        knight,
        elephant,
        queen,
        king,
    }

}
