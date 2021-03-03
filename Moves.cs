using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace ChessHelper
{
    class Moves
    {
        internal string bestNextMove;
        internal string fastNextMove;
        internal string slyNextMove;
        internal readonly List<string> movesHystory;
        private bool isNewMove, isNewFrame;
        internal bool IsPlayerNextMove;
        internal event NotifyDelegate Notify;

        private bool isWhiteNextMove;
        Queue<string> qMoves;
        Fish fish;
        static object lockerQ = new object();
        public Moves()
        {
            qMoves = new Queue<string>();
            movesHystory = new List<string>();
            isWhiteNextMove = true;
            isNewMove = isNewFrame= false;
        }

        internal void AddMoveToHistory(string move)
        {
            if (move == null || move == "")
            {
                Notify?.Invoke("Error. Null move.");
                return;
            }
          
            movesHystory.Add(move);

            if (isWhiteNextMove)
                Notify?.Invoke($"White: {move}");
            else
                Notify?.Invoke($"Black: {move}");
        }

        internal void Restart()
        {
            bestNextMove = string.Empty;
            qMoves = new Queue<string>();
            isWhiteNextMove = true;
            isNewMove = isNewFrame = false;
            movesHystory.Clear();
        }

        internal string[] GetFishMove()
        {
            string[] moveArr = new string[3] { bestNextMove, fastNextMove, slyNextMove};
            bestNextMove = null;
            fastNextMove = null;
            slyNextMove = null;
            return moveArr;
        }

        internal bool IsNewFrame()
        {
            if (isNewFrame)
            {
                isNewFrame = false;
                return true;
            }
            return false;
        }

        internal void SetEngine(Fish fish)
        {
            this.fish = fish;
        }

        internal bool IsNewMove()
        {
            if (isNewMove)
            {
                isNewMove = false;
                return true;
            }
            return false;
        }

        internal string GetlastMove()
        {
            if (movesHystory.Count == 0)
                return string.Empty;

            return movesHystory[^1];
        }

        internal bool CheckIsRepeatMove(string move)
        {
            if (movesHystory.Count == 0)
                return false;

            if ( move == movesHystory[^1])
                return true;
           
            //проверка на дамки
            move += "q";
            if ( move == movesHystory[^1])
                return true;

            return false;
        }

        internal void AddMoveToQueue(string move)
        {
            if (CheckIsRepeatMove(move))
                return;

            lock (lockerQ)
            {
                qMoves.Enqueue(move);
            }
          
        }

        internal void QueueHandler()
        {
            string move;

            while (true)
            {
                Thread.Sleep(20);
                if (qMoves.Count > 0)
                {
                    lock (lockerQ)
                    {
                        move = qMoves.Dequeue();
                    }

                    if (SetMove(move))
                        continue;

                    move += "q";
                    if (SetMove(move))
                        continue;

                    Notify($"Error set move. {move}");
                    return;
                }
            }
        }

        private bool SetMove(string move)
        {
            if (fish.IsMoveCorrect(move))
            {
                AddMoveToHistory(move);
                fish.SynchMove(GetHistoryMovesArray());

                IsPlayerNextMove = !IsPlayerNextMove;
                if (IsPlayerNextMove)
                {
                    isNewMove = true;
                }
                else
                {
                    isNewFrame = true;
                }

                isWhiteNextMove = !isWhiteNextMove;

                return true;
            }
            return false;
        }

        internal void SetBestMove(string move)
        {
            bestNextMove = move;
            isNewFrame = true;
        }

        internal void SetFastMove(string move)
        {
            fastNextMove = move;
            isNewFrame = true;
        }

        internal void SetSlyMove(string move)
        {
            slyNextMove = move;
            isNewFrame = true;
        }
        internal string[] GetHistoryMovesArray()
        {
            return movesHystory.ToArray();
        }

        internal void ClearHistory()
        {
            movesHystory.Clear();
            Notify?.Invoke("Clear history moves.");
        }


    }
}
