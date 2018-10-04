using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;


public class Main : MonoBehaviour 
{		
	bool isDeleteTapped = false;
	
	void OnGUI()
	{
		var centerX = Screen.width / 2;
		var centerY = Screen.height / 2;

		if (this.isDeleteTapped == false) 
		{
			if(GUI.Button(new Rect(new Vector2(centerX - 80, centerY - 40), new Vector2(160,30)), "Play"))
			{
				SceneManager.LoadScene ("LevelSelector", LoadSceneMode.Single);
			}

			if(GUI.Button(new Rect(new Vector2(centerX - 80, centerY), new Vector2(160,30)), "Reset game"))
			{
				this.isDeleteTapped = true;
			}

			if(GUI.Button(new Rect(new Vector2(centerX - 80, centerY + 40), new Vector2(160,30)), "Exit"))
			{
				Application.Quit ();
			}
		}

		if (this.isDeleteTapped == true) 
		{	
			GUI.BeginGroup (new Rect (centerX - 150, centerY - 150, 500, 500));
			GUI.Box (new Rect (0, 0, 250, 300), "Confirm");


			if (GUI.Button (new Rect (new Vector2 (centerX - 475, centerY - 280), new Vector2 (160, 30)), "Yes"))
			{
				File.Delete (Application.dataPath + "/Data.json");
				this.isDeleteTapped = false;
			}

			if (GUI.Button (new Rect (new Vector2 (centerX - 475, centerY - 230), new Vector2 (160, 30)), "No"))
			{
				this.isDeleteTapped = false;
			}


			GUI.EndGroup ();
		}
	}

	public static void WriteDataToFile (string jsonString)
	{
		string path = Application.dataPath + "/Data.json";
		File.WriteAllText (path, jsonString);
	}

	public static string ReadDataFromFile ()
	{
		string path = Application.dataPath + "/Data.json";
		var jsonData = File.ReadAllText (path);
		return jsonData;
	}
}
