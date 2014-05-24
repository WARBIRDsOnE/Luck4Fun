using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {
	public float speed = 10;

	public GameObject camera;
	private NavMeshAgent agent;
	private CharacterController controller;
	
	private float horizontalMotion;
	private float verticalMotion;
	Vector3 serverCurrentMInput = Vector3.zero;
	bool mouseIsClick = false;
	
	public void Start() {
		if (Network.isServer) {
			controller = GetComponent<CharacterController>();
			agent = GetComponent<NavMeshAgent>();
		}
	}
	
	public void Update() {
		if (Network.isClient) {
			return; //Get lost, this is the server-side!
		}
		//Debug.Log("Processing clients movement commands on server");
		/*controller.Move(Vector3(
			horizontalMotion * speed * Time.deltaTime, 
			0, 
			verticalMotion * speed * Time.deltaTime));*/

		if (mouseIsClick)
		{
			RaycastHit hit;
			Ray ray = camera.camera.ScreenPointToRay(serverCurrentMInput);
			if (Physics.Raycast(ray, out hit))
				if (transform.position != hit.point)
			{
				agent.SetDestination(hit.point);
				mouseIsClick = false;
			}
		}
	}
	
	/**
     * The client calls this to notify the server about new motion data
     * @param	motion
     */
	[RPC]
	public void updateClientMotion(Vector3 mousePosition){
		serverCurrentMInput = mousePosition;
		mouseIsClick = true;
	}

	/*public NetworkPlayer theOwner;
	public GameObject camera;
	private NavMeshAgent agent;
	
	/*float lastClientHInput = 0f;
	float lastClientVInput = 0f;
	float serverCurrentHInput = 0f;
	float serverCurrentVInput = 0f;*/
	/*Vector3 lastClientMInput = Vector3.zero;
	Vector3 serverCurrentMInput = Vector3.zero;
	bool mouseIsClick = false;
	
	void Start () {

	}
	
	void Awake()
	{
		if (Network.isClient)
			enabled = false;
	}
	
	[RPC]
	void SetPlayer(NetworkPlayer player)
	{
		theOwner = player;
		if (player == Network.player)
			enabled = true;
	}
	
	void Update()
	{
		if (theOwner != null && Network.player == theOwner)
		{
			if (Input.GetMouseButtonDown(0)) {
				lastClientMInput = Input.mousePosition;
				if (Network.isServer)
				{
					ChangeStatusMouse();
				}
				else if (Network.isClient)
				{
					networkView.RPC("ChangeStatusMouse", RPCMode.Server);
				}
			}
			/*float HInput = Input.GetAxis("Horizontal");
			float VInput = Input.GetAxis("Vertical");
			if (lastClientHInput != HInput || lastClientVInput != VInput)
			{
				lastClientHInput = HInput;
				lastClientVInput = VInput;
			}*/
			/*if (Network.isServer)
			{
				SendMovementInput(lastClientMInput);
			}
			else if (Network.isClient)
			{
				networkView.RPC("SendMovementInput", RPCMode.Server, lastClientMInput);
			}
		}
		if(Network.isServer)
		{
			if (mouseIsClick)
			{
				RaycastHit hit;
				Ray ray = camera.camera.ScreenPointToRay(serverCurrentMInput);
				if (Physics.Raycast(ray, out hit))
					if (transform.position != hit.point)
					{
						agent.SetDestination(hit.point);
						mouseIsClick = false;
					}
			}*/
			/*Vector3 moveDirection = new Vector3(serverCurrentHInput, 0, serverCurrentVInput);
			float speed = 5;
			transform.Translate(speed * moveDirection * Time.deltaTime);*/
		/*}
	}
	
	[RPC]
	void SendMovementInput(Vector3 mousePosition)
	{  
		serverCurrentMInput = mousePosition;
	}

	void ChangeStatusMouse()
	{
		mouseIsClick = true;
	}
	
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		if (stream.isWriting)
		{
			Vector3 pos = transform.position;
			Vector3 mposition = lastClientMInput;
			stream.Serialize(ref mposition);
		}
		else
		{
			Vector3 posReceive = Vector3.zero;
			Vector3 positionReceive = Vector3.zero;
			stream.Serialize(ref positionReceive);
			lastClientMInput = positionReceive;
		}
	}*/
}
