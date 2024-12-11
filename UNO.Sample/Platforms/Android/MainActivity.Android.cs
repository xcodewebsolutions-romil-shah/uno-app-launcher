using Android.App;
using Android.Views;
using Android.Content;
using Android.OS;

namespace UNO.Sample.Droid;
[Activity(
    MainLauncher = true,
    ConfigurationChanges = global::Uno.UI.ActivityHelper.AllConfigChanges,
    WindowSoftInputMode = SoftInput.AdjustNothing | SoftInput.StateHidden
)]
[IntentFilter(new[] { Intent.ActionView }, DataScheme = "https", DataHost = "xws.uno.qr.deeplinking.sample.com", DataPathPrefix = "/", AutoVerify = true, Categories = new[] { Intent.ActionView, Intent.CategoryDefault, Intent.CategoryBrowsable })]
public class MainActivity : Microsoft.UI.Xaml.ApplicationActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        string deepLinkUrl = Intent?.DataString ?? "";
        if (!string.IsNullOrEmpty(deepLinkUrl))
        {
            HandleDeepLinking();
        }
    }

    public void HandleDeepLinking()
    {
        //CREATE NEW INTENT/APPLICATION ACTIVITY
        Intent intent = new Intent(this, typeof(MainActivity));
        intent.AddFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask | ActivityFlags.ClearTask);

        //FINISH/STOP EXISTING INTENT/APPLICATION ACTIVITY
        Finish();

        //START THE NEW ACTIVITY/APPLICATION
        StartActivity(intent);

        //KILL THE PREVIOUS PROCESS
        Process.KillProcess(Process.MyPid());
    }
}
