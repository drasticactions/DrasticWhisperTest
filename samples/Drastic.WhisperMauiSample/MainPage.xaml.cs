using Drastic.WhisperSample.ViewModels;

namespace Drastic.WhisperMauiSample;

public partial class MainPage : ContentPage
{
	int count = 0;

	private DebugViewModel vm;

	public MainPage(IServiceProvider provider)
	{
		InitializeComponent();
		this.BindingContext = this.vm = provider.GetRequiredService<DebugViewModel>();
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
	}
}

