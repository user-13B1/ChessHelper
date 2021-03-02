using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;


namespace ChessHelper
{
    public class GameController
    {
        private readonly Writer console;
        private Board board;
        private Moves moves;
        private Fish fish;
        private Painter painter;
       


        public GameController(Writer console)
        {
            // field = new Field(66,1,142,34,54,528);
            Field field = new Field(75, 1, 70, 40, 60, 600);   // размеры поля для тренировки

            if (console!=null)
                this.console = console;

            moves = new Moves();
            moves.Notify += DisplayMessage;

            board = new Board(moves, field);
            board.Notify += DisplayMessage;

            fish = new Fish(moves );
            
            

            fish.Notify += DisplayMessage;
            moves.SetEngine(fish);

            painter = new Painter(field, board, moves);
            painter.Notify += DisplayMessage;

            DisplayMessage("Loaded");
        }

        internal void Restart()
        {
            //board.Restart();
            //moves.Restart();


            //fish.ClearHistory();
            //var moves = movesHystory.ToArray();
            //stockfish.SetPosition(moves);
            //stockfishF.SetPosition(moves);
        }

        internal void StartWork()
        {
            painter.OverlayLoad();
            board.GetMyFigureColor();

            Task.Run(() => board.Update());
            Task.Run(() => moves.QueueHandler());
            Task.Run(() => fish.GetBestMove());
            Task.Run(() => painter.Draw());
        }

        internal void SetDepthMoves(int value)
        {
            if (value > 30 || value < 1)
                fish.SetDepth(14);
            else
                fish.SetDepth(value);
        }

        void DisplayMessage(string mess)
        {
            console?.WriteLine(mess);
        }
    }
}




//    if (fastMove)
//    {


//            chessBoard.DrawBestMove(move, "red");

//            chessBoard.DrawBestMove(move, "green");

