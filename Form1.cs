using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using SharpDX.Windows;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using AutoItX3Lib;
using System.Globalization;
using Point = OpenCvSharp.Point;


namespace ChessHelper
{
    public partial class Form1 : Form
    {
        internal readonly Writer console;
        ChessBot bot;
        public Form1()
        {
            InitializeComponent();
            console = new Writer(new object(), this, ConsoleBox);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           bot = new ChessBot(console);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bot.chessBoard.TestTaskScanColor();
        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            bot.StartWork();
        }
    }
}
