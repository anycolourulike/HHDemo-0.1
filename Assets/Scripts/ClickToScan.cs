using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToScan : MonoBehaviour
{
	public void OnScanClick()
	{
		BluetoothLEHardwareInterface.Initialize(true, false, () => {

			FoundDeviceListScript.DeviceAddressList = new List<DeviceObject>();

			BluetoothLEHardwareInterface.ScanForPeripheralsWithServices(null, (address, name) => {

				FoundDeviceListScript.DeviceAddressList.Add(new DeviceObject(address, name));

			}, null);

		}, (error) => {

			BluetoothLEHardwareInterface.Log("BLE Error: " + error);

		});
	}
}
