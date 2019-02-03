using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stellio_cache_transformer.Helpers
{
	public static class HelpCommandHelper
	{
		private static readonly string HelpCommand = "help";

		public static bool CheckForHelpParam(string[] parameters)
		{
			foreach (string parameter in parameters)
			{
				if (parameter.ToLower().Contains(HelpCommand))
				{
					return true;
				}
			}
			return false;
		}

		public static string GetHelpText()
		{
			return new StringBuilder()
				.AppendLine("-----------------------------------------------------------------")
				.AppendLine("Help for stellio cache transformer")
				.AppendLine("/help - to get help")
				.AppendLine("/inputDir [absolute or relative path] - source of stellio cache files")
				.AppendLine("/outputDir [absolute or relative path] - destination path for mp3 files")
				.AppendLine("Ive made it for free. You can make some reward to me here - https://yasobe.ru/na/stellio_cache_transformer")
				.AppendLine("-----------------------------------------------------------------")
				.ToString();
		}
	}
}
