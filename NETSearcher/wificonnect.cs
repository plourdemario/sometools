using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Data;
using System.Net;
using System.Net.Http;
using ManagedNativeWifi;
//using ManagedNativeWifi.Win32.BaseMethod;

namespace GetData
{
    class Program
    {
		static void Main(string[] args)
        {
			var availableNetwork = NativeWifi.EnumerateAvailableNetworks().FirstOrDefault(x => x.Ssid.ToString() == ssid);

			if (availableNetwork == null)
				return;

			var profile = NativeWifi.EnumerateProfiles().FirstOrDefault(x => x.Name == ssid);

			if (profile == null)
			{
				// build XML
				string profileName = ssid;
				string mac = StringToHex(profileName);
				string profileXml = string.Format("<?xml version=\"1.0\"?><WLANProfile xmlns = \"http://www.microsoft.com/networking/WLAN/profile/v1\"><name>{0}</name><SSIDConfig><SSID><hex>{1}</hex><name>{0}</name></SSID></SSIDConfig><connectionType>ESS</connectionType><connectionMode>auto</connectionMode><MSM><security><authEncryption><authentication>WPA2PSK</authentication><encryption>AES</encryption><useOneX>false</useOneX></authEncryption><sharedKey><keyType>passPhrase</keyType><protected>true</protected><keyMaterial>... key removed for security...</keyMaterial></sharedKey></security></MSM><MacRandomization xmlns=\"http://www.microsoft.com/networking/WLAN/profile/v3\"><enableRandomization>false</enableRandomization><randomizationSeed>153878511</randomizationSeed></MacRandomization></WLANProfile>", ssid, mac);

				// create a profile
				var profileResult = NativeWifi.SetProfile(availableNetwork.Interface.Id, ProfileType.AllUser, profileXml, encryption, true);
			}
			else
			{
				//todo: log here
			}
		}
	}
}