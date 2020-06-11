using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using CommonsPattern;

public class WarperManager : SingletonManager<WarperManager> {

	protected WarperManager () {} // guarantee this will be always a singleton only - can't use the constructor!

	Warper warper;

	void Awake () {
		SetInstanceOrSelfDestruct(this);
	}

	void Start () {
		Setup();
	}

	public void Reset () {
		Clear();
		Setup();
	}

	public void Clear () {
	}

	public void Setup () {
	}

	void OnEnable () {
	}

	public void RegisterWarper(Warper warper) {
		Debug.LogFormat("Registering {0}", warper);
		this.warper = warper;
	}

	public void MakeWarperAppear () {
		if (warper) warper.Appear();
		else Debug.LogWarning("No Warper registered.");
	}

	public void TryMakeWarperAppear () {
		if (!warper.HasAppeared()) warper.Appear();
	}

}
