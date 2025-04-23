using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvvmGen;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.Maui.Controls.Platform;
using System.Reflection.Metadata;
using Microsoft.Office.Interop.Word;
using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;
using System.Text.Json;


namespace LetterCreator
{
	[ViewModel]
	public partial class LetterVM
	{
		[Property] private string _adress;
		[Property] private string _phone;
		[Property] private string _recipient;
		[Property] private string _recipientRank;
		[Property] private string _theme;
		[Property] private string _text;
		[Property] private string _senderRank;
		[Property] private string _senderFullName;
		private string _templatePath;
		public Action OpenPDF;



		[Command(CanExecuteMethod = nameof(CanSend))]
		private void Send()
		{
			PhoneAttribute phone = new PhoneAttribute();

			if (phone.IsValid(_phone))
			{
				List<string> applications = new List<string>();
				try
				{
					applications = JsonSerializer.Deserialize<List<string>>(File.ReadAllText(Path.Combine(FileSystem.AppDataDirectory, "additions.json")));
				}
				catch (FileNotFoundException)
				{ }
				string appl = applications.Count > 0 ? $"В данном письме содержится {applications.Count} приложений" : string.Empty;
				Dictionary<string, string> data = new Dictionary<string, string>
				{
					{ "<Adress>", _adress },
					{ "<Phone>", _phone },
					{ "<Recipient>", _recipient },
					{ "<RecipRank>", _recipientRank },
					{ "<Theme>", _theme },
					{ "<Text>", _text },
					{ "<Rank>", _senderRank },
					{ "<SenderName>", _senderFullName },
					{ "<Date>", DateTime.Now.ToString("dd.MM.yyyy") },
					{ "<Applications>", appl }

				};
				Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
				Microsoft.Office.Interop.Word.Document srcDoc = word.Documents.Open(_templatePath, ReadOnly: true);
				Microsoft.Office.Interop.Word.Range range = srcDoc.Content;
				Microsoft.Office.Interop.Word.Document newDoc = word.Documents.Add();
				range.Copy();
				newDoc.Content.Paste();
				foreach (var tag in data)
				{
					newDoc.Content.Find.Execute(FindText: tag.Key, ReplaceWith: tag.Value, Replace: WdReplace.wdReplaceAll);
				}
				if (applications.Count != 0)
					foreach (string item in applications)
					{
						int index = applications.IndexOf(item) + 1;
						newDoc.Content.InsertAfter(index.ToString() + ". " + item + "\n");
					}
				try
				{
					newDoc.SaveAs2(FileSystem.Current.AppDataDirectory + "\\res.pdf", FileFormat: WdExportFormat.wdExportFormatPDF);
					File.Delete(Path.Combine(FileSystem.AppDataDirectory, "additions.json"));
					App.Current?.OpenWindow(new Microsoft.Maui.Controls.Window(new PDFview()));

				}
				catch (Exception)
				{
					App.Current?.Windows[0].Page?.DisplayAlert("Error", "Ошибка при сохранении файла", "OK");
				}

				srcDoc.Close();
			}
			else
				App.Current?.Windows[0].Page?.DisplayAlert("Error", "Phone number is not valid", "OK");

		}

		[CommandInvalidate(nameof(Adress), nameof(Phone), nameof(Recipient), nameof(RecipientRank), nameof(Theme), nameof(Text), nameof(SenderRank), nameof(SenderFullName))]
		private bool CanSend() => !string.IsNullOrEmpty(_adress) && !string.IsNullOrEmpty(_phone) && !string.IsNullOrEmpty(_recipient) && !string.IsNullOrEmpty(_theme) && !string.IsNullOrEmpty(_text) && !string.IsNullOrEmpty(_senderRank) && !string.IsNullOrEmpty(_senderFullName);

		partial void OnInitialize()
		{
			CopyFileToAppDataDirectory("template.docx");
		}

		async System.Threading.Tasks.Task CopyFileToAppDataDirectory(string filename)
		{
			// Open the source file
			using Stream inputStream = await FileSystem.Current.OpenAppPackageFileAsync(filename);

			_templatePath = Path.Combine(FileSystem.Current.AppDataDirectory, filename);

			// Copy the file to the AppDataDirectory
			using FileStream outputStream = File.Create(_templatePath);
			await inputStream.CopyToAsync(outputStream);
		}
	}
}
