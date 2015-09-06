using BabySmash.Core;
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
			Loaded += UserControl_Loaded;

			//TODO: this should be discover
			this.availableShapes = new Dictionary<ShapeType, Action<CanvasControl, CanvasDrawingSession>> {
				{ ShapeType.Square, new  Action<CanvasControl, CanvasDrawingSession>(this.DrawSquare) },
				{ ShapeType.Line, new  Action<CanvasControl, CanvasDrawingSession>(this.DrawLine) },
				{ ShapeType.Rectangle, new  Action<CanvasControl, CanvasDrawingSession>(this.DrawRectangle) },
				{ ShapeType.Circle, new  Action<CanvasControl, CanvasDrawingSession>(this.DrawCircle) },
				{ ShapeType.Triangle, new  Action<CanvasControl, CanvasDrawingSession>(this.DrawTriangle) },
				{ ShapeType.Star, new  Action<CanvasControl, CanvasDrawingSession>(this.DrawStar) },
				{ ShapeType.Hexagon, new  Action<CanvasControl, CanvasDrawingSession>(this.DrawHexagon) },
				{ ShapeType.Trapezoid, new  Action<CanvasControl, CanvasDrawingSession>(this.DrawTrapezoid) },
				{ ShapeType.Oval, new  Action<CanvasControl, CanvasDrawingSession>(this.DrawOval) },
				{ ShapeType.Heart, new  Action<CanvasControl, CanvasDrawingSession>(this.DrawHeart) },
     		};

			//Set default to the Heart shape
			//Todo this should be a setting .. or not,  that should be handle on core.. this is just a control implementation
			this.DefaultShape = this.availableShapes.Last();			
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

		//Someone could set this and reuse it
		public KeyValuePair<ShapeType, Action<CanvasControl, CanvasDrawingSession>> DefaultShape
		{
			get; set;
		}

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

		private Dictionary<ShapeType,Action<CanvasControl,CanvasDrawingSession>> availableShapes;
		private CanvasControl canvas;
		private float defaultStroke
		{
			get
			{
				//use font size to specify the stroke for now.. 
				return (float) FontSize;
			}
		}
		
		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			canvas = new CanvasControl();
			canvas.Draw += OnDraw;
			Content = canvas;
		}

		private void OnDraw(CanvasControl sender, CanvasDrawEventArgs args)
		{
			
			var currentShape = this.availableShapes.FirstOrDefault(s => s.Key == ShapeType);
			if(args.DrawingSession == null)
				currentShape = this.DefaultShape;
			currentShape.Value.Invoke(sender, args.DrawingSession);
		}

		private void DrawHeart(CanvasControl sender, CanvasDrawingSession ds)
		{
			var width = (float) sender.ActualWidth;
			var height = (float) sender.ActualHeight;
			var stroke = this.defaultStroke;
			var scale = (Math.Min(width,height) / 2) + 50;
			var center = new Vector2(width / 2 , height / 2);
		
			var heartGeometry = CreateHeart(sender, scale, center);
			ds.FillGeometry(heartGeometry, ForegroundColor);
			ds.DrawGeometry(heartGeometry, GlowColor, stroke);
		}

		private void DrawTrapezoid(CanvasControl sender, CanvasDrawingSession ds)
		{
			var width = (float) sender.ActualWidth;
			var height = (float) sender.ActualHeight;
			var stroke = this.defaultStroke;
			var scale = Math.Min(width, height) / 2;
			var center = new Vector2(width / 2, height / 2);
		
			var trapezoidGeometry = CreateTrapezoidGeometry(sender, scale, center);
			ds.FillGeometry(trapezoidGeometry, ForegroundColor);
			ds.DrawGeometry(trapezoidGeometry, GlowColor, stroke);
		}
	
		private void DrawHexagon(CanvasControl sender, CanvasDrawingSession ds)
		{
			var width = (float) sender.ActualWidth;
			var height = (float) sender.ActualHeight;
			var stroke = this.defaultStroke;
			var scale = width / 2 - stroke * 2;
			var center = new Vector2(width / 2, height / 2);
			
			var hexagonGeometry = CreateHexagonGeometry(sender, scale, center);
			ds.FillGeometry(hexagonGeometry, ForegroundColor);
			ds.DrawGeometry(hexagonGeometry, GlowColor, this.defaultStroke);
		}

		private void DrawLine(CanvasControl sender, CanvasDrawingSession ds)
		{
			var width = (float) sender.ActualWidth;
			var height = (float) sender.ActualHeight;
			var rnd = Utils.GetRandomBoolean();
			var stroke = this.defaultStroke;

			if(rnd)
				ds.DrawLine(0, 0, width - stroke, height - stroke, ForegroundColor, stroke);
			else
				ds.DrawLine(0, height - stroke, width - stroke, 0, ForegroundColor, stroke);
		}

		private void DrawSquare(CanvasControl sender, CanvasDrawingSession ds)
		{
			var width = (float) sender.ActualWidth;
			var height = (float) sender.ActualHeight;
			var stroke = this.defaultStroke;
			var min = Math.Min(width, height);
			min -=  stroke * 2;

			var rect = new Rect(stroke, stroke, min, min);
			ds.FillRectangle(rect, ForegroundColor);
			ds.DrawRectangle(rect, GlowColor, stroke);
		}

		private void DrawOval(CanvasControl sender, CanvasDrawingSession ds)
		{
			var width = (float) sender.ActualWidth;
			var height = (float) sender.ActualHeight;
			var center = new Vector2(width / 2, height / 2);
			var stroke = this.defaultStroke;
			var radiusX = (width / 3) - stroke;
			var radiusY = (height / 2) - stroke;
			
			ds.FillEllipse(center ,radiusX,radiusY, ForegroundColor);
			ds.DrawEllipse(center, radiusX, radiusY, GlowColor, stroke);
		}

		private void DrawTriangle(CanvasControl sender, CanvasDrawingSession ds)
		{
			var width = (float) sender.ActualWidth;
			var height = (float) sender.ActualHeight;
			var stroke = this.defaultStroke;
			var center =  new Vector2(width / 2, height / 2);
			var scale = (width / 2) - (stroke * 2);

			var triangleGeometry = CreateTriangleGeometry(sender, scale, center);
			ds.FillGeometry(triangleGeometry, ForegroundColor);
			ds.DrawGeometry(triangleGeometry, GlowColor, stroke);
			
		}

		private void DrawStar(CanvasControl sender, CanvasDrawingSession ds)
		{
			var width = (float) sender.ActualWidth;
			var height = (float) sender.ActualHeight;
			var stroke = this.defaultStroke;
			var scale = (width / 2) - (stroke * 2);
			var center = new Vector2(width / 2 , height / 2);
			
			var starGeometry = CreateStarGeometry(sender, scale, center);
			ds.FillGeometry(starGeometry, ForegroundColor);
			ds.DrawGeometry(starGeometry, GlowColor, stroke);
		}

		private void DrawRectangle(CanvasControl sender, CanvasDrawingSession ds)
		{
			var width = (float) sender.ActualWidth;
			var height = (float) sender.ActualHeight;
			var stroke = this.defaultStroke;
			var max = Math.Max(width, height);
			max -= stroke * 2;

			var rect = new Rect(stroke, stroke, max, max - max / 3);
			ds.FillRectangle(rect, ForegroundColor);
			ds.DrawRectangle(rect, GlowColor, stroke);
		}

		private void DrawCircle(CanvasControl sender, CanvasDrawingSession ds)
		{
			var width = (float) sender.ActualWidth;
			var height = (float) sender.ActualHeight;
			var stroke = this.defaultStroke;
			var radius = Math.Min(width, height) / 2 - stroke;
			var center = new Vector2(width / 2, height / 2);

			ds.FillCircle(center, radius, ForegroundColor);
			ds.DrawCircle(center, radius, GlowColor, stroke);
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
				new Vector2(-1,1),
				new Vector2(0,-1),
				new Vector2(1,1)
			};

			var transformedPoints = from point in points
									select point * scale + center;

			var convertedPoints = transformedPoints;

			return CanvasGeometry.CreatePolygon(resourceCreator, convertedPoints.ToArray());
		}

		private static CanvasGeometry CreateTrapezoidGeometry(ICanvasResourceCreator resourceCreator, float scale, Vector2 center)
		{
			Vector2[] points =
			{
				new Vector2(-1,0.7f),
				new Vector2(-0.6f,-0.7f),
				new Vector2(0.6f,-0.7f),
				new Vector2(1,0.7f)
			};

			var transformedPoints = from point in points
									select point * scale + center;

			var convertedPoints = transformedPoints;

			return CanvasGeometry.CreatePolygon(resourceCreator, convertedPoints.ToArray());
		}

		private static CanvasGeometry CreateHeart(ICanvasResourceCreator resourceCreator, float scale, Vector2 center)
		{
			center = center - new Vector2(center.X / 2, center.Y / 2);
			CanvasPathBuilder pathBuilder = new CanvasPathBuilder(resourceCreator);
			var begin = new Vector2(0.47f, 0.34f) * scale + center;
			pathBuilder.BeginFigure(begin);
			pathBuilder.AddCubicBezier(new Vector2(0.66f, 0.19f) * scale + center, new Vector2(0.88f, 0.30f) * scale + center, new Vector2(0.88f, 0.48f) * scale + center);
			pathBuilder.AddCubicBezier(new Vector2(0.88f, 0.66f) * scale + center, new Vector2(0.49f, 1) * scale + center, new Vector2(0.49f, 1)* scale + center);
			pathBuilder.AddCubicBezier(new Vector2(0.49f, 1) * scale + center, new Vector2(0, 0.66f) * scale + center, new Vector2(0, 0.48f) * scale + center);
			pathBuilder.AddCubicBezier(new Vector2(0, 0.30f) * scale + center, new Vector2(0.33f, 0.19f) * scale + center, begin);
			
			pathBuilder.SetSegmentOptions(CanvasFigureSegmentOptions.ForceRoundLineJoin);
            pathBuilder.EndFigure(CanvasFigureLoop.Closed);
			var geometry = CanvasGeometry.CreatePath(pathBuilder);
			return geometry;
		}

		private static CanvasGeometry CreateHexagonGeometry(ICanvasResourceCreator resourceCreator, float scale, Vector2 center)
		{
			Vector2[] points =
			{
				new Vector2(-1,0),
				new Vector2(-0.6f,-1),
				new Vector2(0.6f,-1),
				new Vector2(1,0),
				new Vector2(0.6f,1),
				new Vector2(-0.6f,1),
			};

			var transformedPoints = from point in points
									select point * scale + center;

			var convertedPoints = transformedPoints;

			return CanvasGeometry.CreatePolygon(resourceCreator, convertedPoints.ToArray());
		}
	}
}
