using Plugin.BluetoothLE;
using System;
using System.Collections.Generic;
using System.Text;

namespace rinxly.Model
{
	public class Ring
	{
		public IDevice Device { get; set; }
		public string Name { get; set; }
		public DeviceFeatures Features { get; set; }
		public ConnectionStatus Status { get; set; }
		public PairingStatus PairingStatus { get; set; }
		public bool IsConnectable { get; set; }
		public string LocalName { get; set; }
		public int TxPower { get; set; }
	}
}
