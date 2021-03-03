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
        int defaultDepth;
        public Fish(Moves moves )
        {
            this.moves = moves;
            defaultDepth = 15;

            stockfishHard = new Stockfish.NET.Stockfish(@"stockfish.exe")
            {
                SkillLevel = 20,
                Depth = defaultDepth
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
                        stockfishHard.SkillLevel = 7;
                        stockfishHard.Depth = 8;
                        string fastMove = stockfishHard.GetBestMove();
                        moves.SetFastMove(fastMove);

                        stockfishHard.SkillLevel = 15;
                        stockfishHard.Depth = 5;
                        string slyMove = stockfishHard.GetBestMoveTime(50);
                        moves.SetSlyMove(slyMove);

                        stockfishHard.SkillLevel = 20;
                        stockfishHard.Depth = defaultDepth;
                        string bestMove = stockfishHard.GetBestMove();
                        moves.SetBestMove(bestMove);

                      
                    }
                    
                }
            }
        }

        internal void Restart()
        {
            SynchMove(new string[] { });
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
                defaultDepth = depth;
            }
        }
        

    }
}
