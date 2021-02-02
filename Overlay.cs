using System;
using System.Collections.Generic;
using System.Text;
using SharpDX.Windows;
using SharpDX;
using SharpDX.Direct2D1;
using System.Threading;
using SharpDX.Mathematics.Interop;

namespace ChessHelper
{
    class Overlay
    {

        #region sharpDX
        readonly WindowRenderTarget renderTarget;
        readonly RenderForm overlayForm;
        readonly SolidColorBrush brushBlue;
        readonly SolidColorBrush brushRed;
        readonly SharpDX.DirectWrite.Factory DWfactory;
        readonly SharpDX.DirectWrite.TextFormat textFormat;
        #endregion

      
        private List<Rectangle> rectsBlue;
        private List<Rectangle> rectsRed;
        private List<TextOverlay> texts;
       
        public Overlay(int formWidth, int formHeight)
        {
           
            overlayForm = new RenderForm("OverlayDX")
            {
                TransparencyKey = System.Drawing.Color.Black,
                Enabled = true,
                ShowIcon = false,
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.None,
                Width = formWidth,
                Height = formHeight,
                TopMost = true,
                ShowInTaskbar = false
            };

            Factory factory2D = new Factory();

            HwndRenderTargetProperties properties = new HwndRenderTargetProperties
            {
                Hwnd = overlayForm.Handle,
                PixelSize = new SharpDX.Size2(overlayForm.Width, overlayForm.Height),
                PresentOptions = PresentOptions.Immediately
            };

            renderTarget = new WindowRenderTarget(factory2D, new RenderTargetProperties(new PixelFormat(SharpDX.DXGI.Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied)), properties)
            {
                AntialiasMode = AntialiasMode.PerPrimitive,
                TextAntialiasMode = TextAntialiasMode.Cleartype,
                StrokeWidth = 2f
            };

            brushBlue = new SolidColorBrush(renderTarget, new RawColor4(0, 0, 1, 1));
            brushRed = new SolidColorBrush(renderTarget, new RawColor4(1, 0, 0, 1));

            DWfactory = new SharpDX.DirectWrite.Factory();
            textFormat = new SharpDX.DirectWrite.TextFormat(DWfactory, "Calibri", 14);
        }
        


        internal void Load(System.Drawing.Point sourcePos)
        {
            rectsBlue = new List<Rectangle>();
            rectsRed = new List<Rectangle>();

            texts = new List<TextOverlay>();
            overlayForm.Show();
            overlayForm.Location = sourcePos;
            UpdateFrame();
        }

        public void UpdateFrame()
        {
            renderTarget.BeginDraw();
            renderTarget.Clear(new RawColor4(0, 0, 0, 0));

            renderTarget.DrawRectangle(new RawRectangleF(0, 0, overlayForm.Width, overlayForm.Height), brushBlue);
            
            foreach (var rect in rectsBlue)
            {
                renderTarget.DrawRectangle(new RawRectangleF(rect.X - 1, rect.Y - 1, rect.Width + rect.X + 1, rect.Height + rect.Y + 1), brushBlue);
            }

            foreach (var rect in rectsRed)
            {
                renderTarget.DrawRectangle(new RawRectangleF(rect.X - 1, rect.Y - 1, rect.Width + rect.X + 1, rect.Height + rect.Y + 1), brushRed);
            }

            foreach (var text in texts)
            {
                renderTarget.DrawText(text.s, textFormat, new RawRectangleF(text.pos.X, text.pos.Y, text.pos.X + 120, text.pos.Y + 40), brushBlue);
            }

            renderTarget.Flush();
            renderTarget.EndDraw();
        }

        public void ClearElements()
        {
            rectsRed.Clear();
            rectsBlue.Clear();
            texts.Clear();
           
        }

        public void ClearFrame()
        {
            rectsRed.Clear();
            rectsBlue.Clear();
            texts.Clear();
            UpdateFrame();
        }

        internal void DrawRect(int x, int y, int width, int height)=> rectsBlue.Add(new Rectangle(x, y, width, height));

        internal void DrawRedRect(int x, int y, int width, int height) => rectsRed.Add(new Rectangle(x, y, width, height));

        internal void DrawText(string s, int x, int y) => texts.Add(new TextOverlay(s, new Point(x, y)));

        struct TextOverlay
        {
            public string s;
            public Point pos;

            public TextOverlay(string text, Point pos)
            {
                this.s = text;
                this.pos = pos;
            }
        }

    }
}
