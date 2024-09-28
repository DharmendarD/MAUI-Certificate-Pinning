using Javax.Net.Ssl;
using Xamarin.Android.Net;

namespace CertificatePinning;

public partial class TrustKitService : AndroidMessageHandler
{

    public TrustKitService() : base()
    {
        Com.Datatheorem.Android.Trustkit.TrustKit.InitializeWithNetworkSecurityConfiguration(Platform.AppContext, Resource.Xml.network_security_config);
    }

    protected override SSLSocketFactory ConfigureCustomSSLSocketFactory(HttpsURLConnection connection)
    {
        return Com.Datatheorem.Android.Trustkit.TrustKit.Instance.GetSSLSocketFactory(connection.URL.Host);
    }
}
