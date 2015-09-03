using Autofac;
using BabySmash.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace BabySmash.Windows.Views
{
	public sealed partial class SettingsView : UserControl
	{
		private readonly ILifetimeScope scope;

		public SettingsViewModel ViewModel
		{
			get
			{
				return this.DataContext as SettingsViewModel;
			}
		}
		
		public SettingsView()
		{
			this.InitializeComponent();
			this.Unloaded += SettingsViewUnloaded;
			
			this.scope = BabySmash.Core.App.Container.BeginLifetimeScope();
			DataContext = scope.Resolve<SettingsViewModel>();
		
		}

		private void SettingsViewUnloaded(object sender, global::Windows.UI.Xaml.RoutedEventArgs e)
		{
			this.scope.Dispose();
		}
	}
}
