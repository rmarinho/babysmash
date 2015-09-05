using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BabySmash.Core.Models
{
	public class Figure
	{
		public Figure()
		{
			IsVisible = true;
		}
		public Color FillColor
		{
			get; set;
		}

		public Color StrokeColor
		{
			get; set;
		}

		public Size Size
		{
			get; set;
		}

		public Point Position
		{
			get; set;
		}


		private bool isVisible;

		public bool IsVisible
		{
			get
			{
				return isVisible;
			}
			set
			{
				isVisible = value;
			}
		}


	}

	public class LetterFigure : Figure
	{
		public LetterFigure(char letter) 
		{
			this.Letter = letter;
		}

		public char Letter
		{
			get; set;
		}

		public override string ToString()
		{
			return Letter.ToString();
		}
	}

	public class NumberFigure : Figure
	{
		public NumberFigure(int number)
		{
			this.Number = number;
		}

		public int Number
		{
			get; set;
		}

		public override string ToString()
		{
			return Number.ToString();
		}
	}

	public class ShapeFigure : Figure
	{
		public ShapeFigure()
		{
			Type = Utils.GetRandomShape();
		}

		public ShapeType Type
		{
			get; set;
		}

		public override string ToString()
		{
			return Type.ToString();
		}
	}
}
