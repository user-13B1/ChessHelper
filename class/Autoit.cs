﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoItX3Lib;
using System.Threading;
using System.Globalization;
using System.Windows.Forms;

namespace ChessHelper
{
    class Autoit
    {
        internal readonly AutoItX3 au3;
        private readonly string appName;
        internal Rectangle window;
        internal event NotifyDelegate Notify;

        public Autoit(string appName, NotifyDelegate Notify)
        {
            this.Notify = Notify;
            this.appName = appName;

            try
            {
                au3 = new AutoItX3();
                SetWindow();
            }
            catch (Exception e)
            {
                Notify( e.Message);
            }
            UpdateWindowPos();
        }

        public bool UpdateWindowPos()
        {
            au3.WinActivate(appName);
            if (au3.WinExists(appName) != 1)
            {
                Notify("Game not loaded. Please load game.");
                return false;
            }

            window = new Rectangle(au3.WinGetPosX(appName), au3.WinGetPosY(appName), au3.WinGetPosWidth(appName), au3.WinGetPosHeight(appName));
            return true;
        }

        public bool SetWindow()
        {
            if (au3.WinExists(appName) != 1)
            {

                Notify("Game not loaded.Please load game.");
                return false;
            }

            au3.WinSetState(appName, "", 1);
            au3.WinActivate(appName);
           
            // au3.WinSetOnTop(appName, "", 1);
            //if (au3.WinMove(appName, "", 2000, 10, 1322, 756) != 1)
            //{
            //    console.WriteLine("Error. Window not founded");
            //    return false;
            //}

            if (au3.WinGetPosHeight(appName) != 756)
            {
                Notify("Window height not correct");
                return false;
            }

            if (au3.WinGetPosWidth(appName) != 1322)
            {
                Notify("Window width not correct");
                return false;
            }

            return true;
        }

        internal System.Drawing.Point GetPosWindow()
        {
            return new System.Drawing.Point(window.X, window.Y);
        }

        internal System.Drawing.Point GetPosField()
        {
            UpdateWindowPos();
            return new System.Drawing.Point(window.X, window.Y);
        }

        internal int GetPixelColor(int x, int y)
        {
            return au3.PixelGetColor(x + window.X, y + window.Y);
        }

        internal int GetPixelColor(Point p)
        {
            return au3.PixelGetColor(p.X + window.X, p.Y + window.Y);
        }


    }
}
