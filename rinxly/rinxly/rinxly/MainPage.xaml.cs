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

namespace rinxly {
	// Learn more about making custom code visible in the Xamarin.Forms previewer
	// by visiting https://aka.ms/xamarinforms-previewer
	[DesignTimeVisible(true)]
	public partial class MainPage : ContentPage {
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

		private void OnClick_Pair(object sender, EventArgs e)
		{
			if (ringsListView.SelectedItem is JewelleryItem jewelleryItem)
			{
				System.Diagnostics.Debug.WriteLine($"Attempting to Pair with {jewelleryItem.Name}");
				if (jewelleryItem.PairingStatus == PairingStatus.NotPaired)
				{
					var pairingRequest = jewelleryItem.Device.PairingRequest()
						.Subscribe(x =>
						{
							var txt = x ? "Device Paired Successfully" : "Device Pairing Failed";
							DisplayAlert("Pairing attempt", txt, "Ok");
						});
				}
			}
			else
			{
				DisplayAlert("Nothing selected", "Can't pair to empty space", "Ok");
			}
		}

		private void OnClick_Unpair(object sender, EventArgs e)
		{
			if (ringsListView.SelectedItem is JewelleryItem jewelleryItem)
			{
				System.Diagnostics.Debug.WriteLine($"Attempting to Unpair with {jewelleryItem.Name}");
				if (jewelleryItem.PairingStatus == PairingStatus.Paired)
				{
				// TODO unpair
				}
			}
			else
			{
				DisplayAlert("Nothing selected", "Can't unpair to empty space", "Ok");
			}
		}

		private void OnClick_Connect(object sender, EventArgs e)
		{
			if (ringsListView.SelectedItem is JewelleryItem jewelleryItem)
			{
				System.Diagnostics.Debug.WriteLine($"Attempting to Connect to {jewelleryItem.Name}");
				
				{
					var connectionRequest = jewelleryItem.Device.Connect();
					var device = jewelleryItem.Device;
					var features = jewelleryItem.Features;
					var isReliableTransactionsAvailable = jewelleryItem.Device.IsReliableTransactionsAvailable();
					if (isReliableTransactionsAvailable)
					{
						var thing = jewelleryItem.Device.BeginReliableWriteTransaction();
					}
					var deviceFeatures = jewelleryItem.Device.Features;
					var mtuAvailable = jewelleryItem.Device.IsMtuRequestAvailable();
					if (mtuAvailable)
					{
						var mtu = jewelleryItem.Device.GetCurrentMtuSize();
					}
					var heartSensor = jewelleryItem.Device.HasHeartSensor();
					var pairingAvailable = jewelleryItem.Device.IsPairingAvailable();
					var deviceName = jewelleryItem.Device.Name;
					var pairintStatus = jewelleryItem.Device.PairingStatus;
					var deviceStatus = jewelleryItem.Device.Status;
				}
			}
			else
			{
				DisplayAlert("Nothing selected", "Can't Connect to empty space", "Ok");
			}
		}

		private void OnClick_Blink(object sender, EventArgs e)
		{
			if (ringsListView.SelectedItem is JewelleryItem jewelleryItem)
			{
				System.Diagnostics.Debug.WriteLine($"Attempting to Blink {jewelleryItem.Name}");


				// TODO blink
			}
			else
			{
				DisplayAlert("Nothing selected", "Can't Blink nothing", "Ok");
			}
		}

		private void OnClick_Vibrate(object sender, EventArgs e)
		{
			if (ringsListView.SelectedItem is JewelleryItem jewelleryItem)
			{
				System.Diagnostics.Debug.WriteLine($"Attempting to Vibrate {jewelleryItem.Name}");
				
				// TODO vibrate
			}
			else
			{
				DisplayAlert("Nothing selected", "Can't Vibrate nothing, this isn't a blackhole", "Ok");
			}
		}
	}
}
