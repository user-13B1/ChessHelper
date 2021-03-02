using System;
using System.Threading;

namespace ChessHelper
{
    internal delegate void NotifyDelegate(string message);
    class Board
    {
        private readonly OpenCV openCV;
        internal readonly Autoit autoIt;
        private Field field;
        internal bool whitefigure;
        private Moves moves;
        internal event NotifyDelegate Notify;

        public Board(Moves moves, Field field)
        {
            this.field = field;
            this.moves = moves;
            openCV = new OpenCV();
            autoIt = new Autoit("LDPlayer-1", Notify);
        }


        internal void Update()
        {
            string lastMove = string.Empty;
            string currMove_1;
            string currMove_2;
            while (true)
            {
                currMove_1 = UpdateMoveHystory();
                Thread.Sleep(18);
                currMove_2 = UpdateMoveHystory();

                if (currMove_1 != currMove_2 || currMove_1 == null)
                    continue;
             
                if(currMove_1 == lastMove)
                    continue;

                lastMove = currMove_1;
                moves.AddMoveToQueue(currMove_1);
            }
        }

        internal void GetMyFigureColor()
        {
            var col = autoIt.GetPixelColor(field.offsetX + field.cellFigureOffsetX, field.offsetY + field.cellFigureOffsetY);
            if (col == 16316664 || col == 5657426)
            {
                if (col == 16316664)
                {
                    Notify("We play Black figure");
                    whitefigure = false;
                    moves.IsPlayerNextMove = false;
                }
                else
                {
                    Notify("We play Whitefigure");
                    whitefigure = true;
                    moves.IsPlayerNextMove = true;
                }
            }
            else
                Notify("Error update figure color.");
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
            (0, false) => "h" + (j + 1),
             _ => throw new NotImplementedException()
        };

        internal string UpdateMoveHystory()
        {
            int errorCellColor = 0;
            string startMovePos = null;
            string endMovePos = null;
            int[][] colorCells = openCV.ScanColor(field, autoIt.GetPosWindow());
           
            for (int j = 0; j < 8; j++)
                for (int i = 0; i < 8; i++)
                    if (colorCells[j][i] != 15658706 && colorCells[j][i] != 7771734)
                        errorCellColor++;

            if (errorCellColor != 2)
                return null;
           
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
                                endMovePos = String.Format(GetCharCoord(i, j, whitefigure));
                            else
                            {
                                if (startMovePos == null)
                                {
                                    startMovePos = String.Format(GetCharCoord(i, j, whitefigure));
                                }
                                else                                                //Если две пустых желтых клетки
                                {
                                    if (IsCastling(colorCells, out string s))       //Проверка рокировки
                                        return s;
                                }
                            }
                        }
                        else
                            return null;
                    }
                }
            }

            if (startMovePos == null || endMovePos == null)
                return null;

            string lastwMove = startMovePos + endMovePos;
            return lastwMove;
        }

        private bool IsCastling(int[][] colorCells, out string s)
        {
            //green 12307269
            //yellow  16250755
            s = null;
            //Игра черными
            if (colorCells[0][0] == 16250755 && colorCells[0][3] == 12307269)
            {
                s = "e1g1";
                return true;
            }

            if (colorCells[0][7] == 12307269 && colorCells[0][3] == 12307269)
            {
                s = "e1c1";
                return true;
            }

            if (colorCells[7][0] == 12307269 && colorCells[7][3] == 16250755)
            {
                s = "e8g8";
                return true;
            }

            if (colorCells[7][7] == 16250755 && colorCells[7][3] == 16250755)
            {
                s = "e8c8";
                return true;
            }

            //игра белыми
            if (colorCells[0][0] == 16250755 && colorCells[0][4] == 16250755)
            {
                s = "e8c8";
                return true;
            }

            if (colorCells[0][4] == 16250755 && colorCells[0][7] == 12307269)
            {
                s = "e8g8";
                return true;
            }

            if (colorCells[7][4] == 12307269 && colorCells[7][0] == 12307269)
            {
                s = "e1c1";
                return true;
            }

            if (colorCells[7][4] == 12307269 && colorCells[7][7] == 16250755)
            {
                s = "e1g1";
                return true;
            }

           
            return false;
        }

    }

}
