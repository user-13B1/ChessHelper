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

    
        internal void StartWork()
        {
            chessBoard.OverlayLoad();
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
                        console.WriteLine($"В дамки!");
                        SetMove(currMove_1);
                        continue;
                    }

                    console.WriteLine($"СтокФиш - ход некорректный. {currMove_1}");
                    return;
                }

            }
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
                console.WriteLine($"Ход белых:  {move}");
            else
                console.WriteLine($"Ход черных: {move}");
            isWhiteNextMove = !isWhiteNextMove;
        }

        private void UpdateNextMove()
        {
            stockfish.Depth = depth;

            string moveFast = stockfish.GetBestMoveTime(100);
            chessBoard.DrawBestMove(moveFast, "red");

            string move = stockfish.GetBestMove();
            if (stockfish.IsMoveCorrect(move))
            {
                chessBoard.DrawBestMove(move, "blue");
            }
            else
                console.WriteLine($"Некорректный ход {move}");
        }

        internal void SetDepthMoves(int value)
        {
            if (value > 30 || value < 1)
                depth = 12;
            else
                depth = value;
            console.WriteLine($"Глубина простчета ходов {depth}");
        }

    }
}
