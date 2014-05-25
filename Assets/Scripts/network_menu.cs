using UnityEngine;
using System.Collections;

public class network_menu : MonoBehaviour {

	private string	_ip = "127.0.0.1";
	private int		_port = 19604;

	void Awake() {
		MasterServer.RequestHostList("Luck4Fun");
	}

	void OnGUI()
	{
		if (Network.peerType == NetworkPeerType.Disconnected) {
			if (GUI.Button(new Rect(100, 75, 150, 25), "Refresh Server List")){
				GUI.Label(new Rect(100, 100, 150, 25), "Refreshing...");
			}
			HostData[] data = MasterServer.PollHostList();
			// Go through all the hosts in the host list
			foreach (HostData element in data)
			{
				GUILayout.BeginHorizontal();	
				var name = element.gameName + " " + element.connectedPlayers + " / " + element.playerLimit;
				GUILayout.Label(name);	
				GUILayout.Space(5);
				string hostInfo;
				hostInfo = "[";
				foreach (string host in element.ip)
					hostInfo = hostInfo + host + ":" + element.port + " ";
				hostInfo = hostInfo + "]";
				GUILayout.Label(hostInfo);	
				GUILayout.Space(5);
				GUILayout.Label(element.comment);
				GUILayout.Space(5);
				GUILayout.FlexibleSpace();
				if (GUILayout.Button("Connect"))
				{
					// Connect to HostData struct, internally the correct method is used (GUID when using NAT).
					Network.Connect(element);			
				}
				GUILayout.EndHorizontal();	
			}

			if (GUI.Button(new Rect(100, 100, 100, 25), "Connect to server")){
				Network.Connect(_ip, _port);
			}
			if (GUI.Button(new Rect(100, 125, 100, 25), "Create Server")){
				Network.InitializeServer(10, _port, true);
				MasterServer.RegisterHost("Luck4Fun", "WARBIRs game", "l33t game for all");
			}
		} /*else {
			if (Network.peerType == NetworkPeerType.Client) {
				GUI.Label(new Rect(100, 100, 150, 25), "Connected to server");

				if (GUI.Button(new Rect(100, 150, 150, 25), "Disconnect to server")){
					Network.Disconnect(250);
				}
			}
			if (Network.peerType == NetworkPeerType.Server) {
				GUI.Label(new Rect(100, 100, 150, 25), "Server is Online");
				GUI.Label(new Rect(100, 125, 250, 25), "Connected client: " + Network.connections.Length);

				if (GUI.Button(new Rect(100, 150, 100, 25), "Close server")){
					Network.Disconnect(250);
				}
			}
		}*/
		if (GUI.Button(new Rect(100, 300, 150, 25), "Quit Game")){
			Application.Quit();
		}
	}
}
