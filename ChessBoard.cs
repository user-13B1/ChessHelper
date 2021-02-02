using System;

namespace ChessHelper
{
    class ChessBoard
    {
        private readonly Writer console;
        private readonly OpenCV openCV;
        private readonly Autoit autoIt;
        readonly Field field;
        internal bool whitefigure;
        internal Overlay overlay;

        public ChessBoard(Writer console, OpenCV openCV, Autoit autoIt)
        {
            this.console = console;
            this.openCV = openCV;
            this.autoIt = autoIt;
            //field = new Field(75,1,70,40,60,600);   //    размеры поля для тренировки
            field = new Field(66,1,142,34,54,528);    //   online
            
            overlay = new Overlay(field.Width, field.Width);
        }

        internal void GetMyFigureColor()
        {
            var col = autoIt.GetPixelColor(field.offsetX + field.cellFigureOffsetX, field.offsetY + field.cellFigureOffsetY);
            if (col == 16316664 || col == 5657426)
            {
                if (col == 16316664)
                {
                    whitefigure = false;
                    console.WriteLine("Игра за черных.");
                }
                else
                {
                    console.WriteLine("Игра за белых.");
                    whitefigure = true;
                }
            }
            else
                console.WriteLine("Ошибка определения цвета игровых фигур.");
        }

        internal void OverlayLoad()
        {
            System.Drawing.Point p = autoIt.GetPosField();
            p.X += field.offsetX;
            p.Y += field.offsetY;
            overlay.Load(p);
        }

        internal void DrawBestMove(string bestMove,string color = "blue")
        {
            int i1 = GetColumn(bestMove[0], whitefigure);
            int j1 = GetRow(bestMove[1], whitefigure);
            int i2 = GetColumn(bestMove[2], whitefigure);
            int j2 = GetRow(bestMove[3], whitefigure);

            if (i1 == -1|| i2 == -1 || j1 == -1 || j2 == -1)
            {
                console.WriteLine("Ошибка координат доски");
                return;
            }

            overlay.ClearElements();
            switch (color)
            {
                case "blue":
                    overlay.DrawRect(i1 * field.cellWidth, j1 * field.cellWidth, field.cellWidth, field.cellWidth);
                    overlay.DrawRect(i2 * field.cellWidth, j2 * field.cellWidth, field.cellWidth, field.cellWidth);
                    break;

                case "red":
                    overlay.DrawRedRect(i1 * field.cellWidth, j1 * field.cellWidth, field.cellWidth, field.cellWidth);
                    overlay.DrawRedRect(i2 * field.cellWidth, j2 * field.cellWidth, field.cellWidth, field.cellWidth);
                    break;

                default:
                    overlay.DrawRect(i1 * field.cellWidth, j1 * field.cellWidth, field.cellWidth, field.cellWidth);
                    overlay.DrawRect(i2 * field.cellWidth, j2 * field.cellWidth, field.cellWidth, field.cellWidth);
                    break;

            }
            overlay.UpdateFrame();
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

        int GetColumn(char c, bool white) => (c, white) switch
        {
            ('a', true) => 0,
            ('b', true) => 1,
            ('c', true) => 2,
            ('d', true) => 3,
            ('e', true) => 4,
            ('f', true) => 5,
            ('g', true) => 6,
            ('h', true) => 7,

            ('a', false) => 7,
            ('b', false) => 6,
            ('c', false) => 5,
            ('d', false) => 4,
            ('e', false) => 3,
            ('f', false) => 2,
            ('g', false) => 1,
            ('h', false) => 0,
            _ => -1
        };

        int GetRow(char c, bool white) => (c, white) switch
        {
            ('1', true) => 7,
            ('2', true) => 6,
            ('3', true) => 5,
            ('4', true) => 4,
            ('5', true) => 3,
            ('6', true) => 2,
            ('7', true) => 1,
            ('8', true) => 0,

            ('1', false) => 0,
            ('2', false) => 1,
            ('3', false) => 2,
            ('4', false) => 3,
            ('5', false) => 4,
            ('6', false) => 5,
            ('7', false) => 6,
            ('8', false) => 7,
            _ => -1
        };

        internal string UpdateMoveHystory()
        {
            int errorCellColor = 0;
            string startMovePos = null;
            string endMovePos = null;
            int[][] colorCells = openCV.ScanColor(field, autoIt.GetPosWindow());

            for (int j = 0; j < 8; j++)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (colorCells[j][i] != 15658706 && colorCells[j][i] != 7771734)
                        errorCellColor++;
                }
            }

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
                            {
                                endMovePos = String.Format(GetCharCoord(i, j, whitefigure));
                            }
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
