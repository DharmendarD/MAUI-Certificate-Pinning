using Foundation;
using Microsoft.Maui.Controls;
using Security;
using System;
using TrustKitiOSBinding;

namespace CertificatePinning;

public partial class TrustKitService : NSUrlSessionHandler
{
    public const string TSKConfigurationKey = "TSKConfiguration";

    public TrustKitService() : base()
    {
        NSDictionary trustKitDictionary = (NSDictionary)NSBundle.MainBundle.ObjectForInfoDictionary(TSKConfigurationKey);
        List<NSString> keys = new();
        foreach (NSObject key in trustKitDictionary.Keys)
        {
            keys.Add((NSString)key);
        }
        NSDictionary<NSString, NSObject> value = new(keys.ToArray(), trustKitDictionary.Values);
        TrustKit.InitSharedInstanceWithConfiguration(value);
        TrustOverrideForUrl = new NSUrlSessionHandlerTrustOverrideForUrlCallback(TrustKitCallback);
    }

    private bool TrustKitCallback(NSUrlSessionHandler sender, string url, SecTrust trust)
    {
        Uri uri = new(url);
        TSKTrustDecision decision = TrustKit.SharedInstance().PinningValidator.EvaluateTrust(trust, uri.Host);
        return decision switch
        {
            TSKTrustDecision.ShouldAllowConnection => true,
            TSKTrustDecision.ShouldBlockConnection => false,
            TSKTrustDecision.DomainNotPinned => true,
            _ => true,
        };
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            TrustOverrideForUrl = null;
        }
        base.Dispose(disposing);
    }
}
