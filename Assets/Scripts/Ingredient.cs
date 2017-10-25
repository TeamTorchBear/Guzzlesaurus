using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : Draggable {
	public string ingredientName;
	public Sprite ingredientSprite;
	private Vector2 initialPos;
	private ShelfControl shelf;

	public void Init() {
		GetComponentInChildren<SpriteRenderer>().sprite = ingredientSprite;
		shelf = FindObjectOfType<ShelfControl>();
		initialPos = transform.localPosition;
	}

	public override void OnDragStart() {
		shelf.OnClick();
	}

	public override void OnDragEnd() {
		transform.localPosition = initialPos;
	}
}
