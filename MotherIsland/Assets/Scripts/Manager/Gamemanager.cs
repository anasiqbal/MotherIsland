using System;
using UnityEngine;
using UnityEngine.UI;

public class Gamemanager : MonoBehaviour {

	#region Enums

	public enum States
	{
		MAINMENU,
		GAMEPLAY,
		PAUSE,
		GAMEOVER,
		RESTART,
		RESUME
	}

	#endregion

	#region Attribute/Properties

	[Header("Script References")] 
	[SerializeField] private Player playerController;
	[SerializeField] private ShipController shipController;

	[Header("Sounds")] 
	[SerializeField] public AudioClip buttonClickClip;
	private AudioSource buttonClickSource;

	[Header("UI References")] 
	[SerializeField] private GameObject[] menus;
	[SerializeField] private Image soundIcon;
	[SerializeField] private Text time;
	[SerializeField] private Text sruvivedTime;
	[SerializeField] private GameObject creditsWindow;

	[Header("Icons")] 
	[SerializeField] private Sprite[] soundIcons;

	//Private Variables
	private static Gamemanager manager;
	private float seconds;
	private States currentState;
	private AudioSource audioSource;

	//Properties 
	public static Gamemanager Manager{get { return manager; }}

	#endregion

	private void Awake()
	{
		 //Initialze Manager Instance.
		manager = this;

		//Get References.
		buttonClickSource = GetComponent<AudioSource>();
		audioSource = GetComponent<AudioSource>();

		//Configure Audio Source.
		ConfigureSound();

		//Set Game State.
		TransitionGameState(States.MAINMENU);
	}

	void Update()
	{
		if (currentState == States.GAMEPLAY)
		{
			seconds += Time.deltaTime;
			var t = TimeSpan.FromSeconds(seconds);
			time.text = string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds);
		}
	}

	public void TransitionGameState(States gamestate)
	{
		currentState = gamestate;
		switch (gamestate)
		{
			case States.MAINMENU:
				closeOpenMenus();
				menus[(int)gamestate].gameObject.SetActive(true);
				break;
			case States.GAMEPLAY:
				closeOpenMenus();
				seconds = 0;
				menus[(int)gamestate].gameObject.SetActive(true);
				playerController.Initialize();
				shipController.Initialize();
				break;
			case States.PAUSE:
				closeOpenMenus();
				menus[(int)gamestate].gameObject.SetActive(true);
				Time.timeScale = 0;
				break;
			case States.GAMEOVER:
				closeOpenMenus();
				menus[(int)gamestate].gameObject.SetActive(true);
				sruvivedTime.text = TimeSpan.FromSeconds(seconds).ToString();
				shipController.StopSpawning();
				break;
			case States.RESTART:
				closeOpenMenus();
				shipController.RemoveOnScreenShips();
				TransitionGameState(States.GAMEPLAY);
				break;
			case States.RESUME:
				closeOpenMenus();
				menus[(int)States.GAMEPLAY].gameObject.SetActive(true);
				Time.timeScale = 1f;
				break;
		}
	}

	#region Utils

	private void ConfigureSound()
	{
		int soundFlag = PlayerPrefs.GetInt("Sound", 1);
		AudioListener.pause = soundFlag != 1;
		soundIcon.sprite = soundIcons[soundFlag];
	}
	
	private void closeOpenMenus()
	{
		foreach (GameObject menu in menus)
		{
			menu.SetActive(false);
		}
		
	}

	#endregion
	
	#region UI

	public void OnPlay()
	{
		audioSource.PlayOneShot(buttonClickClip);
		TransitionGameState(States.GAMEPLAY);
	}

	public void OnSound()
	{
		audioSource.PlayOneShot(buttonClickClip);
		PlayerPrefs.SetInt("Sound",PlayerPrefs.GetInt("Sound", 1) == 1 ? 0 : 1);
		ConfigureSound();
	}

	public void OnAbout()
	{
		audioSource.PlayOneShot(buttonClickClip);
		creditsWindow.gameObject.SetActive(true);
	}

	public void OnPause()
	{
		audioSource.PlayOneShot(buttonClickClip);
		TransitionGameState(States.PAUSE);
	}

	public void OnRestart()
	{
		audioSource.PlayOneShot(buttonClickClip);
		TransitionGameState(States.RESTART);
	}

	public void OnResume()
	{
		audioSource.PlayOneShot(buttonClickClip);
		TransitionGameState(States.RESUME);
	}

	public void OnCredits()
	{
		creditsWindow.gameObject.SetActive(false);
	}

	public void OnQuit()
	{
		Application.Quit();
	}

	#endregion

}
