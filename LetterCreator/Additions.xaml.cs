

namespace LetterCreator;

public partial class Additions : ContentPage
{
	private AdditionsVM vm;
	public Additions()
	{
		vm = new AdditionsVM();
		BindingContext = vm;
		InitializeComponent();
	}
}