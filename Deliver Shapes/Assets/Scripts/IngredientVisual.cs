using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngredientVisual : MonoBehaviour {

    [SerializeField] private Image ingredientImage;
    [SerializeField] private TextMeshProUGUI countText;

    public void UpdateSprite(Sprite sprite) {
        ingredientImage.sprite = sprite;
    }

    public void UpdateCount(int count) {
        countText.SetText(count.ToString());
    }
}
