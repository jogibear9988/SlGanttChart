/*
 * ********************************
 *  Copyright © 2009. CoderForRent,LLC. All Rights Reserved.  Licensed under the GNU General Public License version 2 (GPLv2) .
 * 
 * */


using System;

namespace CoderForRent.Silverlight.Charting.Core
{
    public class MouseWheelArgs : EventArgs
    {
        private readonly double _Delta;
        private readonly bool _ShiftKey, _CtrlKey, _AltKey;
        public double Delta
        {
            get { return this._Delta; }
        }

        public bool ShiftKey
        {
            get { return this._ShiftKey; }
        }

        public bool CtrlKey
        {
            get { return this._CtrlKey; }
        }

        public bool AltKey
        {
            get { return this._AltKey; }
        }

        public MouseWheelArgs(double delta, bool shiftKey, bool ctrlKey, bool altKey)
        {
            this._Delta = delta;
            this._ShiftKey = shiftKey;
            this._CtrlKey = ctrlKey;
            this._AltKey = altKey;
        }
    }
}
