using Autofac;
using BabySmash.Core.ViewModels;
using BabySmash.Core.Models;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

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

			ViewModel.Shapes.CollectionChanged += Shapes_CollectionChanged;
		}

		private void Shapes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add) {

				var shape = e.NewItems[0] as BabyShape;
				var text = new TextBlock { Text = shape.ToString() };
				text.FontSize = 22;
				//text.Foreground = new SolidColorBrush(global::Windows.UI.Color.FromArgb((byte)shape.FillColor.A, (byte)shape.FillColor.R, (byte)shape.FillColor.G, (byte)shape.FillColor.B));
				text.Foreground = new SolidColorBrush(global::Windows.UI.Colors.Red);

				Canvas.SetTop(text, shape.Position.Y);
				Canvas.SetLeft(text, shape.Position.X);

				figuresCanvas.Children.Add(text);
			}
		}
	}
}
