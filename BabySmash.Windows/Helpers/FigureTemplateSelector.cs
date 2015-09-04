using BabySmash.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BabySmash.Windows.Helpers
{
	public class FigureTemplateSelector : DataTemplateSelector
	{
		public DataTemplate TextTemplate
		{
			get; set;
		}
		public DataTemplate ShapeTemplate
		{
			get; set;
		}

		protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
		{
			if(item is LetterFigure || item is NumberFigure)
				return TextTemplate;
			if(item is ShapeFigure)
				return ShapeTemplate;

			return base.SelectTemplateCore(item, container);
		}
	}
}
