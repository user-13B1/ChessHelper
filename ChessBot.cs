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
            stockfish = new Stockfish.NET.Stockfish(@"stockfish.exe");
        }

    
        internal void StartWork(bool VsPc)
        {
            chessBoard.OverlayLoad(VsPc);
            chessBoard.GetMyFigureColor();
            isMyNextMove = chessBoard.whitefigure;
            Task.Run(() => UpdateBoard());
           
        }

        internal void NewGame()
        {
            chessBoard.GetMyFigureColor();
            isMyNextMove = chessBoard.whitefigure;
            movesHystory.Clear();
        }

        private void UpdateBoardOld()
        {
            string currMove_1;
            string currMove_2;

            while (true)
            {
                currMove_1 = chessBoard.UpdateMoveHystory();
                Thread.Sleep(30);
                currMove_2 = chessBoard.UpdateMoveHystory();
                
                //Если фигуры в движение или игра не началась
                if (currMove_1 != currMove_2 || currMove_1 == null)
                    continue;
                
                //Если первый ход или новый ход
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

        private void UpdateBoard()
        {
            while (true)
            {
                if (UpdateMove(out string move))
                {
                    chessBoard.overlay.ClearFrame();
                    SetMove(move);
                    if (isMyNextMove)
                    {
                        UpdateNextMove();
                    }
                }

            }
        }


        private bool UpdateMove(out string move)
        {
            string currMove_1;
            string currMove_2;
            move = null;
            currMove_1 = chessBoard.UpdateMoveHystory();
            Thread.Sleep(20);
            currMove_2 = chessBoard.UpdateMoveHystory();

            //Проверка повтора хода
            if (movesHystory.Count > 0 && currMove_1 == movesHystory[^1])
                 return false;

            //Если фигуры в движение или игра не началась
            if (currMove_1 != currMove_2 || currMove_1 == null)
                return false;

            //Если первый ход или новый ход
            if (stockfish.IsMoveCorrect(currMove_1))
            {
                move = currMove_1;
                return true;
            }

            //Проверка на дамки
            currMove_1 += "q";
            
            if (currMove_1 == movesHystory[^1])
                return false;

            if (stockfish.IsMoveCorrect(currMove_1))
            {
                console.WriteLine($"In the queens!");
                move = currMove_1;
                return true;
            }

            console.WriteLine($"StockFish - wrong move. {currMove_1}");
            return false;
        }

        internal void SetFastMove(bool @checked) => fastMove = @checked;

        void SetMove(string move)
        {
            PrintMove(move);
            movesHystory.Add(move); 
            stockfish.SetPosition(movesHystory.ToArray());
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
            string move;
            if (fastMove)
            {
                stockfish.SkillLevel = 17;
                stockfish.Depth = 14;
                move = stockfish.GetBestMoveTime(140);
                if (stockfish.IsMoveCorrect(move))
                     chessBoard.DrawBestMove(move, "red");
            }

            stockfish.SkillLevel = 20;
            stockfish.Depth = depth;
            move = stockfish.GetBestMove();
           
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
