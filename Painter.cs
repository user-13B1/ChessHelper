using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ChessHelper
{
    class Painter
    {
        internal Overlay overlay;
        private Field field;
        internal event NotifyDelegate Notify;
        private Moves moves;
        private Board board;
        public Painter(Field field, Board board, Moves moves)
        {
            this.field = field;
            this.board = board;
            this.moves = moves;
            overlay = new Overlay(field.Width, field.Width);
        }
        internal void Draw()
        {
           while(true)
           {
                Thread.Sleep(20);
                if (moves.IsNewFrame())
                {
                    if (moves.IsPlayerNextMove)
                    { 
                        string[] movesArr = moves.GetFishMove();

                        if(movesArr[0]!=null)
                            DrawBestMove(movesArr[0], "blue");
                        if (movesArr[1] != null)
                            DrawBestMove(movesArr[1], "green");
                    }
                    else
                        overlay.ClearFrame();
                }
           }

        }

        internal void DrawBestMove(string bestMove, string color = "blue")
        {
            int i1 = GetColumn(bestMove[0], board.whitefigure);
            int j1 = GetRow(bestMove[1], board.whitefigure);
            int i2 = GetColumn(bestMove[2], board.whitefigure);
            int j2 = GetRow(bestMove[3], board.whitefigure);

            if (i1 == -1 || i2 == -1 || j1 == -1 || j2 == -1)
            {
                Notify("Error. Wrong position.");
                return;
            }

            switch (color)
            {
                case "blue":
                    overlay.DrawBlueRect(i1 * field.cellWidth, j1 * field.cellWidth, field.cellWidth, field.cellWidth);
                    overlay.DrawBlueRect(i2 * field.cellWidth, j2 * field.cellWidth, field.cellWidth, field.cellWidth);
                    break;

                case "red":
                    overlay.DrawRedRect(i1 * field.cellWidth, j1 * field.cellWidth, field.cellWidth, field.cellWidth);
                    overlay.DrawRedRect(i2 * field.cellWidth, j2 * field.cellWidth, field.cellWidth, field.cellWidth);
                    break;

                case "green":
                    overlay.DrawGreenRect(i1 * field.cellWidth, j1 * field.cellWidth, field.cellWidth, field.cellWidth);
                    overlay.DrawGreenRect(i2 * field.cellWidth, j2 * field.cellWidth, field.cellWidth, field.cellWidth);
                    break;


                default:
                    overlay.DrawBlueRect(i1 * field.cellWidth, j1 * field.cellWidth, field.cellWidth, field.cellWidth);
                    overlay.DrawBlueRect(i2 * field.cellWidth, j2 * field.cellWidth, field.cellWidth, field.cellWidth);
                    break;

            }
            overlay.UpdateFrame();
        }

        internal void OverlayLoad()
        {
            System.Drawing.Point p = board.autoIt.GetPosField();
            p.X += field.offsetX;
            p.Y += field.offsetY;
            overlay.Load(p);
        }

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


    }
}
