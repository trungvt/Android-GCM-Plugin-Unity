using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_ANDROID
public class GCMEventListener : MonoBehaviour {

	public static event Action<string> onRegisterErrorEvent;
	public static event Action<string> onRegisterSucceedEvent;
	public static event Action<string> onMessageDeleteEvent;
	public static event Action<string> onMessageSendErrorEvent;
	public static event Action<string> onMessageSucceedEvent;
	
	void Awake() {
		gameObject.name = this.GetType().ToString();
		//DontDestroyOnLoad(this);
	}

	public void onRegisterError(string result) {
		if (onRegisterErrorEvent != null) {
			onRegisterErrorEvent(result);
		}
	}

	public void onRegisterSucceed(string result) {
		if (onRegisterSucceedEvent != null) {
			onRegisterSucceedEvent(result);
		}
	}

	public void onMessageDelete(string result) {
		if (onMessageDeleteEvent != null) {
			onMessageDeleteEvent(result);
		}
	}

	public void onMessageSendError(string result) {
		if (onMessageSendErrorEvent != null) {
			onMessageSendErrorEvent(result);
		}
	}

	public void onMessageSucceed(string result) {
		if (onMessageSucceedEvent != null) {
			onMessageSucceedEvent(result);
		}
	}
}
#endif
