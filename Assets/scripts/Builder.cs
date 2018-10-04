using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class Builder : MonoBehaviour 
{
	GameObject snakeObj;
	int lvlWidth, lvlHeight;
	int lineNumber = 0;
	float xx = 0;
	float yy = 0;

	public GameObject Snake;
	public GameObject Wall;

	struct Points
	{
		public int x;
		public int y;
	}

	void Awake()
	{
		string line;
		List<Points> input = new List<Points> ();

		StreamReader theReader = new StreamReader(Application.streamingAssetsPath + "/LVL" +LevelSelector.SelectedLevel+".txt", Encoding.Default);
		using (theReader)
		{
			do
			{
				line = theReader.ReadLine();
				if (line != null)
				{
					if(this.lineNumber == 0)
					{
						this.lvlWidth = line.Length;
					}
				

				for(int i = 0; i<line.Length;i++)
				{
					if(line[i] == 'w')
					{
						Points temp = new Points();
						temp.y = this.lineNumber/-1;
						temp.x = i;
						input.Add(temp);
					}
				}

				this.lineNumber++;
				this.lvlHeight++;
				}

			}
			while (line != null);  
			theReader.Close();
		}

		for (int i = 0; i < input.Count; i++) 
		{
			var wallObj = Instantiate (this.Wall) as GameObject;
			wallObj.gameObject.transform.position = new Vector3 (input [i].x, input [i].y, 0);
		}
	}

	void Start()
	{
		this.snakeObj = Instantiate (this.Snake) as GameObject;
		this.snakeObj.name = "Snake";
	}

	void Update()
	{
		if (this.Snake != null) 
		{
			if (Input.GetAxis ("Horizontal") > 0) 
			{
				if (this.xx != -1) 
				{
					this.xx = 1;
					this.yy = 0;
				}
			}

			if (Input.GetAxis ("Horizontal") < 0)
			{
				if (this.xx != 1) 
				{
					this.xx = -1;
					this.yy = 0;
				}
			}

			if (Input.GetAxis ("Vertical") > 0) 
			{
				if (this.yy != -1)
				{
					this.yy = 1;
					this.xx = 0;
				}
			}

			if (Input.GetAxis ("Vertical") < 0) 
			{
				if (this.yy != 1) 
				{
					this.yy = -1;
					this.xx = 0;
				}
			}

			if (this.xx != 0 || yy != 0) 
			{
				var snakeController = this.snakeObj.GetComponent<SnakeController> ();

				if (this.xx != 0) 
				{
					snakeController.Direction = new Vector2 (this.xx, 0);
				}

				if (this.yy != 0) 
				{
					snakeController.Direction = new Vector2 (0, this.yy);
				}
			}
		}
	}
}
