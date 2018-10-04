using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class LevelSelector : MonoBehaviour 
{
	public static int SelectedLevel;
	List<DataModel> data = new List<DataModel>();
	
	void Awake()
	{
		try
		{
			var json = Main.ReadDataFromFile ();
			this.data = JsonHelper.FromJson<DataModel> (json).ToList();
			SnakeController.gameData = new List<DataModel>();
			SnakeController.gameData.AddRange(this.data.ToList());
		}
		catch (FileNotFoundException exception){/*ignore*/}
	}

	void Start()
	{
		var dataCount = 20 - this.data.Count;
		if (this.data.Count < 20) 
		{
			for (int i = 0; i < dataCount; i++) 
			{
				this.data.Add (new DataModel());
			}
		}
		for (int i = 0; i < this.data.Count; i++) 
		{
			if (this.data[i].LevelNumber == 0) 
			{
				this.data[i] = new DataModel ()
				{
					IsLevelCompleted = false,
					TopScore = 0,
					LevelNumber = i
				};
			}
		}

		List<DataModel> temp = new List<DataModel> ();;

		foreach (var item in this.data.OrderBy(itm => itm.LevelNumber)) 
		{
			temp.Add (item);
		}

		this.data = temp;
	}

	void OnGUI()
	{
		int x = 80, y = 80;

		if(GUI.Button(new Rect(new Vector2(0,0), new Vector2(80,30)), "<Back"))
		{
			SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
		}

		GUI.BeginGroup(new Rect(Screen.width/2-400,Screen.height/2-250,800,500));
		GUI.Box(new Rect(0,0,800,500),"Choose a level");

		for (int i = 1; i < 21; i++) 
		{

			if (this.data [i - 1].LevelNumber == i && this.data [i - 1].TopScore != 0) 
			{
				GUI.Label ((new Rect (x + 50, y + 30, 110, 60)), this.data [i - 1].TopScore.ToString ());
				GUI.backgroundColor = Color.green;

			} 
			else
			{
				GUI.backgroundColor = Color.gray;
			}

			if (GUI.Button (new Rect (new Vector2 (x, y), new Vector2 (110, 60)), i.ToString()+"\n")) 
			{
				SelectedLevel = i;
				SceneManager.LoadScene ("Game", LoadSceneMode.Single);
			}

			x += 130;

			if (i % 5 == 0) 
			{
				x = 80;
				y += 80;
			}
		}


		GUI.EndGroup ();
	}
}