using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ChessHelper
{
    class ChessBot
    {
        internal readonly Autoit autoIt;
        internal readonly Writer console;
        internal readonly Overlay overlay;
        internal readonly OpenCV openCV;
        internal ChessBoard chessBoard;
        private List<string> movesHystory;

        public ChessBot(Writer console)
        {
            this.console = console;
            autoIt = new Autoit(console, "LDPlayer-1");
            overlay = new Overlay(autoIt);
            openCV = new OpenCV(console, autoIt, overlay);
            chessBoard = new ChessBoard(console,openCV, autoIt);
            movesHystory = new List<string>();
        }

    
        internal void StartWork()
        {
            overlay.Load();
            Task.Run(()=>UpdateBoard());

        }

        private void UpdateBoard()
        {
            string currMove;
            while (true)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                currMove = chessBoard.UpdateMoveHystory();
              
                if (movesHystory.Count == 0 && currMove != null)         //First move
                {
                    console.WriteLine($"First move {currMove}");
                    movesHystory.Add(currMove);
                }

                if (currMove != null && currMove != movesHystory[movesHystory.Count - 1] && movesHystory.Count != 0)
                {
                    movesHystory.Add(currMove);
                    console.WriteLine($"Add new move {currMove}");
                }

                stopwatch.Stop();
                console.WriteLine("ElapsedMilliseconds: " + stopwatch.ElapsedMilliseconds);

            }
        }



    }
}
