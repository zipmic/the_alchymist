using UnityEngine;
using System.Collections;
using System.IO.Ports;


public class SimpleLog : MonoBehaviour {

	SerialPort stream = new SerialPort("/dev/cu.usbmodem1421",9600);

	int buttonState =0;

	void Start(){
		stream.Open();
	}

	void Update (){
		string value = stream.Readline();
		buttonState = int.Parse(value);
	}

	void OnGUI(){

		string newString = "Connected: " + buttonState;
		GUI.Label(new Rect(10,10,300,100), newString);
	}
}