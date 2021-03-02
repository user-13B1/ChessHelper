using System;
using System.Collections.Generic;
using System.Text;
using Stockfish.NET;
using Stockfish;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;

namespace ChessHelper
{

    class Fish
    {
        internal delegate void ConsoleWriter(string message);
        internal event ConsoleWriter Notify;
        readonly IStockfish stockfishHard;
        Moves moves;
        static object locker = new object();

        public Fish(Moves moves )
        {
            this.moves = moves;

            stockfishHard = new Stockfish.NET.Stockfish(@"stockfish.exe")
            {
                SkillLevel = 20,
                Depth = 15
            };

        }

        internal bool SynchMove(string[] movesArray)
        {
            lock (locker)
            {
                stockfishHard.SetPosition(movesArray);
            }
            return true;
        }

        internal void GetBestMove()
        {
            while (true)
            {
                Thread.Sleep(20);
               
                if (moves.IsNewMove() && moves.IsPlayerNextMove)
                {

                    lock (locker)
                    {
                        string bestMove = stockfishHard.GetBestMove();
                        moves.SetBestMove(bestMove);

                        string fastMove = stockfishHard.GetBestMove();
                        moves.SetFastMove(fastMove);

                    }
                    
                }
            }
        }

        internal bool IsMoveCorrect(string move)
        {
            bool result;
            lock (locker)
            {
                result = stockfishHard.IsMoveCorrect(move);
            }
            return result;
        }

        internal void SetDepth(int depth)
        {
            lock (locker)
            {
                stockfishHard.Depth = depth;
            }
        }
        

    }
}
