using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabySmash.Core.Models
{
	public class BabyShape
	{
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

		public override string AsString
		{
			get
			{
				return Letter.ToString();
			}
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

		public override string AsString
		{
			get
			{
				return Number.ToString();
			}
		}
	}

	public class BabyShapeFigure : BabyShape
	{
	}
}
