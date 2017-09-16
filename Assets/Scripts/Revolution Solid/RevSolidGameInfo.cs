﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RevSolidGameInfo : MonoBehaviour {
	
	protected static int hit;
	protected static int falseStrokeCount;
	//private RevSolidUIControl uiController= new RevSolidUIControl();
	public const int MaxFalseCount=8;
	const int GuidingTrialNum = 4;
	const int LearningThres = 6;
	public const int WinningCriterion = 8;

	public static float RecoverInterval=7.0f;
	public static float MaxReactionTime=9999.0f;
	public static int MaxPolygonNum=12;
	public static int MaxPanelNum;

	public static int polygonGenerationCount;

	public static float levelOfDifficulty;//(0.5,1)(1.5,2)

	// Use this for initialization
	void Awake () {
		InitializeHit ();
		falseStrokeCount = 0;
		polygonGenerationCount = 0;
	}

	void OnEnable(){
		EventManager.StartListening ("Qdown",GoBackToWorld);
	}

	void OnDisable(){
		EventManager.StopListening ("Qdown",GoBackToWorld);
	}

	public static void InitializeHit(){
		hit = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static int Add2TotalHit (int scoreAdded){
		hit+=scoreAdded;
		return hit;
	}

	public static void Add2FalseStrokeCount (int scoreAdded){
		falseStrokeCount+=scoreAdded;
	}
		
	public static void CheckEndOfGame(){
		
		if (hit >= WinningCriterion) {
			DataUtil.UnlockCurrentRoom();
			RevSolidUIControl.defaultString = "CONGRATULATIONS you have unlocked this room. Press Q to quit, or continue enjoying your play.";
		}

		if (hit < WinningCriterion) {
			RevSolidUIControl.defaultString = "";
			if (falseStrokeCount >= MaxFalseCount) {
				RevSolidUIControl.BroadcastMsg ("You failed the game. Press RESTART to refill yourself with determination.");
				Time.timeScale = 0;
				RevSolidUIControl.ShowRetryButton ();
			}
		}

		if (Input.GetKeyDown(KeyCode.Q)) {
			EventManager.TriggerEvent ("Qdown");

		}
	}

	void GoBackToWorld(){
		Time.timeScale = 1;
		SceneManager.LoadScene("World Scene");
	}
	 
	public static bool IfNoviceGuideEnds(){
		return hit >= GuidingTrialNum;
	}

	public static bool WhenNoviceGuideEnds(){
		return polygonGenerationCount >= GuidingTrialNum && hit==GuidingTrialNum;
	}

	public static bool CheckIfPlayerLearned(){
		return hit >= LearningThres;
	}

	public static bool CheckIfPlayerFailsMuch(){
		return falseStrokeCount >= MaxFalseCount-2 && Tutorial.isTutorialModeOn == false;
	}

	public virtual void Retry(){
		hit = 0;
		falseStrokeCount = 0;
	}
	public static int GetLODByInt(){
		return Mathf.CeilToInt(levelOfDifficulty);
	}
		
}
