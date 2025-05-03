namespace LetterCreator;

public partial class PDFview : ContentPage
{
	public string path;
	public PDFview()
	{
		InitializeComponent();
		path = FileSystem.Current.AppDataDirectory + "\\res.pdf";
		viewer.Source = path;
		viewer.Reload();
	}

	private void Button_Clicked(object sender, EventArgs e)
	{
		File.Copy(path, "C:\\Users\\konko\\Documents\\res.pdf", true);
	}
}