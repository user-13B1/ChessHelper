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
    public class ChessBot
    {
       
        internal readonly Autoit autoIt;
        internal readonly Writer console;
        internal readonly OpenCV openCV;
        internal ChessBoard chessBoard;
        private readonly List<string> movesHystory;
        readonly IStockfish stockfish;
        readonly IStockfish stockfishF;
        int depth;
        bool isMyNextMove;
        bool isWhiteNextMove;
        bool fastMove;
        public ChessBot(Writer console)
        {
            isWhiteNextMove = true;
            this.console = console;
            autoIt = new Autoit(console, "LDPlayer-1");
            openCV = new OpenCV();
            chessBoard = new ChessBoard(console,openCV, autoIt);
           
            movesHystory = new List<string>();
            depth = 15;
            stockfish = new Stockfish.NET.Stockfish(@"stockfish.exe")
            {
                SkillLevel = 20
            };
            stockfishF = new Stockfish.NET.Stockfish(@"stockfish.exe")
            {
                SkillLevel = 17,
                Depth = 12
            };

            console.WriteLine("Loaded");
        }

        public ChessBot()
        {
            isWhiteNextMove = true;
            movesHystory = new List<string>();
            depth = 15;
            stockfish = new Stockfish.NET.Stockfish(@"stockfish.exe")
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
           
        }

        internal void NewGame()
        {
            chessBoard.GetMyFigureColor();
            isMyNextMove = chessBoard.whitefigure;
            movesHystory.Clear();
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
                        Task.Run(() => FastMove());
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
            Thread.Sleep(18);
            currMove_2 = chessBoard.UpdateMoveHystory();

            if(CheckMove(ref currMove_1, currMove_2))
            {
                move = currMove_1;
                return true;
            }
            else
                return false;
        }

        public bool CheckMove(ref string currMove_1, string currMove_2)
        {
            //Если фигуры в движение или игра не началась
            if (currMove_1 != currMove_2 || currMove_1 == null)
                return false;

            if (CheckIsRepeatMove(currMove_1))
                return false;

            if (stockfish.IsMoveCorrect(currMove_1))
                return true;

            //Проверка прохода в дамки
            currMove_1 += "q";
            if (CheckIsRepeatMove(currMove_1))
                return false;

            if (stockfish.IsMoveCorrect(currMove_1))
            {
                console?.WriteLine($"In the queens!");
                return true;
            }

            console?.WriteLine($"StockFish - wrong move. {currMove_1}");
            return false;
        }

        private bool CheckIsRepeatMove(string currMove_1)
        {
            if (movesHystory.Count > 0 && currMove_1 == movesHystory[^1])
                return true;

            return false;
        }


        internal void SetFastMove(bool @checked) => fastMove = @checked;

        void SetMove(string move)
        {
            PrintMove(move);
            movesHystory.Add(move);
            var moves = movesHystory.ToArray();
            stockfish.SetPosition(moves);
            stockfishF.SetPosition(moves);
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
            string move = stockfish.GetBestMove();
           
            if (stockfish.IsMoveCorrect(move))
            {
                chessBoard.DrawBestMove(move, "blue");
            }
            else
                console.WriteLine($"Wrong move {move}");
        }

        void FastMove()
        {
            if (fastMove)
            {
                string move = stockfishF.GetBestMove();
                if (stockfishF.IsMoveCorrect(move))
                    chessBoard.DrawBestMove(move, "red");
            }
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
