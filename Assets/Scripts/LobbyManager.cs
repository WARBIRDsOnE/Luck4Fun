using UnityEngine;
using System.Collections;

public class LobbyManager : MonoBehaviour {
	public static string levelName ; //current level name

	public Transform playerPrefab;
	public ArrayList playerList = new ArrayList();

	void OnGUI()
	{
		if (Network.peerType != NetworkPeerType.Disconnected)
		{
			if (Network.peerType == NetworkPeerType.Client) {
				GUI.Label(new Rect(100, 100, 150, 25), "Connected to server");
				
				
			}
			if (Network.peerType == NetworkPeerType.Server) {
				GUI.Label(new Rect(100, 100, 150, 25), "Server is Online");
				GUI.Label(new Rect(100, 125, 250, 25), "Connected client: " + Network.connections.Length);
			}
			if (GUI.Button(new Rect(100, 150, 150, 25), "Disconnect")){
				Network.Disconnect(250);
			}
			foreach (Player player in playerList)
			{
				GUILayout.BeginHorizontal();	
				var id = player.getId() + "-" + player.getName();
				GUILayout.Label(id);	
				GUILayout.Space(5);
				GUILayout.Label("Color");
				GUILayout.Space(5);
				if (Network.peerType == NetworkPeerType.Server)
				{
					GUILayout.Label("Kick");
					GUILayout.Space(5);
				}
				GUILayout.EndHorizontal();	
			}
		}
	}

	void OnServerInitialized()
	{
		AddPlayerToLobby(Network.player, "Player0", true);
	}

	void OnPlayerConnected(NetworkPlayer player)
	{
		AddPlayerToLobby(player, "Player"+ playerList.Count.ToString());
	}
	
	void OnPlayerDisconnected(NetworkPlayer player)
	{
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
		playerList.Remove (player);
	}

	void AddPlayerToLobby(NetworkPlayer player, string name, bool isMaster = false)
	{
		string tempPlayerString = player.ToString ();
		int playerNumber = 0;
		if (int.TryParse (tempPlayerString, out playerNumber)) {
			Player playerObj = new Player();
			playerObj.createPlayer(playerNumber, name);
       		playerList.Add(playerObj);
		}
	}

	void SpawnPlayer(Player player)
	{
		string tempPlayerString = player.ToString ();
		int playerNumber = 0;
		if (int.TryParse (tempPlayerString, out playerNumber)) {
			Transform newPlayerTransform = (Transform)Network.Instantiate(playerPrefab, 
			                                                              transform.position + new Vector3(0, 10, 0), 
			                                                              transform.rotation, playerNumber);
			playerList.Add (newPlayerTransform.GetComponent ("Player"));
			NetworkView theNetworkView = newPlayerTransform.networkView;
			theNetworkView.RPC ("setOwner", RPCMode.AllBuffered, player);
		}
	}
}
