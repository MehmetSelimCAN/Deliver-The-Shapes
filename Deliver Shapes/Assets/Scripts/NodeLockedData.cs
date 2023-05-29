using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using static NodeLockedData;

public class NodeLockedData : MonoBehaviour {

    [System.Serializable]
    public class RequiredIngredient {
        public int count;
        public ResourceType resourceType;
    }

    [SerializeField] private List<RequiredIngredient> requiredIngredientsToUnlock = new List<RequiredIngredient>();
    [SerializeField] private Transform requiredIngredientsParent;
    [SerializeField] private Transform requiredIngredientTemplate;

    [SerializeField] private int earnedLineCount;
    [SerializeField] private TextMeshProUGUI earnedLineCountText;

    [SerializeField] private List<RequiredIngredient> inputIngredients = new List<RequiredIngredient>();
    [SerializeField] private Transform inputIngredientsParent;
    [SerializeField] private Transform inputIngredientTemplate;

    [SerializeField] private SpriteRenderer outputIngredientSpriteRenderer;
    [SerializeField] private ResourceType outputResourceType;

    private void Awake() {
        CreateRequiredIngredientsVisual();
        UpdateEarnLineCount();
        CreateInputOutputIngredientsVisual();
    }

    private void CreateRequiredIngredientsVisual() {
        for (int i = 0; i < requiredIngredientsToUnlock.Count; i++) {
            var requiredIngredient = Instantiate(requiredIngredientTemplate, requiredIngredientsParent).GetComponent<IngredientTemplate>();

            Sprite resourceSprite = SpriteProvider.Instance.GetResourceSprite(requiredIngredientsToUnlock[i].resourceType);
            requiredIngredient.UpdateSprite(resourceSprite);
            requiredIngredient.UpdateCount(requiredIngredientsToUnlock[i].count);

            requiredIngredient.gameObject.SetActive(true);
        }
    }

    private void UpdateEarnLineCount() {
        earnedLineCountText.SetText(earnedLineCount.ToString());
    }

    private void CreateInputOutputIngredientsVisual() {
        for (int i = 0; i < inputIngredients.Count; i++) {
            var inputIngredient = Instantiate(inputIngredientTemplate, inputIngredientsParent).GetComponent<IngredientTemplate>();

            Sprite resourceSprite = SpriteProvider.Instance.GetResourceSprite(inputIngredients[i].resourceType);
            inputIngredient.UpdateSprite(resourceSprite);
            inputIngredient.UpdateCount(inputIngredients[i].count);

            inputIngredient.gameObject.SetActive(true);
        }

        outputIngredientSpriteRenderer.sprite = SpriteProvider.Instance.GetResourceSprite(outputResourceType);
    }
}
