using BabySmash.Core.Models;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using static BabySmash.Windows.Helpers.Animations;

namespace BabySmash.Windows.Controls
{
	public class GlowBaseCustomControl : UserControl, IDisposable
	{

		public GlowBaseCustomControl()
		{
			Loaded += UserControl_Loaded;
			Unloaded += UserControl_Unloaded;
		}

		public virtual void PropertyChanged()
		{

		}

		public virtual void Dispose()
		{
			this.IsEnabled = false;
		}

		#region Properties

		public string Text
		{
			get
			{
				return (string) GetValue(TextProperty);
			}
			set
			{
				SetValue(TextProperty, value);
			}
		}

		public bool AnimateGlow
		{
			get
			{
				return (bool) GetValue(AnimateGlowProperty);
			}
			set
			{
				SetValue(AnimateGlowProperty, value);
			}
		}

		public Color ForegroundColor
		{
			get
			{
				return (Color) GetValue(ForegroundColorProperty);
			}
			set
			{
				SetValue(ForegroundColorProperty, value);
			}
		}

		public double GlowAmount
		{
			get
			{
				return (double) GetValue(GlowAmountProperty);
			}
			set
			{
				SetValue(GlowAmountProperty, value);
			}
		}

		public double MaxGlowAmount
		{
			get
			{
				return (double) GetValue(MaxGlowAmountProperty);
			}
			set
			{
				SetValue(MaxGlowAmountProperty, value);
			}
		}

		public Color GlowColor
		{
			get
			{
				return (Color) GetValue(GlowColorProperty);
			}
			set
			{
				SetValue(GlowColorProperty, value);
			}
		}

		public static readonly DependencyProperty AnimateGlowProperty =
			DependencyProperty.Register(
				"AnimateGlow",
				typeof(bool),
				typeof(GlowBaseCustomControl),
				new PropertyMetadata(false, new PropertyChangedCallback(OnAnimateGlowChanged)));

		public static readonly DependencyProperty TextProperty =
			DependencyProperty.Register(
				"Text",
				typeof(string),
				typeof(GlowBaseCustomControl),
				new PropertyMetadata("", new PropertyChangedCallback(OnPropertyChanged)));

		public static readonly DependencyProperty ForegroundColorProperty =
			DependencyProperty.Register(
				"ForegroundColor",
				typeof(Color),
				typeof(GlowBaseCustomControl),
				new PropertyMetadata(Colors.White, new PropertyChangedCallback(OnPropertyChanged)));

		public static readonly DependencyProperty GlowAmountProperty =
			DependencyProperty.Register(
				"GlowAmount",
				typeof(double),
				typeof(GlowBaseCustomControl),
				new PropertyMetadata(5.0, new PropertyChangedCallback(OnPropertyChanged)));

		public static readonly DependencyProperty MaxGlowAmountProperty =
			DependencyProperty.Register(
				"MaxGlowAmount",
				typeof(double),
				typeof(GlowBaseCustomControl),
				new PropertyMetadata(5.0, new PropertyChangedCallback(OnPropertyChanged)));


		public static readonly DependencyProperty GlowColorProperty =
			DependencyProperty.Register(
				"GlowColor",
				typeof(Color),
				typeof(GlowBaseCustomControl),
				new PropertyMetadata(Colors.Green, new PropertyChangedCallback(OnPropertyChanged)));
		#endregion
		
		private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var instance = d as GlowBaseCustomControl;
			if(d == null)
				return;

			instance.PropertyChanged();
		}

		private static void OnAnimateGlowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var instance = d as GlowTextCustomControl;
			if(instance == null)
				return;

			if(instance.AnimateGlow && Settings.Default.UseEffects && Settings.Default.UseAnimations) {
				instance.BeginAnimateGlow(instance.GlowAmount, instance.MaxGlowAmount);
			}

		}
	
		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			if(Settings.Default.UseAnimations) {
				AddInitialAnimation();
				if(Settings.Default.FadeAway)
					AddFinalAnimation();
			}
		}

		private void UserControl_Unloaded(object sender, RoutedEventArgs e)
		{
			Dispose();
		}

		private void AddInitialAnimation()
		{
			var tf = new TransformGroup();
			RenderTransform = tf;

			ApplyRandomAnimationEffect(this, new Duration(TimeSpan.FromSeconds(1)));

			var scale = new ScaleTransform { ScaleX = 0, ScaleY = 0, CenterX = 0.5, CenterY = 0.5 };
			tf.Children.Add(scale);
			var sScale = CreateDPAnimation(scale, "ScaleY", new Duration(TimeSpan.FromSeconds(1)), 0, 1);
			CreateDPAnimation(scale, "ScaleX", new Duration(TimeSpan.FromSeconds(1)), 0, 1, false, false, new SineEase(), sScale);
			sScale.Begin();
		}

		private void AddFinalAnimation()
		{
			var sFade = CreateDPAnimation(this, "Opacity", new Duration(TimeSpan.FromSeconds(Settings.Default.FadeAfter)), 1, 0);
			sFade.Begin();
			sFade.Completed += (s,e) => Dispose();
		}

		private void BeginAnimateGlow(double from, double to)
		{
			Storyboard stb = CreateDPAnimation(this, "GlowAmount", new Duration(new TimeSpan(0, 0, 1)), from, to, loop: true, autoReverse: true);
			stb.Begin();
		}
	
		// This is the amount that we grow the desired size by (to account for the glow)
		internal double ExpandAmount
		{
			get
			{
				return Math.Max(GlowAmount, MaxGlowAmount) * 4;
			}
		}

	}

	class GlowEffectGraph
	{
		public ICanvasImage Output
		{
			get
			{
				return blur;
			}
		}

		MorphologyEffect morphology = new MorphologyEffect() {
			Mode = MorphologyEffectMode.Dilate,
			Width = 1,
			Height = 1
		};

		GaussianBlurEffect blur = new GaussianBlurEffect() {
			BlurAmount = 0,
			BorderMode = EffectBorderMode.Soft
		};

		public GlowEffectGraph()
		{
			blur.Source = morphology;
		}

		public void Setup(ICanvasImage source, float amount)
		{
			morphology.Source = source;

			var halfAmount = Math.Min(amount / 2, 100);
			morphology.Width = (int) Math.Ceiling(halfAmount);
			morphology.Height = (int) Math.Ceiling(halfAmount);
			blur.BlurAmount = halfAmount;
		}
	}
}
