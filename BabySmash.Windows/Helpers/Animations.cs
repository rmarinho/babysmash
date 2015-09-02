using BabySmash.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace BabySmash.Windows.Helpers
{
	internal static class Animations
	{
		public static Storyboard CreateDPAnimation(DependencyObject shape, string dp, Duration duration, double from, double to, bool loop = false, bool autoReverse = false, EasingFunctionBase easing = null, Storyboard st = null)
		{
			if(st == null)
				st = new Storyboard();
			var d = new DoubleAnimation {
				From = from,
				To = to,
				Duration = duration,
				EasingFunction = easing,
				AutoReverse = autoReverse
			};

			if(loop)
				d.RepeatBehavior = RepeatBehavior.Forever;

			st.Children.Add(d);
			Storyboard.SetTarget(d, shape);
			Storyboard.SetTargetProperty(d, dp);
			return st;
		}

		public static void ApplyRandomAnimationEffect(FrameworkElement fe, Duration duration)
		{
			int e = Utils.RandomBetweenTwoNumbers(0, 3);
			switch(e) {
				case 0:
				ApplyJiggle(fe, duration);
				break;
				case 1:
				ApplySnap(fe, duration);
				break;
				case 2:
				ApplyThrob(fe, duration);
				break;
				case 3:
				ApplyRotate(fe, duration);
				break;
			}
		}

		public static void ApplyRotate(FrameworkElement fe, Duration duration)
		{
			fe.RenderTransformOrigin = new Point(0.5, 0.5);
			fe.RenderTransform = new RotateTransform ();

			var storyboard = CreateDPAnimation(fe.RenderTransform, "Angle", duration, 0, 360, false, false, new BounceEase { Bounces = 2, Bounciness = 5 });

			storyboard.Begin();
		}

		public static void ApplyJiggle(FrameworkElement fe, Duration duration)
		{
			fe.RenderTransformOrigin = new Point(0.5, 0.5);
			fe.RenderTransform = new RotateTransform ();

			var storyboard = CreateDPAnimation(fe.RenderTransform, "Angle", duration, 0, 20, false, false, new ElasticEase { Oscillations = 5 });
			
			storyboard.Begin();
		}

		public static void ApplySnap(FrameworkElement fe, Duration duration)
		{
			fe.RenderTransformOrigin = new Point(0.5, 0.5);
			fe.RenderTransform = new ScaleTransform { ScaleY = 1, ScaleX = 1 };

			var storyboard = CreateDPAnimation(fe.RenderTransform, "ScaleY", duration, 0, 1, false, false, new ElasticEase {  Springiness = 0.4 });

			storyboard.Begin();
		}

		public static void ApplyThrob(FrameworkElement fe, Duration duration)
		{
			fe.RenderTransformOrigin = new Point(0.5, 0.5);
			fe.RenderTransform = new ScaleTransform { ScaleY = 1, ScaleX = 1 };

			var storyboard = CreateDPAnimation(fe.RenderTransform, "ScaleY", duration, 0.95, 1, false, false, new ElasticEase {  Springiness = 0.4 });
			CreateDPAnimation(fe.RenderTransform, "ScaleX", duration, 0.95, 1, false, false, new ElasticEase { Springiness = 0.4 }, storyboard);

			storyboard.Begin();
		}
	}
}