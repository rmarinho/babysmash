using Autofac;
using BabySmash.Core.ViewModels;
using BabySmash.Core.Models;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using System.Globalization;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BabySmash.Windows
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		private readonly ILifetimeScope scope;

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
			this.Unloaded += MainPageUnloaded;
			
			this.scope = BabySmash.Core.App.Container.BeginLifetimeScope();
			DataContext = scope.Resolve<MainViewModel>();
		
		}

		private void MainPageUnloaded(object sender, global::Windows.UI.Xaml.RoutedEventArgs e)
		{
			this.scope.Dispose();
		}
	}
}