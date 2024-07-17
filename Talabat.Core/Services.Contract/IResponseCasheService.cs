using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Services.Contract
{
	public interface IResponseCasheService
	{
		Task CasheResponseAsync(string key,object response,TimeSpan timeToLive);

		Task<string?> GetCashedResponseAsync(string key);


	}
}
