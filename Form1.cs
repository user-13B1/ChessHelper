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
           
        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            bot.StartWork();
        }

        private void textBoxDepth_TextChanged(object sender, EventArgs e)
        {
            int value = 10;
            if (!Int32.TryParse(textBoxDepth.Text, out _))
                textBoxDepth.Text = 10.ToString();
            else
            {
                value = Int32.Parse(textBoxDepth.Text);
                bot.SetDepthMoves(value);
            }
            
        }

    }
}
