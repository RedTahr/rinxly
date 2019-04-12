using Plugin.BluetoothLE;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using rinxly.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace rinxly
{
	// Learn more about making custom code visible in the Xamarin.Forms previewer
	// by visiting https://aka.ms/xamarinforms-previewer
	[DesignTimeVisible(true)]
	public partial class MainPage : ContentPage
	{
	private MainPageViewModel vm = new MainPageViewModel();
		public MainPage()
		{
			InitializeComponent();
			BindingContext = vm;
			GetPermissions();

		}
		
		private async void GetPermissions()
		{
			try
			{
				var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
				if (status != PermissionStatus.Granted)
				{
					if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
					{
						await DisplayAlert("Need location", "Gunna need that location", "OK");
					}

					var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
					//Best practice to always check that the key exists
					if (results.ContainsKey(Permission.Location))
						status = results[Permission.Location];
				}

				if (status != PermissionStatus.Unknown)
				{
					await DisplayAlert("Location Denied", "Can not continue, try again.", "OK");
				}
			}
			catch (Exception ex)
			{

				System.Diagnostics.Debug.WriteLine($"Permissions Error: {ex.Message}");
			}
		}

		private void OnClick_Pair(object sender, EventArgs e) {
			if (ringsListView.SelectedItem is Ring ring) {
				System.Diagnostics.Debug.WriteLine($"Attempting to Pair with {ring.Name}");
				if (ring.PairingStatus == PairingStatus.NotPaired) {
					var pairingRequest = ring.Device.PairingRequest()
						.Subscribe(x => {
							var txt = x ? "Device Paired Successfully" : "Device Pairing Failed";
							DisplayAlert("Pairing attempt", txt, "Ok");
						});
				}
			}
			else {
				DisplayAlert("Nothing selected", "Can't pair to empty space", "Ok");
			}
		}

		private void OnClick_Connect(object sender, EventArgs e) {
			if(ringsListView.SelectedItem is Ring ring) {
				System.Diagnostics.Debug.WriteLine($"Attempting to Connect to {ring.Name}");
				if(ring.IsConnectable) {
					var connectionRequest = ring.Device.Connect();
				}
			}
			else {
				DisplayAlert("Nothing selected", "Can't Connect to empty space", "Ok");
			}
		}
	}
}
