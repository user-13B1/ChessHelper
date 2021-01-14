using System;
using System.Collections.Generic;
using System.Text;
using Point = OpenCvSharp.Point;
using System.Drawing;
using System.Diagnostics;

namespace ChessHelper
{
    class ChessBoard
    {
        private Writer console;
        private OpenCV openCV;
        private Autoit autoIt;
        internal SquareField[][] squares;
        int figureCounter;
        Field field;
        bool whiteGame;


        public ChessBoard(Writer console, OpenCV openCV, Autoit autoIt)
        {
            this.console = console;
            this.openCV = openCV;
            this.autoIt = autoIt;
            field = new Field(75,1,70,40,60);   //размеры поля для тренировки

            squares = new SquareField[8][];
            for (int j = 0; j < 8; j++)
            {
                squares[j] = new SquareField[8];
                for (int i = 0; i < 8; i++)
                {
                    string name = GetCharCoord(i, whiteGame);
                    name = name + (j + 1).ToString();
                    squares[j][i] = new SquareField(name);
                }
            }
        }

        string GetCharCoord(int i,bool white) => (i,white) switch
        {
            (0, true) => "h",
            (1, true) => "g",
            (2, true) => "f",
            (3, true) => "e",
            (4, true) => "d",
            (5, true) => "c",
            (6, true) => "b",
            (7, true) => "a",
            (7, false) => "h",
            (6, false) => "g",
            (5, false) => "f",
            (4, false) => "e",
            (3, false) => "d",
            (2, false) => "c",
            (1, false) => "b",
            (0, false) => "a"
        };

        internal void UpdateStartPos(System.Drawing.Point p, Overlay overlay)
        {
            openCV.GetFieldsImage(p.X ,p.Y, squares);
            for (int j = 0; j < 8; j++)
            {
                for (int i = 0; i < 8; i++)
                {
                    squares[j][i].oldFigure = squares[j][i].currFigure = openCV.FigureDefinition(squares[j][i].image);
                    if (squares[j][i].currFigure != Figure.empty)
                    {
                        figureCounter++;
                        string s = squares[j][i].currFigure.ToString();
                        overlay.DrawText($"{squares[j][i].name} {s}", i*66, j*66);
                    }
                }
            }
            console.WriteLine($"Figure count {figureCounter}");
            overlay.UpdateFrame();
        }

        internal string UpdateMove(System.Drawing.Point p, Overlay overlay)
        {
            string moveStart = null;
            string moveEnd = null;
            overlay.ClearElements();
            overlay.UpdateFrame();
            openCV.GetFieldsImage(p.X, p.Y, squares);
            for (int j = 0; j < 8; j++)
            {
                for (int i = 0; i < 8; i++)
                {
                    squares[j][i].currFigure = openCV.FigureDefinition(squares[j][i].image);
                    
                    if(squares[j][i].currFigure!= squares[j][i].oldFigure)
                    {
                        if (squares[j][i].oldFigure == Figure.empty)
                        {
                            moveEnd = squares[j][i].name;
                        }
                        else
                        {
                            moveEnd = squares[j][i].name;
                            console.WriteLine("Figure hit.");
                        }

                        if (squares[j][i].currFigure == Figure.empty)
                        {
                            moveStart = squares[j][i].name;
                        }

                        squares[j][i].oldFigure = squares[j][i].currFigure;
                    }


                }
            }

            return moveStart + moveEnd;
        }
       
        internal string UpdateMoveHystory()
        {

            int yellowCell = 0;
            string startMovePos = null;
            string endMovePos = null;
           
            int[][] colorCells = new int[8][];
            for (int j = 0; j < 8; j++)
                colorCells[j] = new int[8];

            for (int j = 0; j < 8; j++)
                for (int i = 0; i < 8; i++)
                {
                    colorCells[j][i] = autoIt.GetPixelColor(i * field.cellLenght + field.offsetX, j * field.cellLenght + field.offsetY);
                    if (colorCells[j][i] != 15658706 && colorCells[j][i] != 7771734)
                    {
                        yellowCell++;
                    }
                }

            if(yellowCell != 2)
            {
                console.WriteLine(yellowCell);
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
                            var colorCellCenter = autoIt.GetPixelColor(i * field.cellLenght + field.offsetX + field.cellFigureOffsetX, j * field.cellLenght + field.offsetY + field.cellFigureOffsetY);
                            if (colorCellCenter == 16316664 || colorCellCenter == 5657426)    //Если стоит фигура на клетке
                            {
                                endMovePos = String.Format("{0}{1}", GetCharCoord(i), j + 1);

                            }
                            else
                            {
                                if (startMovePos == null)
                                    startMovePos = String.Format("{0}{1}", GetCharCoord(i), j + 1);
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

    class SquareField
    {
        public Bitmap image;
        public Figure currFigure;
        public Figure oldFigure;
        public string name;

        public SquareField(string name)
        {
            this.name = name;
        }
    }

    class Field
    {
        public int cellLenght;
        public int offsetX;
        public int offsetY;
        public int cellFigureOffsetX;
        public int cellFigureOffsetY;

        public Field(int cellLenght, int offsetX, int offsetY, int cellFigureOffsetX, int cellFigureOffsetY)
        {
            this.cellLenght = cellLenght;
            this.offsetX = offsetX;
            this.offsetY = offsetY;
            this.cellFigureOffsetX = cellFigureOffsetX;
            this.cellFigureOffsetY = cellFigureOffsetY;
        }
    }
}
