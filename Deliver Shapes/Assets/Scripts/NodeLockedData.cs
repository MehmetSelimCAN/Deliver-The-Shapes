using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class NodeLockedData : MonoBehaviour {

    [System.Serializable]
    public class RequiredIngredient {
        public int count;
        public ResourceType resourceType;
    }

    [SerializeField] private List<RequiredIngredient> requiredIngredientsToUnlock = new List<RequiredIngredient>();
    public List<RequiredIngredient> RequiredIngredientsToUnlock { get { return requiredIngredientsToUnlock; } }
    [SerializeField] private Transform requiredIngredientsParent;
    [SerializeField] private Transform requiredIngredientTemplate;
    private List<IngredientVisual> RequiredIngredientVisuals = new List<IngredientVisual>();

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
            var requiredIngredientVisual = Instantiate(requiredIngredientTemplate, requiredIngredientsParent).GetComponent<IngredientVisual>();

            Sprite resourceSprite = SpriteProvider.Instance.GetResourceSprite(requiredIngredientsToUnlock[i].resourceType);
            requiredIngredientVisual.UpdateSprite(resourceSprite);
            requiredIngredientVisual.UpdateCount(requiredIngredientsToUnlock[i].count);

            requiredIngredientVisual.gameObject.SetActive(true);
            RequiredIngredientVisuals.Add(requiredIngredientVisual);
        }
    }

    private void UpdateEarnLineCount() {
        earnedLineCountText.SetText(earnedLineCount.ToString());
    }

    private void CreateInputOutputIngredientsVisual() {
        for (int i = 0; i < inputIngredients.Count; i++) {
            var inputIngredient = Instantiate(inputIngredientTemplate, inputIngredientsParent).GetComponent<IngredientVisual>();

            Sprite resourceSprite = SpriteProvider.Instance.GetResourceSprite(inputIngredients[i].resourceType);
            inputIngredient.UpdateSprite(resourceSprite);
            inputIngredient.UpdateCount(inputIngredients[i].count);

            inputIngredient.gameObject.SetActive(true);
        }

        outputIngredientSpriteRenderer.sprite = SpriteProvider.Instance.GetResourceSprite(outputResourceType);
    }

    public void UpdateRequiredIngredients() {
        for (int i = 0; i < RequiredIngredientVisuals.Count; i++) {
            int requiredIngredientCount = requiredIngredientsToUnlock[i].count;
            RequiredIngredientVisuals[i].UpdateCount(requiredIngredientCount);

            if (requiredIngredientCount == 0) {
                RequiredIngredientVisuals[i].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < RequiredIngredientVisuals.Count; i++) {
            if (RequiredIngredientVisuals[i].gameObject.activeInHierarchy) {
                return;
            }
        }

        Debug.Log("Unlocked");
        gameObject.SetActive(false);
    }
}
