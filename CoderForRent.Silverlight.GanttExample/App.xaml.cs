/*
 * ********************************
 *  Copyright © 2009. CoderForRent,LLC. All Rights Reserved.  Licensed under the GNU General Public License version 2 (GPLv2) .
 * 
 * */
using System;
using System.Windows;

namespace CoderForRent.Silverlight.GanttExample
{
    public partial class App : Application
    {

        public App()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            
            InitializeComponent();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //Uncomment to use for Generated Column Example
			this.RootVisual = new Page();

			//Uncomment to use the XAML column Example
			//this.RootVisual = new PageWithColumnsDefined();
        }

        private void Application_Exit(object sender, EventArgs e)
        {

        }        
    }
}
