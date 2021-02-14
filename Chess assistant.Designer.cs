
namespace ChessHelper
{
    partial class ChessHelper
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChessHelper));
            this.ConsoleBox = new System.Windows.Forms.TextBox();
            this.ButtonStart = new System.Windows.Forms.Button();
            this.LabelDepth = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.textBoxDepth = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkBoxVsPc = new System.Windows.Forms.CheckBox();
            this.checkBoxFastMove = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ConsoleBox
            // 
            this.ConsoleBox.Location = new System.Drawing.Point(12, 12);
            this.ConsoleBox.Multiline = true;
            this.ConsoleBox.Name = "ConsoleBox";
            this.ConsoleBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ConsoleBox.Size = new System.Drawing.Size(180, 127);
            this.ConsoleBox.TabIndex = 1;
            // 
            // ButtonStart
            // 
            this.ButtonStart.Location = new System.Drawing.Point(117, 211);
            this.ButtonStart.Name = "ButtonStart";
            this.ButtonStart.Size = new System.Drawing.Size(75, 25);
            this.ButtonStart.TabIndex = 3;
            this.ButtonStart.Text = "Start";
            this.ButtonStart.UseVisualStyleBackColor = true;
            this.ButtonStart.Click += new System.EventHandler(this.ButtonStart_Click);
            // 
            // LabelDepth
            // 
            this.LabelDepth.AutoSize = true;
            this.LabelDepth.Location = new System.Drawing.Point(10, 13);
            this.LabelDepth.Name = "LabelDepth";
            this.LabelDepth.Size = new System.Drawing.Size(39, 15);
            this.LabelDepth.TabIndex = 5;
            this.LabelDepth.Text = "Depth";
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(45, 6);
            this.trackBar1.Maximum = 20;
            this.trackBar1.Minimum = 5;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(125, 45);
            this.trackBar1.TabIndex = 6;
            this.trackBar1.Tag = "";
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trackBar1.Value = 12;
            this.trackBar1.Scroll += new System.EventHandler(this.TrackBar1_Scroll);
            // 
            // textBoxDepth
            // 
            this.textBoxDepth.Enabled = false;
            this.textBoxDepth.Location = new System.Drawing.Point(168, 10);
            this.textBoxDepth.Name = "textBoxDepth";
            this.textBoxDepth.Size = new System.Drawing.Size(22, 23);
            this.textBoxDepth.TabIndex = 4;
            this.textBoxDepth.Text = "12";
            this.textBoxDepth.TextChanged += new System.EventHandler(this.TextBoxDepth_TextChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.textBoxDepth);
            this.panel1.Controls.Add(this.trackBar1);
            this.panel1.Controls.Add(this.LabelDepth);
            this.panel1.Location = new System.Drawing.Point(2, 145);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 51);
            this.panel1.TabIndex = 7;
            // 
            // checkBoxVsPc
            // 
            this.checkBoxVsPc.AutoSize = true;
            this.checkBoxVsPc.Location = new System.Drawing.Point(12, 190);
            this.checkBoxVsPc.Name = "checkBoxVsPc";
            this.checkBoxVsPc.Size = new System.Drawing.Size(56, 19);
            this.checkBoxVsPc.TabIndex = 8;
            this.checkBoxVsPc.Text = "Vs PC";
            this.checkBoxVsPc.UseVisualStyleBackColor = true;
            // 
            // checkBoxFastMove
            // 
            this.checkBoxFastMove.AutoSize = true;
            this.checkBoxFastMove.Location = new System.Drawing.Point(12, 215);
            this.checkBoxFastMove.Name = "checkBoxFastMove";
            this.checkBoxFastMove.Size = new System.Drawing.Size(80, 19);
            this.checkBoxFastMove.TabIndex = 9;
            this.checkBoxFastMove.Text = "Fast move";
            this.checkBoxFastMove.UseVisualStyleBackColor = true;
            this.checkBoxFastMove.CheckedChanged += new System.EventHandler(this.CheckBoxFastMove_CheckedChanged);
            // 
            // ChessHelper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(204, 248);
            this.Controls.Add(this.checkBoxFastMove);
            this.Controls.Add(this.checkBoxVsPc);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ButtonStart);
            this.Controls.Add(this.ConsoleBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ChessHelper";
            this.Text = "Rook move!";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox ConsoleBox;
        private System.Windows.Forms.Button ButtonStart;
        private System.Windows.Forms.Label LabelDepth;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.TextBox textBoxDepth;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox checkBoxVsPc;
        private System.Windows.Forms.CheckBox checkBoxFastMove;
    }
}

