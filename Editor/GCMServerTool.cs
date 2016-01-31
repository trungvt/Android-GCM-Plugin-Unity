using UnityEngine;
using UnityEditor;
using System;
using System.Text;
using System.Collections;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;

public class GCMServerTool : EditorWindow {

	private const string GOOGLE_GCM_SERVER_URL = "https://android.googleapis.com/gcm/send";

	private string appAPIKey = string.Empty; // test only AIzaSyBcb5rM5KoHidM3fpHZ7lVPtZ7oKUn9qgI
	private string registeredDeviceId = string.Empty;
	private string notificationMessage = string.Empty;
	private string error = string.Empty;
	private string requestResult = string.Empty;

	[MenuItem("Tools/GCM Server Tool")]
	static void Init() {
		GCMServerTool gcmServerToolWindow = (GCMServerTool)EditorWindow.GetWindow(typeof(GCMServerTool));
		gcmServerToolWindow.Show();
	}

	void OnGUI() {
		// Application API KEY
		GUILayout.Label("Required Information", EditorStyles.boldLabel);
		appAPIKey = EditorGUILayout.TextField("App API Key (*)", appAPIKey);
		registeredDeviceId = EditorGUILayout.TextField("Registerd Device Id (*)", registeredDeviceId);
		notificationMessage = EditorGUILayout.TextField("Message Content", notificationMessage);
		EditorGUILayout.LabelField(error, EditorStyles.boldLabel);
		if (GUILayout.Button("Send Message")) {
			error = string.Empty;
			if (string.IsNullOrEmpty(appAPIKey) || string.IsNullOrEmpty(registeredDeviceId)) {
				error = "Required Field is empty!";
				return;
			}
			string postData = PreparePostData(registeredDeviceId, notificationMessage);
			error = SendGCMRequest(appAPIKey, postData);
			requestResult = error;
			if (error.Contains("\"success\":1")) {
				error = "Succeeded!";
			} else {
				error = "Failed...";
			}
		}
		EditorGUILayout.LabelField("Request Result", EditorStyles.boldLabel);
		EditorGUI.TextField(new Rect(0f, 150f, 2000f, 100f), requestResult);
	}

	private string PreparePostData(string registerdId, string message) {
        return "{ \"registration_ids\": [ \"" + registerdId + "\" ], \"data\": {\"tickerText\":\"" + "Test" + "\", \"contentTitle\":\"" + "Test" + "\", \"message\": \"" + message + "\", \"t_id\": \"" + "" + "\"}}";
	}

	private string SendGCMRequest(string apiKey, string postData, string dataContentType = "application/json") {
		ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateServerCertificate);

		byte[] content = Encoding.UTF8.GetBytes(postData);
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GOOGLE_GCM_SERVER_URL);
		request.Method = "POST";
		request.KeepAlive = false;
		request.ContentType = dataContentType;
		request.ContentLength = content.Length;
		request.Headers.Add(String.Format("Authorization: key={0}", apiKey));

		Stream dataStream = request.GetRequestStream();
		dataStream.Write(content, 0, content.Length);
		dataStream.Close();
		try {
			WebResponse response = request.GetResponse();
			HttpStatusCode resStatusCode = ((HttpWebResponse)response).StatusCode;
			if (resStatusCode.Equals(HttpStatusCode.Unauthorized) || resStatusCode.Equals(HttpStatusCode.Forbidden)) {
				return "Unauthorized - need new token";
			} else if (!resStatusCode.Equals(HttpStatusCode.OK)) {
				return "Google GCM Server request was not OK";
			}
			StreamReader resReader = new StreamReader(response.GetResponseStream());
			string responseResult = resReader.ReadToEnd();
			resReader.Close();
			Debug.Log(responseResult);
			return responseResult;
		} catch (Exception e) {
			Debug.Log(e.StackTrace);
		}

		return "Error has been occurred";
	}

	private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslError) {
		return true;
	}
}
