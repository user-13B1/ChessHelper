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



namespace ChessHelper
{
    public partial class ChessHelp : Form
    {
        internal readonly Writer console;
        ChessBot bot;
        public ChessHelp()
        {
            InitializeComponent();
            console = new Writer(new object(), this, ConsoleBox);
            textBoxDepth.Text = trackBar1.Value.ToString();
        }

        private void Form1_Load(object sender, EventArgs e) => bot = new ChessBot(console);

        private void ButtonStart_Click(object sender, EventArgs e) => bot.StartWork();

        private void TextBoxDepth_TextChanged(object sender, EventArgs e)
        {
            if (!Int32.TryParse(textBoxDepth.Text, out _))
                textBoxDepth.Text = 10.ToString();
            else
            {
                int value = Int32.Parse(textBoxDepth.Text);
               // bot.SetDepthMoves(value);
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            textBoxDepth.Text = trackBar1.Value.ToString();
            bot.SetDepthMoves(trackBar1.Value);
        }


    }
}
