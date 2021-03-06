﻿/*
 * Multiplayer Script by RAMIN Alexandre
 * Point & Click movement
 * Need Agent engine
 * */
using UnityEngine;
using System.Collections;

public class C_PlayerManager : MonoBehaviour {
	//That's actually not the owner but the player,
	//the server instantiated the prefab for, where this script is attached
	private NetworkPlayer owner;
	public GameObject camera;
	
	//Those are stored to only send RPCs to the server when the 
	//data actually changed.
	private float lastMotionH; //horizontal motion
	private float lastMotionV; //vertical motion

	Vector3 lastClientMInput = Vector3.zero;
	
	[RPC]
	void setOwner(NetworkPlayer player){
		Debug.Log("Setting the owner.");
		owner = player;
		if(player == Network.player){
			//So it just so happens that WE are the player in question,
			//which means we can enable this control again
			enabled=true;
		}
		else {
			//Disable a bunch of other things here that are not interesting:
			//camera.camera.enabled = false;
			//camera.SetActive(false);
			GetComponentInChildren<Camera>().enabled = false;
			
			if (GetComponent<AudioListener>()) {
				GetComponent<AudioListener>().enabled = false;
			}
			
			if (GetComponent<GUILayer>()) {
				GetComponent<GUILayer>().enabled = false;
			}
		}
	}
	
	[RPC]
	NetworkPlayer getOwner(){
		return owner;
	}
	
	public void Awake() {
		//Disable this by default for now
		//Just to make sure no one can use this until we didn't
		//find the right player. (see setOwner())
		if (Network.isClient) {
			enabled = false;
		}
	}
	
	public void Update () {
		if (Network.isServer) {
			if ((owner != null) && (Network.player == owner)) {
				if (Input.GetMouseButtonDown(0)) {
					lastClientMInput = Input.mousePosition;
					RaycastHit hit;
					Ray ray = camera.camera.ScreenPointToRay(lastClientMInput);
					if (Physics.Raycast(ray, out hit))
					{
						PlayerManager pm = GetComponent<PlayerManager>();
						pm.updateClientMotion(hit.point);
					}
				}
			}
			return; //get lost, this is the client side!
		}
		//Check if this update applies for the current client
		if ((owner != null) && (Network.player == owner)) {
			if (Input.GetMouseButtonDown(0)) {
				lastClientMInput = Input.mousePosition;
				RaycastHit hit;
				Ray ray = camera.camera.ScreenPointToRay(lastClientMInput);
				if (Physics.Raycast(ray, out hit))
				{
					networkView.RPC("updateClientMotion", RPCMode.Server, hit.point);
				}
			}
		}
	}
}
