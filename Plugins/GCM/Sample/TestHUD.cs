using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestHUD : MonoBehaviour {

	// test only 382777013196
	private static AndroidJavaObject gcmAndroid;
	private bool isOn = true;
	private bool isSoundOn = true;
	private bool isVibrationOn = true;

	void Init() {
		isOn = GCMManager.GetPushNotificationSetting();
		isSoundOn = GCMManager.GetSoundSetting();
		isVibrationOn = GCMManager.GetVibrationSetting();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI() {
		if (GUI.Button(new Rect(50f, 50f, 100f, 50f), "Initialize GCM")) {
			GCMManager.Init("382777013196", "Test GCM Game", "com.unity3d.player.UnityPlayerProxyActivity");
			GCMManager.SetSoundFileNameSetting("push_sound.wav");
		}

		if (GUI.Button(new Rect(50f, 150f, 300f, 50f), "Push Notification Setting - " + (string)(isOn ? "ON" : "OFF"))) {
			isOn = GCMManager.GetPushNotificationSetting();
			isOn = !isOn;
			GCMManager.PushNotificationSetting(isOn);
			Debug.Log("PUSH NOTIFICATION - SET - " + isOn);
		}

		if (GUI.Button(new Rect(50f, 250f, 300f, 50f), "Sound Setting - " + (string)(isSoundOn ? "ON" : "OFF"))) {
			isSoundOn = GCMManager.GetSoundSetting();
			isSoundOn = !isSoundOn;
			GCMManager.SoundSetting(isSoundOn);
			Debug.Log("SOUND - SET - " + isSoundOn);
		}

		if (GUI.Button(new Rect(50f, 350f, 300f, 50f), "Vibration Setting - " + (string)(isVibrationOn ? "ON" : "OFF"))) {
			isVibrationOn = GCMManager.GetVibrationSetting();
			isVibrationOn = !isVibrationOn;
			GCMManager.VibrationSetting(isVibrationOn);
			Debug.Log("VIBRATION - SET - " + isVibrationOn);
		}
	}
}
