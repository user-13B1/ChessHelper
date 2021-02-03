using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Stockfish;
using Stockfish.NET;
using System.Threading;

namespace ChessHelper
{
    class ChessBot
    {
        internal readonly Autoit autoIt;
        internal readonly Writer console;
        internal readonly OpenCV openCV;
        internal ChessBoard chessBoard;
        private readonly List<string> movesHystory;
        readonly IStockfish stockfish;
        int depth;
        bool isMyNextMove;
        bool isWhiteNextMove;
        bool fastMove;
        public ChessBot(Writer console)
        {
            isWhiteNextMove = true;
            this.console = console;
            autoIt = new Autoit(console, "LDPlayer-1");
            openCV = new OpenCV(console);
            chessBoard = new ChessBoard(console,openCV, autoIt);
            movesHystory = new List<string>();
            depth = 15;
            stockfish = new Stockfish.NET.Stockfish(@"stockfish.exe", depth: depth)
            {
                SkillLevel = 20
            };
        }

    
        internal void StartWork(bool VsPc)
        {
            chessBoard.OverlayLoad(VsPc);
            chessBoard.GetMyFigureColor();
            isMyNextMove = chessBoard.whitefigure;
            Task.Run(() => UpdateBoard());
            Task.Run(() => UpdateNextMove());
        }

        private void UpdateBoard()
        {
            string currMove_1;
            string currMove_2;

            while (true)
            {
                
                currMove_1 = chessBoard.UpdateMoveHystory();
                Thread.Sleep(30);
                currMove_2 = chessBoard.UpdateMoveHystory();
                
                if (currMove_1 != currMove_2 || currMove_1 == null)
                    continue;
                
                if ((movesHystory.Count == 0 || currMove_1 != movesHystory[^1]) && stockfish.IsMoveCorrect(currMove_1))
                {
                    SetMove(currMove_1);
                    continue;
                }
                
                if (!stockfish.IsMoveCorrect(currMove_1) && currMove_1 != movesHystory[^1])
                {
                    currMove_1 += "q";
                    if (currMove_1 == movesHystory[^1])
                        continue;

                    if (stockfish.IsMoveCorrect(currMove_1))
                    {
                        console.WriteLine($"In the queens!");
                        SetMove(currMove_1);
                        continue;
                    }

                    console.WriteLine($"StockFish - wrong move. {currMove_1}");
                    return;
                }

            }
        }

        internal void SetFastMove(bool @checked)
        {
            fastMove = @checked;
        }

        void SetMove(string move)
        {
            PrintMove(move);

            movesHystory.Add(move); 
            stockfish.SetPosition(movesHystory.ToArray());

            if (!isMyNextMove)
            {
                UpdateNextMove();
            }
            else
                chessBoard.overlay.ClearFrame();
            
            isMyNextMove = !isMyNextMove;
        }

        private void PrintMove(string move)
        {
            if (isWhiteNextMove)
                console.WriteLine($"White move:  {move}");
            else
                console.WriteLine($"Black move: {move}");
            isWhiteNextMove = !isWhiteNextMove;
        }

        private void UpdateNextMove()
        {
            stockfish.Depth = depth;

            if (fastMove)
            {
                string moveFast = stockfish.GetBestMoveTime(100);
                chessBoard.DrawBestMove(moveFast, "red");
            }

            string move = stockfish.GetBestMove();
            if (stockfish.IsMoveCorrect(move))
            {
                chessBoard.DrawBestMove(move, "blue");
            }
            else
                console.WriteLine($"Wrong move {move}");
        }

        internal void SetDepthMoves(int value)
        {
            if (value > 30 || value < 1)
                depth = 12;
            else
                depth = value;
        }

    }
}
