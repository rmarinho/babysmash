﻿using Autofac;
using BabySmash.Core.ViewModels;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BabySmash.Windows
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		public MainViewModel ViewModel
		{
			get
			{
				return this.DataContext as MainViewModel;
			}
		}
		public MainPage()
		{
			this.InitializeComponent();
			var scope = BabySmash.Core.App.Container.BeginLifetimeScope();
			// Resolve services from a scope that is a child
			// of the root container.
			DataContext = scope.Resolve<MainViewModel>();
		}

	}
}
