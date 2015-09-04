using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabySmash.Core.Services
{
	public interface ISoundService : IDisposable
	{
		Task PlayUrlAsync(Uri url);
		Task PlayEmbebedResourceAsync(string path);
		Task PlayStreamAsync(Stream stream, string mimeType);
	}
}
