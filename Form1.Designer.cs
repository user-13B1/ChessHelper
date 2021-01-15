
namespace ChessHelper
{
    partial class Form1
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
            this.ConsoleBox = new System.Windows.Forms.TextBox();
            this.ButtonStart = new System.Windows.Forms.Button();
            this.textBoxDepth = new System.Windows.Forms.TextBox();
            this.LabelDepth = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ConsoleBox
            // 
            this.ConsoleBox.Location = new System.Drawing.Point(12, 12);
            this.ConsoleBox.Multiline = true;
            this.ConsoleBox.Name = "ConsoleBox";
            this.ConsoleBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ConsoleBox.Size = new System.Drawing.Size(189, 139);
            this.ConsoleBox.TabIndex = 1;
            // 
            // ButtonStart
            // 
            this.ButtonStart.Location = new System.Drawing.Point(126, 201);
            this.ButtonStart.Name = "ButtonStart";
            this.ButtonStart.Size = new System.Drawing.Size(75, 23);
            this.ButtonStart.TabIndex = 3;
            this.ButtonStart.Text = "Start";
            this.ButtonStart.UseVisualStyleBackColor = true;
            this.ButtonStart.Click += new System.EventHandler(this.ButtonStart_Click);
            // 
            // textBoxDepth
            // 
            this.textBoxDepth.Location = new System.Drawing.Point(161, 157);
            this.textBoxDepth.Name = "textBoxDepth";
            this.textBoxDepth.Size = new System.Drawing.Size(40, 23);
            this.textBoxDepth.TabIndex = 4;
            this.textBoxDepth.Text = "10";
            this.textBoxDepth.TextChanged += new System.EventHandler(this.textBoxDepth_TextChanged);
            // 
            // LabelDepth
            // 
            this.LabelDepth.AutoSize = true;
            this.LabelDepth.Location = new System.Drawing.Point(117, 160);
            this.LabelDepth.Name = "LabelDepth";
            this.LabelDepth.Size = new System.Drawing.Size(39, 15);
            this.LabelDepth.TabIndex = 5;
            this.LabelDepth.Text = "Depth";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(213, 236);
            this.Controls.Add(this.LabelDepth);
            this.Controls.Add(this.textBoxDepth);
            this.Controls.Add(this.ButtonStart);
            this.Controls.Add(this.ConsoleBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox ConsoleBox;
        private System.Windows.Forms.Button ButtonStart;
        private System.Windows.Forms.TextBox textBoxDepth;
        private System.Windows.Forms.Label LabelDepth;
    }
}

