using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechTweaking.Bluetooth;
using UnityEngine.UI;
using TMPro;

public class MyDevice : MonoBehaviour {

	public string MAC;
	public BluetoothDevice device;
	public Text statusText;

	// Use this for initialization
	void Awake () {
		device = new BluetoothDevice();
		prepareBluetooth ();
	}



	public void prepareBluetooth() {
		print ("Preparing Bluetooth");
//		connect ();
		if (BluetoothAdapter.isBluetoothEnabled ()) {
			print ("Bluetooth is enabled");
			statusText.text = "Are we connecting?";
			connect ();
		} else {
			print ("Please enable Bluetooth");
			BluetoothAdapter.enableBluetooth(); //you can by this force enabling Bluetooth without asking the user
			statusText.text = "Please Enable Bluetooth";
		
			BluetoothAdapter.OnBluetoothStateChanged += HandleOnBluetoothStateChanged;
			BluetoothAdapter.listenToBluetoothState (); // if you want to listen to the following two events  OnBluetoothOFF or OnBluetoothON

			BluetoothAdapter.askEnableBluetooth ();//Ask user to enable Bluetooth
		}
	
	}

	private void connect () {
		print (MAC);
		statusText.text = "Trying To Connect to " + MAC;


		/* The Property device.MacAdress doesn't require pairing. 
		 * Also Mac Adress in this library is Case sensitive,  all chars must be capital letters
		 */
		device.MacAddress = MAC;

		/* device.Name = "My_Device";
		* 
		* Trying to identefy a device by its name using the Property device.Name require the remote device to be paired
		* but you can try to alter the parameter 'allowDiscovery' of the Connect(int attempts, int time, bool allowDiscovery) method.
		* allowDiscovery will try to locate the unpaired device, but this is a heavy and undesirable feature, and connection will take a longer time
		*/


		/*
		* 10 equals the char '\n' which is a "new Line" in Ascci representation, 
		* so the read() method will retun a packet that was ended by the byte 10. simply read() will read lines.
		* If you don't use the setEndByte() method, device.read() will return any available data (line or not), then you can order them as you want.
		*/
		device.setEndByte (10);


		/*
		 * The ManageConnection Coroutine will start when the device is ready for reading.
		 */
//		device.ReadingCoroutine = ManageConnection;

		device.normal_connect (true, false);

	}


	//############### Handlers/Recievers #####################
	void HandleOnBluetoothStateChanged (bool isBtEnabled)
	{
		if (isBtEnabled) {
			connect ();
			//We now don't need our recievers
			BluetoothAdapter.OnBluetoothStateChanged -= HandleOnBluetoothStateChanged;
			BluetoothAdapter.stopListenToBluetoothState ();
		}
	}

	//This would mean a failure in connection! the reason might be that your remote device is OFF
	void HandleOnDeviceOff (BluetoothDevice dev)
	{
		if (!string.IsNullOrEmpty (dev.Name)) {
			statusText.text = "Can't connect to '" + dev.Name;
		} else if (!string.IsNullOrEmpty (dev.MacAddress)) {
			statusText.text = "Can't connect to '" + dev.MacAddress;
		}
	}

	//Because connecting using the 'Name' property is just searching, the Plugin might not find it!.
	void HandleOnDeviceNotFound (BluetoothDevice dev)
	{
		if (!string.IsNullOrEmpty (dev.Name)) {
			statusText.text = "Can't find a device with the name '" + dev.Name + "', device might be OFF or not paird yet ";

		} 
	}

	public void disconnect ()
	{
		if (device != null)
			device.close ();
	}


	public void sendMSG() {
		if (device.IsReading) {
			statusText.text = "Device is reading";
			byte[] outMsg = System.Text.Encoding.UTF8.GetBytes ("f");
			device.send (outMsg);
		}
	}

	public bool readMsg() {
		if (device.IsDataAvailable) {
			statusText.text = "Device is sending";
			device.read ();
			return true;
		}	
		return false;
	}

	IEnumerator  ReadConnection (BluetoothDevice device){
		while (device.IsReading) {
			byte[] outMsg = System.Text.Encoding.UTF8.GetBytes ("f");
			device.send (outMsg);
			statusText.text = "Sent Message " + outMsg.ToString (); 

			if (device.IsDataAvailable) {
				statusText.text = "Status: Phase 2";
				byte[] msg = device.read ();//because we called setEndByte(10)..read will always return a packet excluding the last byte 10.

				//				if (msg != null && msg.Length > 0) {
				//					string content = System.Text.ASCIIEncoding.ASCII.GetString (msg);
				//					messageText.text = "MSG : " + content + " " + content.Length;
			}
		}
		yield return null;
	}


	//############### Reading Data  #####################
	//Please note that you don't have to use this Couroutienes/IEnumerator, you can just put your code in the Update() method
	IEnumerator  SendConnection (BluetoothDevice device) {
		while (device.IsReading) {
			byte[] outMsg = System.Text.Encoding.UTF8.GetBytes ("f");
			device.send (outMsg);
			statusText.text = "Sent Message " + outMsg.ToString (); 

			if (device.IsDataAvailable) {
				statusText.text = "Status: Phase 2";
				byte[] msg = device.read ();//because we called setEndByte(10)..read will always return a packet excluding the last byte 10.

				//				if (msg != null && msg.Length > 0) {
				//					string content = System.Text.ASCIIEncoding.ASCII.GetString (msg);
				//					messageText.text = "MSG : " + content + " " + content.Length;
			}
		}
		yield return null;
//		while (device.IsReading) {
//			byte[] outMsg = System.Text.Encoding.UTF8.GetBytes ("f");
//			device.send (outMsg);
//			statusText.text = "Sent Message " + outMsg.ToString (); 
//
//			if (device.IsDataAvailable) {
//				statusText.text = "Status: Phase 2";
//				byte[] msg = device.read ();//because we called setEndByte(10)..read will always return a packet excluding the last byte 10.
//
////				if (msg != null && msg.Length > 0) {
////					string content = System.Text.ASCIIEncoding.ASCII.GetString (msg);
////					messageText.text = "MSG : " + content + " " + content.Length;
//				}
//			}
//			yield return null;
	}


	//############### Deregister Events  #####################
	void OnDestroy ()
	{
		BluetoothAdapter.OnDeviceOFF -= HandleOnDeviceOff;
		BluetoothAdapter.OnDeviceNotFound -= HandleOnDeviceNotFound;

	}


	public bool isConnected(){
		return device.IsConnected;
	}

	// Update is called once per frame
	void Update () {
		if (device.IsConnected) {
			statusText.text = "Connected";
		} else {
			statusText.text = "Not Connected";
		}
	}
}
