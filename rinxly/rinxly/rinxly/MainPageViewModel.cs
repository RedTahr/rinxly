using Plugin.BluetoothLE;
using rinxly.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace rinxly {
	public class MainPageViewModel {
		public ObservableCollection<JewelleryItem> JewelleryItems { get; set; } = new ObservableCollection<JewelleryItem>();

		public MainPageViewModel() {

			FindSomeBlue();
		}

		private async void FindSomeBlue() {
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
		private void ConsiderScanResult(IScanResult scanResult) {
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

			var ring = new JewelleryItem
			{
				Device = scanResult.Device,
				Name = scanResult.Device.Name ?? "",
				Features = scanResult.Device.Features,
				Status = scanResult.Device.Status,
				PairingStatus = scanResult.Device.PairingStatus,
				IsConnectable = scanResult.AdvertisementData.IsConnectable,
				LocalName = scanResult.AdvertisementData.LocalName,
				TxPower = scanResult.AdvertisementData.TxPower,
			};

			if(!JewelleryItems.Any(r => r.Name == ring.Name && r.Device == ring.Device)) {
				JewelleryItems.Add(ring);
							   
				// subscribe to discoveriable things.
			}
		}
	}
}
