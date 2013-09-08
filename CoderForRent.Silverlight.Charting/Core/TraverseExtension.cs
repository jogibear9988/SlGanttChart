/*
 * ********************************
 *  Copyright © 2009. CoderForRent,LLC. All Rights Reserved.  Licensed under the GNU General Public License version 2 (GPLv2) .
 * 
 * */


using System;
using System.Collections.Generic;

namespace CoderForRent.Charting.Core
{
    public static class TraverseExtension
    {
        public static IEnumerable<T> Traverse<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> fnRecurse)
        {

            foreach (T item in source)
            {

                yield return item;

                IEnumerable<T> seqRecurse = fnRecurse(item);

                if (seqRecurse != null)
                {

                    foreach (T itemRecurse in Traverse(seqRecurse, fnRecurse))
                    {

                        yield return itemRecurse;

                    }

                }

            }

        }


    }
}
