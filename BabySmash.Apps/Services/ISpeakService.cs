using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabySmash.Core.Services
{
	public interface ISpeakService
	{
		Task SpeakText(string text);
		Task SpeakUriStream(Uri url);
	}
}
