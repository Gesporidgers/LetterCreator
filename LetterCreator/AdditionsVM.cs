using MvvmGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LetterCreator
{
	[ViewModel]
	public partial class AdditionsVM
	{
		[Property] bool emptyElements;
		public List<string> additions;
		[Property] string _text;
		[Property] string _navText;
		[Property]private int pageIndex = 0;

		[Command]
		private void Add()
		{
			this.EmptyElements = false;
			additions.Add(this._text);
			PageIndex++;
			this.NavText = $"{pageIndex}/{additions.Count}";
		}
		[Command]
		private void Next()
		{
			additions[pageIndex - 1] = this.Text;
			if (pageIndex == additions.Count)
			{
				additions.Add(string.Empty);
				this.Text = string.Empty;
				PageIndex++;
				this.NavText = $"{pageIndex}/{additions.Count}";
			}
			else
			{
				this.Text = additions[pageIndex];
				PageIndex++;
				this.NavText = $"{pageIndex}/{additions.Count}";
			}
		}

		[Command(CanExecuteMethod = nameof(CanPrev))]
		private void Prev()
		{
			additions[pageIndex - 1] = this.Text;
			this.Text = additions[pageIndex - 2];
			PageIndex--;
			this.NavText = $"{pageIndex}/{additions.Count}";
		}

		[CommandInvalidate(nameof(PageIndex))]
		private bool CanPrev() => pageIndex - 1 > 0;

		partial void OnInitialize()
		{
			emptyElements = true;
			additions = new List<string>();
			_text = string.Empty;
			if (File.Exists(FileSystem.Current.AppDataDirectory + "\\additions.json"))
			{
				EmptyElements = false;
				additions = JsonSerializer.Deserialize<List<string>>(File.ReadAllText(Path.Combine(FileSystem.AppDataDirectory,"additions.json")));
				PageIndex = 1;
				this.NavText = $"{pageIndex}/{additions.Count}";
				this.Text = additions[pageIndex - 1];
			}
		}

		[Command]
		private void SaveAll()
		{
			additions[pageIndex - 1] = this.Text;
			File.WriteAllText(FileSystem.Current.AppDataDirectory+"\\additions.json", JsonSerializer.Serialize(additions));
		}
	}
}
