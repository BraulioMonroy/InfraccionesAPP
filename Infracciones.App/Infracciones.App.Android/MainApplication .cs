using Android.App;
using Android.Runtime;
using Plugin.CurrentActivity;
using System;

namespace Infracciones.App.Droid
{
	#if DEBUG
		[Application(Debuggable = true)]
	#else
		[Application(Debuggable = false)]
	#endif
	class MainApplication : Application
    {
		public MainApplication(IntPtr handle, JniHandleOwnership transer) : base(handle, transer)
		{
		}

		public override void OnCreate()
		{
			System.Net.ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) =>
			{
				Console.WriteLine($"****************************************************************************************************");

				return true;
			};
			base.OnCreate();
			CrossCurrentActivity.Current.Init(this);
		}
	}
}