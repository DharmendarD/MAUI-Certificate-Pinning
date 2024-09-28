namespace CertificatePinning;

public partial class MainPage : ContentPage
{
    HttpClient _httpClient;

    public MainPage()
    {
        InitializeComponent();
        _httpClient = new HttpClient(new TrustKitService());
    }

    private async void EstablishConnectionClicked(object sender, EventArgs e)
    {
        IsBusy = true;
        try
        {
            StatusLabel.IsVisible = false;
            string text = await _httpClient.GetStringAsync("https://in.linkedin.com/in/dharmendar-dhanasekar-993411121?trk=public_profile_samename-profile");
            SampleWebView.Source = new HtmlWebViewSource() { Html = text };
            EstablishConnectionButton.IsVisible = false;
            DisplayAlert("Success!", "Connection Successfully established", "OK");
        }
        catch (Exception ex)
        {
            StatusLabel.IsVisible = true;
            DisplayAlert("Error!", "Connection failed to establish", "OK");
        }
        IsBusy = false;
    }
}
