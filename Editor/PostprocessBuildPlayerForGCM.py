#!/usr/bin python
# -*- coding:utf-8 -*-

import sys
import os.path
from xml.dom.minidom import parse, parseString

ANDROID_MANIFEST_PATH = 'Assets/Plugins/Android/AndroidManifest.xml'

def post_config_android():
	doc = parse(ANDROID_MANIFEST_PATH)
	
	# get the package name
	manifestTag = doc.getElementsByTagName('manifest')[0]
	packageName = manifestTag.getAttribute('package')
	# check the uses-permission tag
	hasInternet = False
	hasAccounts = False
	hasWakelock = False
	hasReceiver = False
	hasC2DMessage = False
	hasVibration = False
	for perTag in doc.getElementsByTagName('uses-permission'):
		if perTag.getAttribute('android:name') == 'android.permission.INTERNET':
			hasInternet = True
		elif perTag.getAttribute('android:name') == 'android.permission.GET_ACCOUNTS':
			hasAccounts = True
		elif perTag.getAttribute('android:name') == 'android.permission.WAKE_LOCK':
			hasWakelock = True
		elif perTag.getAttribute('android:name') == 'com.google.android.c2dm.permission.RECEIVE':
			hasReceiver = True
		elif perTag.getAttribute('android:name') == 'android.permission.VIBRATE':
			hasVibration = True
		elif '.permission.C2D_MESSAGE' in perTag.getAttribute('android:name'):
			# has this uses-permission
			hasC2DMessage = True
			perTag.setAttribute('android:name', packageName + '.permission.C2D_MESSAGE')

	if hasInternet is False:
		newInternet = doc.createElement('uses-permission')
		doc.childNodes[0].appendChild(newInternet)
		newInternet.setAttribute('android:name', 'android.permission.INTERNET')
		newInternet.setIdAttribute('android:name')
	if hasAccounts is False:
		newAccounts = doc.createElement('uses-permission')
		doc.childNodes[0].appendChild(newAccounts)
		newAccounts.setAttribute('android:name', 'android.permission.GET_ACCOUNTS')
		newAccounts.setIdAttribute('android:name')
	if hasWakelock is False:
		newWakelock = doc.createElement('uses-permission')
		doc.childNodes[0].appendChild(newWakelock)
		newWakelock.setAttribute('android:name', 'android.permission.WAKE_LOCK')
		newWakelock.setIdAttribute('android:name')
	if hasReceiver is False:
		newReceiver = doc.createElement('uses-permission')
		doc.childNodes[0].appendChild(newReceiver)
		newReceiver.setAttribute('android:name', 'com.google.android.c2dm.permission.RECEIVE')
		newReceiver.setIdAttribute('android:name')
	if hasC2DMessage is False:
		newC2DMessage = doc.createElement('uses-permission')
		doc.childNodes[0].appendChild(newC2DMessage)
		newC2DMessage.setAttribute('android:name', packageName + '.permission.C2D_MESSAGE')
		newC2DMessage.setIdAttribute('android:name')

	if hasVibration is False:
		newVibration = doc.createElement('uses-permission')
		doc.childNodes[0].appendChild(newVibration)
		newVibration.setAttribute('android:name', 'android.permission.VIBRATE')
		newVibration.setIdAttribute('android:name')

	# check the permission tag
	hasPerC2DMessage = False
	for permissionTag in doc.getElementsByTagName('permission'):
		if '.permission.C2D_MESSAGE' in permissionTag.getAttribute('android:name'):
			hasPerC2DMessage = True
			permissionTag.setAttribute('android:name', packageName + '.permission.C2D_MESSAGE')
			permissionTag.setAttribute('android:protectionLevel', 'signature')

	if hasPerC2DMessage is False:
		newPerC2DMessage = doc.createElement('permission')
		doc.childNodes[0].appendChild(newPerC2DMessage)
		newPerC2DMessage.setAttribute('android:name', packageName + '.permission.C2D_MESSAGE')
		newPerC2DMessage.setAttribute('android:protectionLevel', 'signature')
		newPerC2DMessage.setIdAttribute('android:name')

	# check uses-sdk tag
	hasSDK = False
	for sdkTag in doc.getElementsByTagName('uses-sdk'):
		if sdkTag.hasAttribute('android:minSdkVersion'):
			hasSDK = True
			sdkTag.setAttribute('android:minSdkVersion', '8')
			sdkTag.setIdAttribute('android:minSdkVersion')

	if hasSDK is False:
		newSDK = doc.createElement('uses-sdk')
		doc.childNodes[0].appendChild(newSDK)
		newSDK.setAttribute('android:minSdkVersion', '8')
		newSDK.setIdAttribute('android:minSdkVersion')

	# check broadcast receiver tag

	receiverTag = doc.createElement('receiver')
	receiverTag.setAttribute('android:name', 'com.gcm.unity.GCMBroadcastReceiver')
	receiverTag.setAttribute('android:permission', 'com.google.android.c2dm.permission.SEND')
	receiverTag.setIdAttribute('android:name')

	intentFilter = doc.createElement('intent-filter')
	receiverTag.appendChild(intentFilter)

	receiveActionTag = doc.createElement('action')
	intentFilter.appendChild(receiveActionTag)
	receiveActionTag.setAttribute('android:name', 'com.google.android.c2dm.intent.RECEIVE')
	receiveActionTag.setIdAttribute('android:name')

	registerActionTag = doc.createElement('action')
	intentFilter.appendChild(registerActionTag)
	registerActionTag.setAttribute('android:name', 'com.google.android.c2dm.intent.REGISTRATION')
	registerActionTag.setIdAttribute('android:name')

	categoryTag = doc.createElement('category')
	intentFilter.appendChild(categoryTag)
	categoryTag.setAttribute('android:name', packageName)
	categoryTag.setIdAttribute('android:name')

	hasBroadcast = False
	applicationTag = doc.getElementsByTagName('application')[0]
	for broadcastTag in applicationTag.getElementsByTagName('receiver'):
		if 'com.google.android.c2dm.permission.SEND' in broadcastTag.getAttribute('android:permission'):
			hasBroadcast = True
			applicationTag.replaceChild(receiverTag, broadcastTag)

	if hasBroadcast is False:
		applicationTag.appendChild(receiverTag)

	xmlContent = doc.toxml()
	config = open(ANDROID_MANIFEST_PATH, r"w")
	config.write(xmlContent)
	config.close()
	out = open('Temp/StagingArea/AndroidManifest.xml', r"w")
	out.write(xmlContent)
	out.close()
	print "FINISH"

if __name__ == "__main__":
	# check platform
	print "OKKKKKKK"
	target_platform = sys.argv[2]
	if target_platform != "android":
		sys.exit()

	post_config_android()