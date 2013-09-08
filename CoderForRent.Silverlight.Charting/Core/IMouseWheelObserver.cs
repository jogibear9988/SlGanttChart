/*
 * ********************************
 *  Copyright © 2009. CoderForRent,LLC. All Rights Reserved.  Licensed under the GNU General Public License version 2 (GPLv2) .
 * 
 * */

using System.Windows.Input;

namespace CoderForRent.Charting.Core
{
    public interface IMouseWheelObserver 
    { 
        void OnMouseWheel(MouseWheelArgs args); 
        event MouseEventHandler MouseEnter; 
        event MouseEventHandler MouseLeave;
    }
}
