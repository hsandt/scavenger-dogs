using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

using CommonsPattern;

// SEO: before PlayerJoinUI because we deactivate some game objects having this script in messageDict, and we do not want to let them call OnEnable for nothing
public class UIManager : SingletonManager<UIManager> {

	protected UIManager () {} // guarantee this will be always a singleton only - can't use the constructor!

	// canvas object references
	// public GameObject uiCanvas;
	// the following two must be attached in the inspector
	// WARNING: if you want to instantiate them, use the prefab; otherwise link to the scene object!
	public GameObject messageCanvas;
	public GameObject pauseMenuCanvas;
	public GameObject pauseMenuDefaultSelected;

	// message references
	Dictionary<string, GameObject> messageDict;

	// Use this for initialization
	void Awake () {
		SetInstanceOrSelfDestruct(this);

		// if object references have not been set already, set them
		// does not work for inactive objects
		// TODO Solutions:
		// 1 - start with all objects active, reference them, then deactivate them
		// 		before the game begins
		// 2 - look for all games in the trees with "search inactive" option true
		//		quite costly for big scenes
		// 3 - tag and keep active intermediate parents
		//		then search in all their children, including inactive ones (cheap)
		//		you can still work with the lower parent for activation/deactivation
		// this.SetWithTagIfNullOrDie(ref uiCanvas, "UICanvas");

		// just check if references have been set
		if (messageCanvas == null) {
			throw new UnityException(this + "messageCanvas not set in the Inspector.");
		}
		if (pauseMenuCanvas == null) {
			throw new UnityException(this + "pauseMenuCanvas not set in the Inspector.");
		}
		if (pauseMenuDefaultSelected == null) {
			throw new UnityException(this + "pauseMenuDefaultSelected not set in the Inspector.");
		}

	}

	void Start () {
		Setup();
	}

	public void Setup () {
		// register all messages and deactivate them just in case
		messageDict = new Dictionary<string, GameObject>();
		foreach (Transform child in messageCanvas.transform) {
			var go = child.gameObject;
			messageDict[child.name] = go;
			go.SetActive(false);
		}

		// deactive menu tree
		pauseMenuCanvas.SetActive(false);
	}

	public GameObject GetMessage(string messageName) {
		return messageDict[messageName];
	}

	/// Show the message registered with name messageName
	public void ShowMessage(string messageName) {
		messageDict[messageName].SetActive(true);
	}

	/// Hide the message registered with name messageName
	public void HideMessage(string messageName) {
		messageDict[messageName].SetActive(false);
	}

	/// Select Selectable message with name messageName
	public void SelectMessage(string messageName) {
		EventSystem.current.SetSelectedGameObject(messageDict[messageName]);
	}

	/// Show and select Selectable message with name messageName
	public void ShowAndSelectMessage(string messageName) {
		var selectableMessage = messageDict[messageName];
		selectableMessage.SetActive(true);
		EventSystem.current.SetSelectedGameObject(selectableMessage);
	}

	/// Deselect and hide Selectable message. Use this instead of Hide to properly hide selectables
	public void HideSelectableMessage(string messageName) {
		EventSystem.current.SetSelectedGameObject(null);
		HideMessage(messageName);
	}

	public bool IsPauseMenuActive () {
		return pauseMenuCanvas.activeSelf;
	}

	/// Open the pause menu
	public void OpenPauseMenu () {
		// first activate the pause menu object to ensure selection events are correctly passed
		pauseMenuCanvas.SetActive(true);
		// select default first element (reference from this script, not from EventSystem.firstSelectedGameObject)
		EventSystem.current.SetSelectedGameObject(pauseMenuDefaultSelected);
	}

	/// Close the pause menu
	public void ClosePauseMenu () {
		// deselect current selected to avoid no highlight bug when menu is reactivated
		EventSystem.current.SetSelectedGameObject(null);
		// deactive menu tree
		pauseMenuCanvas.SetActive(false);
	}
}
