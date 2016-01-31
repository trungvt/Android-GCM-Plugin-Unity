using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class GCMAndroidBuildSettings : EditorWindow {

	private static string rootPath = Application.dataPath;
	private static string ANDROID_MANIFEST_PATH = "/Plugins/Android/AndroidManifest.xml";

	private string bundleIdentifier = PlayerSettings.bundleIdentifier;
	private static XmlDocument xmlDoc = new XmlDocument();

	[MenuItem("Tools/GCM Android Build Setting")]
	static void Init() {
		GCMAndroidBuildSettings window = (GCMAndroidBuildSettings)EditorWindow.GetWindow(typeof(GCMAndroidBuildSettings));
		window.Show(true);
	}

	void OnGUI() {
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Insert bundle identifier:");
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		bundleIdentifier = EditorGUILayout.TextField(bundleIdentifier);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("");
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Push the Commit button!");
		EditorGUILayout.EndHorizontal();

		if (GUILayout.Button ("Commit")) {
			ConfirmSettings();
		}

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Choose the building option:");
		EditorGUILayout.EndHorizontal();

		if (GUILayout.Button ("Build")) {
			
		}
		
		if (GUILayout.Button ("Build and Run")) {
			
		}
	}

	void ConfirmSettings() {
		XmlNode newAppendNode;
		XmlElement newAppendElement;
		XmlAttribute newAttribute;
		string xmlNamespace = "http://schemas.android.com/apk/res/android";

		PlayerSettings.bundleIdentifier = bundleIdentifier;
		xmlDoc.Load(rootPath + ANDROID_MANIFEST_PATH);
		XmlNamespaceManager nameSpaceMgr = new XmlNamespaceManager(xmlDoc.NameTable);
		nameSpaceMgr.AddNamespace ("android", xmlNamespace);

		// Xml Manifest node
		XmlNode manifestNode = xmlDoc.SelectSingleNode("descendant::manifest", nameSpaceMgr);
		XmlElement packageElement = (XmlElement)manifestNode;
		packageElement.SetAttribute("package", bundleIdentifier);

		// uses-sdk node
		XmlNode usesSdkNode = xmlDoc.SelectSingleNode("descendant::manifest/uses-sdk", nameSpaceMgr);
		if (usesSdkNode != null) {
			XmlElement minSdkElement = (XmlElement)usesSdkNode;
			minSdkElement.SetAttribute("android:minSdkVersion", "8");
		} else {
			newAppendElement = xmlDoc.CreateElement("uses-sdk");
			newAttribute = CreateXmlAttribute(xmlDoc, "android", "minSdkVersion", xmlNamespace, "8");
			newAppendElement.Attributes.Append(newAttribute);
			newAttribute = CreateXmlAttribute(xmlDoc, "android", "targetSdkVersion", xmlNamespace, "17");
			newAppendElement.Attributes.Append(newAttribute);
			newAppendNode = (XmlNode)newAppendElement;
			manifestNode.AppendChild(newAppendNode);
		}

		xmlDoc.Save(rootPath + ANDROID_MANIFEST_PATH);
	}

	XmlAttribute CreateXmlAttribute(XmlDocument xmlDoc , string prefix, string attributeName, string xmlNamespace, string xmlValue) {
		XmlAttribute result = xmlDoc.CreateAttribute (prefix, attributeName, xmlNamespace);
		result.Value = xmlValue;
		return result;
	}
}
