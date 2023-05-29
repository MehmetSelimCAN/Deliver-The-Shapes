using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IngredientTemplate : MonoBehaviour {

    [SerializeField] private SpriteRenderer ingredientSpriteRenderer;
    [SerializeField] private TextMeshProUGUI countText;

    public void UpdateSprite(Sprite sprite) {
        ingredientSpriteRenderer.sprite = sprite;
    }

    public void UpdateCount(int count) {
        countText.SetText(count.ToString());
    }
}
