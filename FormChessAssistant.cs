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
    public partial class FormChessAssistant : Form
    {
        internal readonly Writer console;
        GameController gameController;
        public FormChessAssistant()
        {
            InitializeComponent();
            console = new Writer(new object(), this, ConsoleBox);
            textBoxDepth.Text = trackBar1.Value.ToString();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            gameController = new GameController(console);
        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            if (ButtonStart.Text == "Start")
            {
                checkBoxVsPc.Enabled = false;
                gameController.StartWork();
                ButtonStart.Text = "Restart";
            }
            else
                gameController.Restart();
            
        }

        private void TextBoxDepth_TextChanged(object sender, EventArgs e)
        {
            if (!Int32.TryParse(textBoxDepth.Text, out _))
                textBoxDepth.Text = 10.ToString();
            else
            {
                int value = int.Parse(textBoxDepth.Text);
            }
        }

        private void TrackBar1_Scroll(object sender, EventArgs e)
        {
            textBoxDepth.Text = trackBar1.Value.ToString();
            gameController.SetDepthMoves(trackBar1.Value);
        }

    }
}
