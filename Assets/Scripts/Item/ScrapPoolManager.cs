using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using CommonsPattern;

public class ScrapPoolManager : MultiPoolManager<Scrap, ScrapPoolManager>  {

	void Awake () {
		Init();
	}

	public Scrap SpawnScrap(Vector2 position, GameColor color) {
		// TODO: use string interpolation from .NET 4.5
		// resource names must match enum names exactly
		string resourceName = $"Scrap_{color}";
		Scrap scrap = GetObject(resourceName);
		if (scrap != null) {
			scrap.Spawn(position);
			return scrap;
		}
		// pool starvation
		return null;
	}

}
