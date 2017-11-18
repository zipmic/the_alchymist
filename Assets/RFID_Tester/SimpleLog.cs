using UnityEngine;
using System.Collections;
using System.IO.Ports;

public class SimpleLog : MonoBehaviour {

	SerialPort stream = new SerialPort("COM3", 9600);

	int buttonState = 0;

	void Start()
    {
		stream.Open();
        stream.ReadTimeout = 1;
	}

	void Update ()
    {
        var value = stream.ReadLine();
        Debug.Log(value);
	}

	void OnGUI()
    {
		string newString = "Connected: " + buttonState;
		GUI.Label(new Rect(10, 10, 300, 100), newString);
	}
}