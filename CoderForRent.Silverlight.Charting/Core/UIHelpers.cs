using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

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

        public static FrameworkElement RootUI(this FrameworkElement element)
        {

#if SILVERLIGHT
            return Application.Current.RootVisual as FrameworkElement;
#else
            return element.TryFindRoot();
#endif
        }

#if !SILVERLIGHT
        public static FrameworkElement TryFindRoot(this FrameworkElement child)
        {
            var ch = child;
            var old = child;

            while (ch!=null)
            {
                old = ch;
                ch = ch.TryFindParent<FrameworkElement>();
            }

            return old;
        }

        public static T TryFindParent<T>(this DependencyObject child) where T : class
        {
            //get parent item
            DependencyObject parentObject = GetParentObject(child);

            //we've reached the end of the tree
            if (parentObject == null) return null;

            //check if the parent matches the type we're looking for
            T parent = parentObject as T;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                //use recursion to proceed with next level
                return TryFindParent<T>(parentObject);
            }
        }

        public static DependencyObject GetParentObject(this DependencyObject child)
        {
            if (child == null) return null;

            //handle content elements separately
            ContentElement contentElement = child as ContentElement;
            if (contentElement != null)
            {
                DependencyObject parent = ContentOperations.GetParent(contentElement);
                if (parent != null) return parent;

                FrameworkContentElement fce = contentElement as FrameworkContentElement;
                return fce != null ? fce.Parent : null;
            }

            //also try searching for parent in framework elements (such as DockPanel, etc)
            FrameworkElement frameworkElement = child as FrameworkElement;
            if (frameworkElement != null)
            {
                DependencyObject parent = frameworkElement.Parent;
                if (parent != null) return parent;
            }

            //if it's not a ContentElement/FrameworkElement, rely on VisualTreeHelper
            return VisualTreeHelper.GetParent(child);
        }
#endif
    }
}
