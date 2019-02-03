using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using stellio_cache_transformer.Helpers;

namespace stellio_cache_transformer
{
	public class Program
	{
		public static int Main(string[] args)
		{
			if (HelpCommandHelper.CheckForHelpParam(args))
			{
				Console.Write(HelpCommandHelper.GetHelpText());
				return 0;
			}

			Uri inputDir = InputOutputDirectoriesCommandHelper.GetInputUriFromConsoleArgs(args);
			Uri outputDir = InputOutputDirectoriesCommandHelper.GetOutputUriFromConsoleArgs(args);

			if (!Directory.Exists(inputDir.LocalPath))
			{
				Console.WriteLine("Wrong inputDir");
				return 1;
			}
			if (!Directory.Exists(outputDir.LocalPath))
			{
				Directory.CreateDirectory(outputDir.LocalPath);
			}

			List<string> fileNames =
				Directory.GetFiles(inputDir.LocalPath, "*_*", SearchOption.TopDirectoryOnly)
				.Select(Path.GetFileName).ToList();

			Directory.SetCurrentDirectory(inputDir.LocalPath);

			List<string> exceptions = new List<string>();
			int numberOfFiles = fileNames.Count;
			int currentNumber = 0;
			foreach (string name in fileNames)
			{
				try
				{
					string newName;
					using (Id3.Mp3 mp3 = new Id3.Mp3(name))
					{
						Id3.Id3Tag tag = mp3.GetAllTags().First();
						newName = Normalize($"{tag.Artists} - {tag.Title}.mp3");
					}

					File.Copy(name, newName);

					string outPath = Path.Combine(outputDir.LocalPath, newName);
					if (File.Exists(outPath))
					{
						outPath = Path.Combine(outputDir.LocalPath, $"{name}.mp3");
					}

					Directory.Move(Path.Combine(inputDir.LocalPath, newName), outPath);

					Console.Clear();
					Console.WriteLine($"{++currentNumber} / {numberOfFiles}");
				}
				catch (Exception e)
				{
					exceptions.Add($"{name} - {e.Message}");
				}
			}

			if (exceptions.Any())
			{
				StringBuilder builder = new StringBuilder("Errors, which has occurred during the work");
				foreach(string exception in exceptions)
				{
					builder.AppendLine(exception);
				}

				Console.WriteLine(builder.ToString());
			}

			Console.WriteLine("Done.");
			return 0;
		}

		public static string Normalize(string str)
		{
			return str
				.Where(c =>
					c == '.' ||
					c == ' ' ||
					c == '-' ||
					(c >= 'a' && c <= 'z') ||
					(c >= 'A' && c <= 'Z') ||
					(c >= '0' && c <= '9') ||
					(c >= 'а' && c <= 'я') ||
					(c >= 'А' && c <= 'Я')
				)
				.Aggregate("", (a, c) => a + c);
		}
	}
}