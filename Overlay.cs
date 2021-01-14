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
        public int FORMWIDTH, FORMHEIGHT;
        WindowRenderTarget renderTarget;
        RenderForm overlayForm;
        SolidColorBrush brush;
        SharpDX.DirectWrite.Factory DWfactory;
        SharpDX.DirectWrite.TextFormat textFormat;
        #endregion

        int formWidth, formHeight;
        Autoit autoIt;
        List<Rectangle> rects;
        List<textOverlay> texts;


        public Overlay(Autoit autoIt)
        {
            formWidth = 528;
            formHeight = 528;
            this.autoIt = autoIt;
            overlayForm = new RenderForm("SharpDX")
            {
                TransparencyKey = System.Drawing.Color.Black,
               
                Enabled = true,
                ShowIcon = false,
                //FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle,
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.None,
                Width = formWidth,
                Height = formHeight,
                TopMost = true,
            };

            Factory factory2D = new Factory();

            HwndRenderTargetProperties properties = new HwndRenderTargetProperties
            {
                Hwnd = overlayForm.Handle,
                PixelSize = new SharpDX.Size2(overlayForm.Width, overlayForm.Height),
                PresentOptions = PresentOptions.Immediately
            };

            renderTarget = new WindowRenderTarget(factory2D, new RenderTargetProperties(new PixelFormat(SharpDX.DXGI.Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied)), properties);
            renderTarget.AntialiasMode = AntialiasMode.PerPrimitive;
            renderTarget.TextAntialiasMode = TextAntialiasMode.Cleartype;
            renderTarget.StrokeWidth = 1.5f;
            brush = new SolidColorBrush(renderTarget, new RawColor4(1, 0, 0, 0.8f));
            DWfactory = new SharpDX.DirectWrite.Factory();
            textFormat = new SharpDX.DirectWrite.TextFormat(DWfactory, "Calibri", 16);
          
           // overlayForm.Show();
            
        }



        internal void Load()
        {
            rects = new List<Rectangle>();
            texts = new List<textOverlay>();
            overlayForm.Show();
            overlayForm.Location = autoIt.GetPosField();
            UpdateFrame();

        }

        public void UpdateFrame()
        {
            renderTarget.BeginDraw();
            renderTarget.Clear(new RawColor4(0, 0, 0, 0));


           // renderTarget.DrawRectangle(new RawRectangleF(0, 0, formWidth, formHeight), brush);

            foreach (var rect in rects)
            {
                renderTarget.DrawRectangle(new RawRectangleF(rect.X - 1, rect.Y - 1, rect.Width + rect.X + 1, rect.Height + rect.Y + 1), brush);
            }

            foreach (var text in texts)
            {
                renderTarget.DrawText(text.s, textFormat, new RawRectangleF(text.pos.X, text.pos.Y, text.pos.X + 120, text.pos.Y + 40), brush);

            }


            renderTarget.Flush();
            renderTarget.EndDraw();
            overlayForm.Activate();
            Thread.Sleep(16);
        }

        public void ClearElements()
        {
            rects.Clear();
            texts.Clear();
        }

        internal void DrawRect(int x, int y, int width, int height)
        {
            rects.Add(new Rectangle(x, y, width, height));
            // UpdateFrame();
        }
      
        internal void DrawText(string s, int x, int y)
        {
            texts.Add(new textOverlay(s, new Point(x, y)));
            // UpdateFrame();
        }

        struct textOverlay
        {
            public string s;
            public Point pos;

            public textOverlay(string text, Point pos)
            {
                this.s = text;
                this.pos = pos;
            }
        }


    }
}
