using BabySmash.Core.Models;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using static BabySmash.Windows.Helpers.Animations;

namespace BabySmash.Windows.Controls
{

	public class GlowShapeCustomControl : GlowBaseCustomControl
	{
		public GlowShapeCustomControl()
		{
			this.Width = this.Height = 200;
			Loaded += UserControl_Loaded;

			availableShapes = new List<Shape> {
				new Shape() { Name = "Square", Type = ShapeType.Square,   Drawer = this.DrawSquare        },
				new Shape() { Name = "Line",  Type = ShapeType.Line, Drawer = this.DrawLine             },
				new Shape() { Name = "Rectangle", Type = ShapeType.Rectangle,   Drawer = this.DrawRectangle        },
				new Shape() { Name = "Circle", Type = ShapeType.Circle, Drawer = this.DrawCircles },
				new Shape() { Name = "Oval", Type = ShapeType.Oval, Drawer = this.DrawOval },
				new Shape() { Name = "Triangle",Type = ShapeType.Triangle, Drawer = this.DrawTriangle },
				new Shape() { Name = "Star",Type = ShapeType.Star, Drawer = this.DrawStar},
			};
		}

		#region Properties
		public ShapeType ShapeType
		{
			get
			{
				return (ShapeType) GetValue(ShapeTypeProperty);
			}
			set
			{
				SetValue(ShapeTypeProperty, value);
			}
		}

		public static readonly DependencyProperty ShapeTypeProperty =
			DependencyProperty.Register(
				"ShapeType",
				typeof(ShapeType),
				typeof(GlowShapeCustomControl),
				new PropertyMetadata(ShapeType.Square, new PropertyChangedCallback(OnShapeTypeChanged)));

		private static void OnShapeTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var instance = d as GlowShapeCustomControl;
			if(d == null)
				return;

			instance.PropertyChanged();
		}
		#endregion
		
		public override void PropertyChanged()
		{
			if(canvas != null) {
				canvas.Invalidate();
			}
		}

		public override void Dispose()
		{
			// Explicitly remove references to allow the Win2D controls to get garbage collected
			if(canvas != null) {
				canvas.RemoveFromVisualTree();
				canvas = null;
			}
			base.Dispose();
		}

		private List<Shape> availableShapes;
		private CanvasControl canvas;
		private void OnDraw(CanvasControl sender, CanvasDrawEventArgs args)
		{
			//var currentShape = availableShapes.FirstOrDefault(s => s.Type == ShapeType);

			//if(currentShape == null)
			var currentShape = availableShapes[6];


			currentShape.Drawer(sender, args.DrawingSession);
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			canvas = new CanvasControl();
			canvas.Draw += OnDraw;
			Content = canvas;

		}

		private void DrawLine(CanvasControl sender, CanvasDrawingSession ds)
		{
			var width = (float) sender.ActualWidth;
			var height = (float) sender.ActualHeight;

			var middle = height / 2;
			var color = GradientColor(0.2f);

			ds.DrawLine(0, 0, width, height, color);
		}

		private void DrawSquare(CanvasControl sender, CanvasDrawingSession ds)
		{
			var width = (float) sender.ActualWidth;
			var height = (float) sender.ActualHeight;

			var color = GradientColor(0.2f);

			ds.DrawRectangle(0, 0, width, height, color);
		}

		private void DrawOval(CanvasControl sender, CanvasDrawingSession ds)
		{
			var width = (float) sender.ActualWidth;
			var height = (float) sender.ActualHeight;

			var color = GradientColor(0.2f);

			ds.DrawEllipse(width / 2, height / 2, width / 3, (height / 2), color);
		}

		private void DrawTriangle(CanvasControl sender, CanvasDrawingSession ds)
		{
			var width = (float) sender.ActualWidth;
			var height = (float) sender.ActualHeight;

			var color = GradientColor(0.2f);

			var triangleGeometry = CreateTriangleGeometry(sender, 1, new Vector2(width / 2, height / 2));
			ds.DrawGeometry(triangleGeometry, color);
		}

		private void DrawStar(CanvasControl sender, CanvasDrawingSession ds)
		{
			var width = (float) sender.ActualWidth;
			var height = (float) sender.ActualHeight;

			var color = GradientColor(0.2f);

			var starGeometry = CreateStarGeometry(sender, 100, new Vector2(width / 2, height / 2));
			ds.DrawGeometry(starGeometry, color);
		}

		private void DrawRectangle(CanvasControl sender, CanvasDrawingSession ds)
		{
			var width = (float) sender.ActualWidth;
			var height = (float) sender.ActualHeight;

			var color = GradientColor(0.2f);

			ds.DrawRectangle(0, 0, width, height - height / 3, color, 2.0f);

		}

		private void DrawRoundedRectangle(CanvasControl sender, CanvasDrawingSession ds)
		{
			var width = (float) sender.ActualWidth;
			var height = (float) sender.ActualHeight;

			int steps = Math.Min((int) (width / 30), 10);

			for(int i = 0; i < steps; ++i) {
				var mu = (float) i / steps;

				var color = GradientColor(mu);

				mu *= 0.5f;
				var x = mu * width;
				var y = mu * height;

				var xx = (1 - mu) * width;
				var yy = (1 - mu) * height;

				var radius = mu * 50.0f;

				ds.DrawRoundedRectangle(
					x, y,
					xx - x, yy - y,
					radius, radius,
					color,
					2.0f);
			}
		}

		private void DrawCircles(CanvasControl sender, CanvasDrawingSession ds)
		{
			float width = (float) sender.ActualWidth;
			float height = (float) sender.ActualHeight;

			var color = GradientColor(0.6f);
			var strokeWidth = 1f;
			var radius = Math.Min(width, height) / 2;

			ds.DrawCircle(width / 2, height / 2, radius, color, strokeWidth);
		}

		private static Color GradientColor(float mu)
		{
			byte c = (byte) ((Math.Sin(mu * Math.PI * 2) + 1) * 127.5);

			return Color.FromArgb(255, (byte) (255 - c), c, 220);
		}

		private static CanvasGeometry CreateStarGeometry(ICanvasResourceCreator resourceCreator, float scale, Vector2 center)
		{
			Vector2[] points =
			{
				new Vector2(-0.24f, -0.24f),
				new Vector2(0, -1),
				new Vector2(0.24f, -0.24f),
				new Vector2(1, -0.2f),
				new Vector2(0.4f, 0.2f),
				new Vector2(0.6f, 1),
				new Vector2(0, 0.56f),
				new Vector2(-0.6f, 1),
				new Vector2(-0.4f, 0.2f),
				new Vector2(-1, -0.2f),
			};

			var transformedPoints = from point in points
									select point * scale + center;

			var convertedPoints = transformedPoints;

			return CanvasGeometry.CreatePolygon(resourceCreator, convertedPoints.ToArray());
		}

		private static CanvasGeometry CreateTriangleGeometry(ICanvasResourceCreator resourceCreator, float scale, Vector2 center)
		{
			Vector2[] points =
			{
				new Vector2(0,1),
				new Vector2(0.5f,0),
				new Vector2(1,1)
			};

			var transformedPoints = from point in points
									select point * scale + center;

			var convertedPoints = transformedPoints;

			return CanvasGeometry.CreatePolygon(resourceCreator, convertedPoints.ToArray());
		}

	}

	class Shape
	{
		public string Name
		{
			get; set;
		}

		public ShapeType Type
		{
			get; set;
		}
		public Action<CanvasControl, CanvasDrawingSession> Drawer
		{
			get; set;
		}
	}


}
