using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class SnakeController : MonoBehaviour
{
	Vector2 d = new Vector2 ();
	bool gameIsFinished;

	public int Score = 0;
	public int TopScore = 0;
	public int scorePerTarget = 1;
	public int Buff = 0;
	public GameObject BodyPart;
	public List<GameObject> Body = new List<GameObject>();
	public static List<DataModel> gameData;
	public Vector2 Direction;
	public float SpeedMove = 10f;
	public bool Paused; 


	public void AddBodyPart()
	{
		var pos = transform.position;

		if (this.Body.Count > 0) 
		{
			pos = this.Body [this.Body.Count - 1].transform.position;
		}

		GameObject lastPart = Instantiate (this.BodyPart, pos, Quaternion.identity) as GameObject;

		this.Body.Add (lastPart);
	}

	void Start () 
	{
		if (gameData == null) 
		{
			gameData = new List<DataModel> ();
		}

		foreach (var item in gameData) 
		{
			if (item.LevelNumber == LevelSelector.SelectedLevel) 
			{
				this.TopScore = item.TopScore;
			}
		}
		TargetController.targetsCollected = 0;
		Time.timeScale = 1;
		this.Body.Clear ();
		
		for(int i = 0;i<3;i++)
		{
			this.AddBodyPart ();
		}
	}
		
	void Update () 
	{
		this.Buff++;

		if (this.Buff > this.SpeedMove/2) 
		{
			this.Buff = 0;
		
			if (this.Direction.x != 0 || this.Direction.y != 0 )
			{
				var rBody = GetComponent<Rigidbody> ();

				rBody.velocity = new Vector3 (this.Direction.x * this.SpeedMove, Direction.y * this.SpeedMove, 0);

				if (this.Body.Count > 0) 
				{
					this.Body [0].transform.position = this.transform.position;
					for (int i = this.Body.Count - 1; i > 0; i--) 
					{
						this.Body [i].transform.position = this.Body [i - 1].transform.position;
					}
				}
			}
		}

		if(Input.GetKeyUp(KeyCode.Escape))
		{  
			if(!this.Paused)
			{  
				Time.timeScale = 0;
				d = new Vector2 (this.Direction.x, this.Direction.y);
				this.Direction = new Vector2 (0, 0);
				this.Paused = true;  
			}
			else
			{  
				Time.timeScale = 1;
				this.Direction = d;
				this.Paused = false;
			} 
		} 

		if (TargetController.targetsCollected == 10 && !this.gameIsFinished) 
		{
			this.gameIsFinished = true;
			this.Direction = new Vector2 (0, 0);

			var currData = new DataModel () 
			{
				IsLevelCompleted = true,
				LevelNumber = LevelSelector.SelectedLevel,
				TopScore = this.Score
			};

			if (gameData.Count > 0) 
			{
				for (int i = 0; i < gameData.Count; i++)
				{
					if (gameData [i].LevelNumber == currData.LevelNumber)
					{
						gameData [i] = currData;
					}
					else 
					{
						gameData.Add (new DataModel ()
						{
							IsLevelCompleted = true,
							LevelNumber = LevelSelector.SelectedLevel,
							TopScore = this.Score
						});
					}

				}
			} 
			else
			{
				gameData.Add (currData);
			}

			var lh = new ListHolder ();
			lh.dataModel = gameData.ToArray();
			string json = JsonHelper.ToJson (lh.dataModel);
			Main.WriteDataToFile (json);
		}
	}

	public void SnakeDestroy()
	{
		this.Direction = new Vector2 (0, 0);

		foreach (GameObject part in this.Body) 
		{
			DestroyObject (part.gameObject);
		}

		if (this.Score > this.TopScore)
		{
			this.TopScore = this.Score;
		}

		DestroyObject (this.gameObject);

		Time.timeScale = 0;
		SceneManager.LoadScene ("Game");

	}
		
	void OnGUI()
	{
		var centerX = Screen.width / 2;
		var centerY = Screen.height / 2;

		GUI.Label (new Rect (new Vector2 (centerX - 50, 0), new Vector2 (160, 30)), "Score: " + this.Score +"");

		GUI.Label (new Rect (new Vector2 (centerX + 50, 0), new Vector2 (160, 30)), "TopScore: " + this.TopScore +"");

		if (this.Paused == true) 
		{
			GUI.BeginGroup (new Rect (centerX - 150, centerY - 150, 500, 500));
			GUI.Box (new Rect (0, 0, 250, 300), "Confirm");

			if(GUI.Button(new Rect(new Vector2(centerX - 475, centerY - 280), new Vector2(160,30)), "Yes"))
			{
				Application.LoadLevel ("LevelSelector");
			}

			if(GUI.Button(new Rect(new Vector2(centerX - 475, centerY - 230), new Vector2(160,30)), "No"))
			{
				Time.timeScale = 1;
				this.Direction = d;
				this.Paused = false;
			}


			GUI.EndGroup ();
		}


		if(TargetController.targetsCollected == 10)
		{
			this.Direction = new Vector2 ();
			if(this.Score > this.TopScore)
			{
				this.TopScore = this.Score;
			}
			GUI.BeginGroup (new Rect (centerX - 150, centerY - 150, 500, 500));
			GUI.Box (new Rect (0, 0, 250, 300), "You win!");

			if(GUI.Button(new Rect(new Vector2(centerX - 475, centerY - 280), new Vector2(160,30)), "Next"))
			{
				Application.LoadLevel ("Game");
				if(LevelSelector.SelectedLevel<20)
				{
					LevelSelector.SelectedLevel++;
					TargetController.targetsCollected = 0;
				}
			}
			GUI.EndGroup ();
		}
	}
}
