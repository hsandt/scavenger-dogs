using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using CommonsPattern;
using UnityConstants;

public class ScrapPoolManager : MultiPoolManager<Scrap, ScrapPoolManager>  {
	protected override void Init()
	{
		// MultiPoolManager Init will generate pool items
		// in poolTransform, so we need to set it before calling base.Init().
		// This also means we don't have to set poolTransform in the inspector
		// avoiding an external reference that couldn't be saved in
		// ScrapPoolManager prefab.
		// However, I let the designer override that if they want,
		// so only set it if not set yet.
		if (poolTransform == null)
		{
			poolTransform = Locator.FindWithTag(Tags.ScrapParent).transform;
		}

		base.Init();
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
