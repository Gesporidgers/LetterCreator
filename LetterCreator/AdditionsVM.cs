using MvvmGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
		private int index = 0;

		[Command]
		private void Add()
		{
			this.EmptyElements =false;
			additions.Add(this._text);
			index++;
			this.NavText = $"{index}/{additions.Count}";
		}
		
		partial void OnInitialize()
		{
			emptyElements = true;
			additions = new List<string>();
			_text = string.Empty;
		}
	}
}
