using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	private int _id;
	private string _name;

	public void createPlayer (int id, string name)
	{
		_id = id;
		_name = name;
	}

	public int getId()
	{
		return _id;
	}

	public string getName()
	{
		return _name;
	}

	public void setId(int id)
	{
		_id = id;
	}

	public void setName(string name)
	{
		_name = name;
	}
}
