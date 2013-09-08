/*
 * ********************************
 *  Copyright © 2009. CoderForRent,LLC. All Rights Reserved.  Licensed under the GNU General Public License version 2 (GPLv2) .
 * 
 * */


using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Input;

namespace CoderForRent.Charting.Core
{
    public class MouseWheelListener
    {
        private Stack<IMouseWheelObserver> _ElementStack;
        private MouseWheelListener()
        {
            this._ElementStack = new Stack<IMouseWheelObserver>();
            HtmlPage.Window.AttachEvent("DOMMouseScroll", OnMouseWheel);
            HtmlPage.Window.AttachEvent("onmousewheel", OnMouseWheel);
            HtmlPage.Document.AttachEvent("onmousewheel", OnMouseWheel);
            Application.Current.Exit += new EventHandler(OnApplicationExit);
        }
        /// <summary>    
        /// Detaches from the browser-generated scroll events.    
        /// </summary>   
        private void Dispose()
        {
            HtmlPage.Window.DetachEvent("DOMMouseScroll", OnMouseWheel);
            HtmlPage.Window.DetachEvent("onmousewheel", OnMouseWheel);
            HtmlPage.Document.DetachEvent("onmousewheel", OnMouseWheel);
        }
        public void AddObserver(IMouseWheelObserver element)
        {
            element.MouseEnter += new MouseEventHandler(OnElementMouseEnter);
            element.MouseLeave += new MouseEventHandler(OnElementMouseLeave);
        }

        private void OnMouseWheel(object sender, HtmlEventArgs args)
        {
            double delta = 0;
            ScriptObject e = args.EventObject;
            if (e.GetProperty("detail") != null)
            {
                // Mozilla and Safari      
                delta = ((double)e.GetProperty("detail"));
            }
            else if (e.GetProperty("wheelDelta") != null)
            {
                // IE and Opera          
                delta = ((double)e.GetProperty("wheelDelta"));
            }
            delta = Math.Sign(delta);

            if (this._ElementStack.Count > 0)
                this._ElementStack.Peek().OnMouseWheel(new MouseWheelArgs(delta, args.ShiftKey, args.CtrlKey, args.AltKey));
        }

        private void OnElementMouseLeave(object sender, MouseEventArgs e)
        { this._ElementStack.Pop(); }

        private void OnElementMouseEnter(object sender, MouseEventArgs e)
        { this._ElementStack.Push((IMouseWheelObserver)sender); }

        private void OnApplicationExit(object sender, EventArgs e)
        { this.Dispose(); }

        private static MouseWheelListener _Instance = null;
        public static MouseWheelListener Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new MouseWheelListener();
                }

                return _Instance;
            }
        }
    }
}
