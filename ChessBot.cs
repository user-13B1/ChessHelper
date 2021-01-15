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
        int depth;
        internal readonly OpenCV openCV;
        internal ChessBoard chessBoard;
        private List<string> movesHystory;
        IStockfish stockfish;
        public ChessBot(Writer console)
        {
            this.console = console;
            autoIt = new Autoit(console, "LDPlayer-1");
            openCV = new OpenCV(console, autoIt);
            chessBoard = new ChessBoard(console,openCV, autoIt);
            movesHystory = new List<string>();
           // Stockfish.NET.Models.Settings setting = new Stockfish.NET.Models.Settings();
            stockfish = new Stockfish.NET.Stockfish(@"stockfish.exe", depth: 20);
            stockfish.SkillLevel = 20;
            depth = 10;
        }

    
        internal void StartWork()
        {
            chessBoard.OverlayLoad();
            chessBoard.UpdateMyFigureColor();
            Task.Run(() => UpdateBoard());
            UpdateFastMove();
        }

        
        private void UpdateFastMove()
        {
            stockfish.Depth = depth;
            string fastrMove = stockfish.GetBestMove();
            if (stockfish.IsMoveCorrect(fastrMove))
            {
                chessBoard.DrawBestMove(fastrMove);
                console.WriteLine($"FastrMove {fastrMove}");
            }
            else
                console.WriteLine($"Некорректный ход {fastrMove}");

        }

       

        internal void SetDepthMoves(int value)
        {
            if (value > 30 || value < 1)
                depth = 10;
            else
                depth = value;
            console.WriteLine($"Глубина простчета ходов {depth}");
        }

        private void UpdateBoard()
        {
            string currMove_1;
            string currMove_2;
            bool flipflop = chessBoard.whiteGame;

            while (true)
            {
                currMove_1 = chessBoard.UpdateMoveHystory();
                Thread.Sleep(50);
                currMove_2 = chessBoard.UpdateMoveHystory();

                if (currMove_1 != currMove_2 || currMove_1 == null)
                    continue;

                if (movesHystory.Count == 0 && stockfish.IsMoveCorrect(currMove_1))         //First move
                {
                    console.WriteLine($"First move: {currMove_1}");
                    movesHystory.Add(currMove_1);
                    stockfish.SetPosition(movesHystory.ToArray());
                    UpdateFastMove();
                    continue;
                }

                if (currMove_1 != movesHystory[^1]  && stockfish.IsMoveCorrect(currMove_1))
                {
                    movesHystory.Add(currMove_1);
                    console.WriteLine($"Move: {currMove_1}");
                    stockfish.SetPosition(movesHystory.ToArray());
                    if(flipflop) 
                        UpdateFastMove();
                    flipflop = !flipflop;
                    continue;
                }



                if (!stockfish.IsMoveCorrect(currMove_1) && currMove_1 != movesHystory[movesHystory.Count - 1])
                {
                    currMove_1 += "q";
                    if (currMove_1 == movesHystory[^1])
                    {
                        continue;
                    }

                    if (stockfish.IsMoveCorrect(currMove_1))
                    {
                        console.WriteLine($"Дамки");
                        movesHystory.Add(currMove_1);
                        stockfish.SetPosition(movesHystory.ToArray());
                        if (flipflop)
                            UpdateFastMove();
                        flipflop = !flipflop;
                        continue;
                    }

                    console.WriteLine($"СтокФиш - ход некорректный. {currMove_1}");
                    return;
                }


            }
        }



    }
}
