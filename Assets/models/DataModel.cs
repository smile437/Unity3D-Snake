using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DataModel
{
	public bool IsLevelCompleted = false;
	public int TopScore = 0;
	public int LevelNumber = 0;
}

[Serializable]
public class ListHolder
{
	public DataModel [] dataModel;
}
