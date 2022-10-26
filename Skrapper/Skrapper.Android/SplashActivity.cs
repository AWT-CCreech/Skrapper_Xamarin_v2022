using Android.App;
using Android.OS;
using Skrapper.Droid;
using Xamarin.Forms;

namespace ScanMaster_Xamarin_v2022.Droid
{
    [Activity(Label = "Skrapper", Theme = "@style/SplashTheme", Icon = "@mipmap/Skrapper_Launcher", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Forms.Init(this, savedInstanceState);
            FormsMaterial.Init(this, savedInstanceState);
            //this.StartActivity(typeof(MainActivity));
            StartActivity(typeof(MainActivity));
        }
    }
}