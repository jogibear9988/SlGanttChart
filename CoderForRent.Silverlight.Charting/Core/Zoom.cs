/*
 * ********************************
 *  Copyright © 2009. CoderForRent,LLC. All Rights Reserved.  Licensed under the GNU General Public License version 2 (GPLv2) .
 * 
 * */

namespace CoderForRent.Charting.Core
{
    public class Zoom
    {
        private static double _Value = 1.0;
        public static double Value { get { return _Value; } set { _Value = value; } }
    }
}
