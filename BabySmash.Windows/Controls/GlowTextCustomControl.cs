using BabySmash.Core.Models;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using static BabySmash.Windows.Helpers.Animations;

namespace BabySmash.Windows.Controls
{
	public sealed class GlowTextCustomControl : GlowBaseCustomControl
	{

		public GlowTextCustomControl()
		{
			Loaded += UserControl_Loaded;
		}

		protected override Size MeasureOverride(Size availableSize)
		{
			// CanvasTextLayout cannot cope with infinite sizes, so we change
			// infinite to some-large-value.
			if(double.IsInfinity(availableSize.Width))
				availableSize.Width = 6000;

			if(double.IsInfinity(availableSize.Height))
				availableSize.Height = 6000;

			var device = CanvasDevice.GetSharedDevice();

			var layout = CreateTextLayout(device, availableSize, (float) FontSize);
			var bounds = layout.LayoutBounds;

			return new Size(Math.Min(availableSize.Width, bounds.Width + ExpandAmount), Math.Min(availableSize.Height, bounds.Height + ExpandAmount));
		}

		public override void PropertyChanged()
		{
			if(canvas != null) {
				canvas.Invalidate();
				InvalidateMeasure();
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

		private GlowEffectGraph glowEffectGraph = new GlowEffectGraph();
		private CanvasControl canvas;

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			canvas = new CanvasControl();
			canvas.Draw += OnDraw;
			Content = canvas;
		}
	
		private void OnDraw(CanvasControl sender, CanvasDrawEventArgs args)
		{
			DoEffect(args.DrawingSession, sender.Size, (float) GlowAmount);
		}

		private void DoEffect(CanvasDrawingSession ds, Size size, float amount)
		{
			size.Width = size.Width - ExpandAmount;
			size.Height = size.Height - ExpandAmount;

			var offset = (float) (ExpandAmount / 2);

			using(var textLayout = CreateTextLayout(ds, size, (float) FontSize))
			using(var textCommandList = new CanvasCommandList(ds)) {
				using(var textDs = textCommandList.CreateDrawingSession()) {
					textDs.DrawTextLayout(textLayout, 0, 0, GlowColor);
				}
				if(Settings.Default.UseEffects) {
					glowEffectGraph.Setup(textCommandList, amount);
					ds.DrawImage(glowEffectGraph.Output, offset, offset);
				}

				ds.DrawTextLayout(textLayout, offset, offset, ForegroundColor);
			}
		}

		private CanvasTextLayout CreateTextLayout(ICanvasResourceCreator resourceCreator, Size size, float fontSize)
		{
			var format = new CanvasTextFormat() {
				HorizontalAlignment = GetCanvasHorizontalAlignemnt(),
				VerticalAlignment = GetCanvasVerticalAlignment(),
				FontSize = fontSize,
			};

			return new CanvasTextLayout(
				resourceCreator,
				Text,
				format,
				(float) size.Width,
				(float) size.Height);
		}

		private CanvasHorizontalAlignment GetCanvasHorizontalAlignemnt()
		{
			switch(HorizontalContentAlignment) {
				case HorizontalAlignment.Center:
				return CanvasHorizontalAlignment.Center;

				case HorizontalAlignment.Left:
				return CanvasHorizontalAlignment.Left;

				case HorizontalAlignment.Right:
				return CanvasHorizontalAlignment.Right;

				default:
				return CanvasHorizontalAlignment.Left;
			}
		}

		private CanvasVerticalAlignment GetCanvasVerticalAlignment()
		{
			switch(VerticalContentAlignment) {
				case VerticalAlignment.Center:
				return CanvasVerticalAlignment.Center;

				case VerticalAlignment.Top:
				return CanvasVerticalAlignment.Top;

				case VerticalAlignment.Bottom:
				return CanvasVerticalAlignment.Bottom;

				default:
				return CanvasVerticalAlignment.Top;
			}
		}
	

	}

}
