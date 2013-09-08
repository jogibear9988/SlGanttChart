using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CoderForRent.Charting.Core
{
    public static class UIHelpers
    {
        private static bool? _isInDesignMode;

        /// <summary>
        /// Gets a value indicating whether the control is in design mode (running in Blend
        /// or Visual Studio).
        /// </summary>
        public static bool IsInDesignModeStatic
        {
            get
            {
                if (!_isInDesignMode.HasValue)
                {
#if SILVERLIGHT
                    _isInDesignMode = DesignerProperties.IsInDesignTool;
#else
                    var prop = DesignerProperties.IsInDesignModeProperty;
                    _isInDesignMode
                        = (bool) DependencyPropertyDescriptor
                            .FromProperty(prop, typeof (FrameworkElement))
                            .Metadata.DefaultValue;
#endif
                }

                return _isInDesignMode.Value;
            }
        }

        public static FrameworkElement RootUI
        {
            get
            {
#if SILVERLIGHT
                return Application.Current.RootVisual as FrameworkElement;
#else
                return Application.Current.MainWindow;
#endif
            }
        }
    }

}
