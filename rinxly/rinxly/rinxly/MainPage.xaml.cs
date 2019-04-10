using Plugin.BluetoothLE;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
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
		public MainPage()
		{
			InitializeComponent();
			GetPermissions();

			FindSomeBlue();
		}

		private async void FindSomeBlue()
		{
			// discover some devices
			var isScanning = CrossBleAdapter.Current.IsScanning;
			System.Diagnostics.Debug.WriteLine($"IsScanning {isScanning}");
			var status = CrossBleAdapter.Current.Status;
			System.Diagnostics.Debug.WriteLine($"status {status}");
			var features = CrossBleAdapter.Current.Features;
			System.Diagnostics.Debug.WriteLine($"features {features}");

			CrossBleAdapter.Current.Scan().Subscribe(scanResult => { ConsiderScanResult(scanResult); });



			//// Once finding the device/scanresult you want
			//scanResult.Device.Connect();

			//Device.WhenAnyCharacteristicDiscovered().Subscribe(characteristic => {
			//	// read, write, or subscribe to notifications here
			//	var result = await characteristic.Read(); // use result.Data to see response
			//	await characteristic.Write(bytes);

			//	characteristic.EnableNotifications();
			//	characteristic.WhenNotificationReceived().Subscribe(result => {
			//		//result.Data to get at response
			//	});
			//});
		}

		private void ConsiderScanResult(IScanResult scanResult)
		{
			System.Diagnostics.Debug.WriteLine($"ScanResult.Device: {scanResult.Device}");
			System.Diagnostics.Debug.WriteLine($"ScanResult.Device.Name: {scanResult.Device.Name}");
			System.Diagnostics.Debug.WriteLine($"ScanResult.Device.Features: {scanResult.Device.Features}");
			System.Diagnostics.Debug.WriteLine($"ScanResult.Device.Status: {scanResult.Device.Status}");
			System.Diagnostics.Debug.WriteLine($"ScanResult.Device.PairingStatus: {scanResult.Device.PairingStatus}");
			System.Diagnostics.Debug.WriteLine($"ScanResult.AdData: {scanResult.AdvertisementData}");
			System.Diagnostics.Debug.WriteLine($"ScanResult.AdData.IsConnectable: {scanResult.AdvertisementData.IsConnectable}");
			System.Diagnostics.Debug.WriteLine($"ScanResult.AdData.LocalName: {scanResult.AdvertisementData.LocalName}");
			var manuData = scanResult.AdvertisementData.ManufacturerData == null
						? null
						: BitConverter.ToString(scanResult.AdvertisementData.ManufacturerData);
			System.Diagnostics.Debug.WriteLine($"ScanResult.AdData.ManufacturerData: {manuData}");
			System.Diagnostics.Debug.WriteLine($"ScanResult.AdData.ServiceUuids: {scanResult.AdvertisementData.ServiceUuids}");
			System.Diagnostics.Debug.WriteLine($"ScanResult.AdData.TxPower: {scanResult.AdvertisementData.TxPower}");
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
	}
}
