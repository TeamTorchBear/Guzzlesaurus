using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : Draggable {
    public string ingredientName;
    public Sprite ingredientSprite;

    [HideInInspector]
    public Vector2 initialPos;
    private ShelfControl shelf;
    private BowlControl bowl;

    public void Init() {
        GetComponentInChildren<SpriteRenderer>().sprite = ingredientSprite;
        shelf = FindObjectOfType<ShelfControl>();
        bowl = FindObjectOfType<BowlControl>();
        initialPos = transform.position;
    }

    public override void OnDragStart() {
        base.OnDragStart();
        shelf.Close();
    }

    public override void OnDragEnd() {
        base.OnDragEnd();

        if (!bowl.DropIngredient(transform.position, this)) {
            MoveTo(initialPos, false);
        }
    }

    public void MoveTo(Vector2 position, bool destroyAfter) {
        StartCoroutine(AnimatePosition(position, destroyAfter));
    }


    private IEnumerator AnimatePosition(Vector3 finalPos, bool destroyAfter) {
        float startTime = Time.time;
        Vector3 initialPos = transform.position;
        float distance = Vector3.Distance(initialPos, finalPos);
        float distCovered = 0, fracJourney = 0;
        if (distance > 0) {
            while (fracJourney < 1) {
                distCovered = (Time.time - startTime) * 20f;
                fracJourney = distCovered / distance;
                transform.position = Vector3.Lerp(initialPos, finalPos, fracJourney);
                yield return false;
            }
            transform.position = finalPos;
            if(destroyAfter) {
                Destroy(this.gameObject);
            }
        }
    }
}
