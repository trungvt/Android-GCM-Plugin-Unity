using UnityEngine;
using System.Collections;

#if UNITY_ANDROID
public class GCMManager : MonoBehaviour {

	private static AndroidJavaObject gcmAndroid;
	
	void Start () {
		GCMEventListener.onRegisterErrorEvent += RegisterError;
		GCMEventListener.onRegisterSucceedEvent += RegisterSucceed;
		GCMEventListener.onMessageDeleteEvent += MessageDelete;
		GCMEventListener.onMessageSendErrorEvent += MessageSendError;
		GCMEventListener.onMessageSucceedEvent += MessageSucceed;
	}

	void OnDisable () {
		GCMEventListener.onRegisterErrorEvent -= RegisterError;
		GCMEventListener.onRegisterSucceedEvent -= RegisterSucceed;
		GCMEventListener.onMessageDeleteEvent -= MessageDelete;
		GCMEventListener.onMessageSendErrorEvent -= MessageSendError;
		GCMEventListener.onMessageSucceedEvent -= MessageSucceed;
	}

	public static void Init(string senderId, string notificationTitle, string mainActivityConfig) {
		if (gcmAndroid == null) {
			gcmAndroid = new AndroidJavaObject("com.gcm.unity.GCMController");
		}
		gcmAndroid.Call("Init", senderId, notificationTitle, mainActivityConfig);
	}

	public static void PushNotificationSetting(bool isOn) {
		if (gcmAndroid == null) {
			gcmAndroid = new AndroidJavaObject("com.gcm.unity.GCMController");
		}
		gcmAndroid.Call("SetPushNotificationSetting", isOn);
	}

	public static bool GetPushNotificationSetting() {
		if (gcmAndroid == null) {
			gcmAndroid = new AndroidJavaObject("com.gcm.unity.GCMController");
		}
		bool setting = gcmAndroid.Call<bool>("GetPushNotificationSetting");
		Debug.Log ("CHECK PUSH SETTING - " + setting);
		return setting;
	}

	public static void SoundSetting(bool isPlay) {
		if (gcmAndroid == null) {
			gcmAndroid = new AndroidJavaObject("com.gcm.unity.GCMController");
		}
		gcmAndroid.Call("SetSoundSetting", isPlay);
	}

	public static void SetSoundFileNameSetting(string soundFileName) {
		if (gcmAndroid == null) {
			gcmAndroid = new AndroidJavaObject("com.gcm.unity.GCMController");
		}
		gcmAndroid.Call("SetSoundFileNameSetting", soundFileName);
	}

	public static bool GetSoundSetting() {
		if (gcmAndroid == null) {
			gcmAndroid = new AndroidJavaObject("com.gcm.unity.GCMController");
		}
		bool setting = gcmAndroid.Call<bool>("GetSoundSetting");
		Debug.Log ("CHECK SOUND SETTING - " + setting);
		return setting;
	}

	public static void VibrationSetting(bool isOn) {
		if (gcmAndroid == null) {
			gcmAndroid = new AndroidJavaObject("com.gcm.unity.GCMController");
		}
		gcmAndroid.Call("SetVibrationSetting", isOn);
	}

	public static bool GetVibrationSetting() {
		if (gcmAndroid == null) {
			gcmAndroid = new AndroidJavaObject("com.gcm.unity.GCMController");
		}
		bool setting = gcmAndroid.Call<bool>("GetVibrationSetting");
		Debug.Log ("CHECK VIBRATION SETTING - " + setting);
		return setting;
	}

	void RegisterError(string result) {
		// TODO: send request to register again
		Debug.Log("Error: " + result);
	}

	void RegisterSucceed(string result) {
		// TODO: send regId to application server for archiving
		Debug.Log ("Registration ID: " + result);
	}

	void MessageDelete(string result) {
		// TODO: application server should send a new message
		Debug.Log (result);
	}

	void MessageSendError(string result) {
		// TODO: message has not been sent correctly... anyway
		Debug.Log ("Error: " + result);
	}

	void MessageSucceed(string result) {
		// TODO: enjoy yourself!!!
		Debug.Log ("Message: " + result);
	}
}
#endif
