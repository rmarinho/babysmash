using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BabySmash.Core.Models
{
	public class BabyShape
	{
		public Color FillColor
		{
			get; set;
		}

		public Color BorderColor
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

		public virtual string AsString
		{
			get;
		}
		
	}

	public class BabyShapeLetter : BabyShape
	{
		public BabyShapeLetter(char letter)
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

	public class BabyShapeNumber : BabyShape
	{
		public BabyShapeNumber(int number)
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

	public class BabyShapeFigure : BabyShape
	{
		public BabyShapeFigure()
		{
			ShapeName = Utils.GetRandomShape();
		}

		public string ShapeName
		{
			get; set;
		}

		public override string ToString()
		{
			return ShapeName;
		}
	}
}
