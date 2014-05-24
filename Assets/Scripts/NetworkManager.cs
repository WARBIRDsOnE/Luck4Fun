using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour {

	public Transform playerPrefab;
	public ArrayList playerScripts = new ArrayList();
	
	void OnServerInitialized()
	{
		SpawnPlayer(Network.player);
	}
	void OnPlayerConnected(NetworkPlayer player)
	{
		SpawnPlayer(player);
	}

	void OnPlayerDisconnected(NetworkPlayer player)
	{
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
		playerScripts.Remove (player);
	}

	void SpawnPlayer(NetworkPlayer player)
	{
		string tempPlayerString = player.ToString ();
		int playerNumber = 0;
		if (int.TryParse (tempPlayerString, out playerNumber)) {
			Transform newPlayerTransform = (Transform)Network.Instantiate (playerPrefab, transform.position, transform.rotation, playerNumber);
			playerScripts.Add (newPlayerTransform.GetComponent ("Player"));
			NetworkView theNetworkView = newPlayerTransform.networkView;
			theNetworkView.RPC ("setOwner", RPCMode.AllBuffered, player);
		}
	}
}
