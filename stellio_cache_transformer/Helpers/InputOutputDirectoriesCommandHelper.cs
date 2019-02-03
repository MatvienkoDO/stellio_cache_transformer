using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stellio_cache_transformer.Helpers
{
	public static class InputOutputDirectoriesCommandHelper
	{
		private static readonly Uri DefaultInputDir = new Uri(Environment.CurrentDirectory);
		private static readonly Uri DefaultOutputDir = new Uri(Path.Combine(Environment.CurrentDirectory, "mp3_files"));

		private static readonly string InputDirCommand = "inputDir";
		private static readonly string OutputDirCommand = "outputDir";

		public static Uri GetInputUriFromConsoleArgs(string[] args)
		{
			string inputDirArgument = GetArgumentOfParameter(args, InputDirCommand);
			Uri inputDir = GetUriFromAbsoluteOrRelativePath(inputDirArgument);
			if(inputDir == null)
			{
				return DefaultInputDir;
			}

			return inputDir;
		}

		public static Uri GetOutputUriFromConsoleArgs(string[] args)
		{
			string outputDirArgument = GetArgumentOfParameter(args, OutputDirCommand);
			Uri outputDir = GetUriFromAbsoluteOrRelativePath(outputDirArgument);
			if (outputDir == null)
			{
				return DefaultOutputDir;
			}

			return outputDir;
		}

		private static Uri GetUriFromAbsoluteOrRelativePath(string path)
		{
			try
			{
				Uri absolute = new Uri(path);
				return absolute;
			}
			catch (Exception e)
			{
			}

			try
			{
				Uri relative = new Uri(Path.Combine(Environment.CurrentDirectory, path));
				return relative;
			}
			catch (Exception e)
			{
			}

			return null;
		}

		private static string GetArgumentOfParameter(string[] args, string parameter)
		{
			for (int i = 0; i < args.Length - 1; i++) //foreach except last
			{
				if (args[i].ToLower().Contains(parameter.ToLower()))
				{
					string probableArgument = args[i + 1];
					if(probableArgument[0] == '/' || probableArgument[0] == '-')
					{
						return null;
					}

					return probableArgument;
				}
			}

			return null;
		}
	}
}
