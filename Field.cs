using System;
using System.Collections.Generic;
using System.Text;

namespace ChessHelper
{
    class Field
    {
        public int cellWidth;
        public int offsetX;
        public int offsetY;
        public int cellFigureOffsetX;
        public int cellFigureOffsetY;
        public int Width;

        public Field(int cellWidth, int offsetX, int offsetY, int cellFigureOffsetX, int cellFigureOffsetY, int Width)
        {
            this.cellWidth = cellWidth;
            this.offsetX = offsetX;
            this.offsetY = offsetY;
            this.cellFigureOffsetX = cellFigureOffsetX;
            this.cellFigureOffsetY = cellFigureOffsetY;
            this.Width = Width;
        }

    }
}
