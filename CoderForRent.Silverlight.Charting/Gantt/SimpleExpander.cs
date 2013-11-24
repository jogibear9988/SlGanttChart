/*
 * ********************************
 *  Copyright © 2009. CoderForRent,LLC. All Rights Reserved.  Licensed under the GNU General Public License version 2 (GPLv2) .
 * 
 * */

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace CoderForRent.Charting.Gantt
{
	/// <summary>
	/// This is a control that turs a triangle 90 degrees when clicked.
	/// </summary>
    /// 
    [TemplatePart(Name="CollapseAnimation",Type=typeof(DoubleAnimation)),TemplatePart(Name="ExpandAnimation",Type=typeof(DoubleAnimation))]
    public class SimpleExpander : Control
	{
		#region Dependency Properties
		public static DependencyProperty IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(SimpleExpander), new PropertyMetadata(false));
		public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set
            {
				if (IsExpanded != value)
				{
					SetValue(IsExpandedProperty, value);
					RaiseIsExpandedChanged(EventArgs.Empty);
					SetVisualState(true);
				}
            }
		}
		#endregion

        #region Template Parts

        protected DoubleAnimation CollapseAnimation { get; private set; }
        protected DoubleAnimation ExpandAnimation { get; private set; }
        protected RotateTransform ExpanderRotate { get; private set; }

        #endregion

        #region Properties

        public bool UseAnimation { get; set; }

        #endregion

        #region Events
        public event EventHandler IsExpandedChanged;
        protected void RaiseIsExpandedChanged(EventArgs e)
        {
            if (IsExpandedChanged != null)
                IsExpandedChanged(this, e);
		}
		#endregion

		#region Constructors and overrides

#if !SILVERLIGHT
        static SimpleExpander()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SimpleExpander), new FrameworkPropertyMetadata(typeof(SimpleExpander)));
        }
#endif

        public SimpleExpander()
        {
#if SILVERLIGHT
            this.DefaultStyleKey = typeof(SimpleExpander);
#endif
			this.MouseLeftButtonDown += new MouseButtonEventHandler(SimpleExpander_MouseLeftButtonDown);
            this.Loaded += new RoutedEventHandler(SimpleExpander_Loaded);
		}

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            CollapseAnimation = (DoubleAnimation) GetTemplateChild("CollapseAnimation");
            ExpandAnimation = (DoubleAnimation) GetTemplateChild("ExpandAnimation");
            ExpanderRotate = (RotateTransform) (GetTemplateChild("Triangle") as Polygon).RenderTransform;
        }

    
		#endregion

		#region Private functions
		private void SetVisualState(bool UseTransitions)
        {
			
			if (IsExpanded)
			{
                VisualStateManager.GoToState(this, "Expanded", UseTransitions);
			}
			else
			{
                VisualStateManager.GoToState(this, "Collapsed", UseTransitions);
			}
		}
		#endregion

		#region Event handling function
		private void SimpleExpander_Loaded(object sender, RoutedEventArgs e)
        {

            if (UseAnimation)
            {
                SetVisualState(true);
            }
            else
            {
                if (ExpanderRotate == null)
                    return;

                if (IsExpanded)
                    this.ExpanderRotate.Angle = 90;
                else
                    this.ExpanderRotate.Angle = 0;

            }
        }
        private void SimpleExpander_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			IsExpanded = !IsExpanded;

		}

		#endregion
	}
}
