using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class IngredientVisual : MonoBehaviour {

    private ResourceType resourceType;
    public ResourceType ResourceType { get { return resourceType; } set { resourceType = value; } }

    [SerializeField] private Image ingredientImage;
    [SerializeField] private TextMeshProUGUI countText;

    public void UpdateResourceType(ResourceType resourceType) {
        this.resourceType = resourceType;
    }

    public void UpdateSprite() {
        Sprite resourceSprite = SpriteProvider.Instance.GetResourceSprite(resourceType);
        ingredientImage.sprite = resourceSprite;
    }

    public void UpdateCount(int count = 0) {
        countText.SetText(count.ToString());
    }
}
