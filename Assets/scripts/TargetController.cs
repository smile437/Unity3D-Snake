using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour 
{
	public static int targetsCollected = 0;
	GameObject targetObj;

	public GameObject Target;
	public bool Paused; 

	void Start () 
	{
		var xx = Random.Range (1, 13);
		var yy = Random.Range (-9, -1);

		this.transform.position = new Vector3 (xx, yy, 0);
	}

	void OnCollisionEnter(Collision obj)
	{
		var snakeController = obj.gameObject.GetComponent<SnakeController> ();

		if (snakeController != null) 
		{
			for (int i = 0; i < snakeController.scorePerTarget; i++) 
			{
				snakeController.AddBodyPart ();
				snakeController.Score = snakeController.scorePerTarget;
			}

			snakeController.scorePerTarget++;

			targetsCollected++;

			if (obj.gameObject.name == "Snake" || obj.gameObject.name == "Wall") 
			{
				this.ReplaceTarget ();			
			}

			if (targetsCollected == 10) 
			{
				Time.timeScale = 0;
			}
		}
	}

	private void ReplaceTarget()
	{
		Destroy (this.gameObject);
		this.targetObj = Instantiate (this.Target) as GameObject;
		this.targetObj.transform.position = new Vector3 (Random.Range (1, 13), Random.Range (-9, -1),0);			
	}
}
