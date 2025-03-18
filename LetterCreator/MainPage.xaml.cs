namespace LetterCreator
{
    public partial class MainPage : ContentPage // Передделать на навигатион пейдж потому что надо будет открывать выходную pdf
    {
        public LetterVM ViewModel { get; } = new LetterVM();

		public MainPage()
        {
			BindingContext = ViewModel;
            ViewModel.OpenPDF = OpenPDFwindow;
			InitializeComponent();
        }

        public void OpenPDFwindow()
        {
            Navigation.PushAsync(new PDFview());
        }
    }

}
