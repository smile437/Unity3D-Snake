using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour 
{
	void OnCollisionEnter(Collision obj)
	{
		var snakeController = obj.gameObject.GetComponent<SnakeController> ();

		if (snakeController != null) 
		{
			snakeController.SnakeDestroy ();
		}
	}
}
