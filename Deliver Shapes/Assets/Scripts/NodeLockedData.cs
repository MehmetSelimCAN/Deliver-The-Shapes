using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NodeLockedData : MonoBehaviour {

    private NodeData nodeData;
    [SerializeField] private NodeUnlockedData unlockedData;

    [SerializeField] private Transform requiredIngredientsParent;
    [SerializeField] private Transform requiredIngredientTemplate;
    private List<IngredientVisual> requiredIngredientVisuals = new List<IngredientVisual>();

    [SerializeField] private TextMeshProUGUI earnedLineCountText;

    [SerializeField] private Transform inputIngredientsParent;
    [SerializeField] private Transform inputIngredientTemplate;

    [SerializeField] private Image outputIngredientImage;

    private void Awake() {
        nodeData = GetComponentInParent<NodeData>();

        CreateRequiredIngredientsVisual();
        UpdateEarnLineCount();
        CreateInputOutputIngredientsVisual();
    }

    private void CreateRequiredIngredientsVisual() {
        for (int i = 0; i < nodeData.RequiredIngredientsToUnlock.Count; i++) {
            var requiredIngredientVisual = Instantiate(requiredIngredientTemplate, requiredIngredientsParent).GetComponent<IngredientVisual>();

            Sprite resourceSprite = SpriteProvider.Instance.GetResourceSprite(nodeData.RequiredIngredientsToUnlock[i].resourceType);
            requiredIngredientVisual.UpdateSprite(resourceSprite);
            requiredIngredientVisual.UpdateCount(nodeData.RequiredIngredientsToUnlock[i].count);

            requiredIngredientVisual.gameObject.SetActive(true);
            requiredIngredientVisuals.Add(requiredIngredientVisual);
        }
    }

    private void UpdateEarnLineCount() {
        earnedLineCountText.SetText("+" + nodeData.EarnedLineCount);
    }

    private void CreateInputOutputIngredientsVisual() {
        for (int i = 0; i < nodeData.InputIngredients.Count; i++) {
            var inputIngredient = Instantiate(inputIngredientTemplate, inputIngredientsParent).GetComponent<IngredientVisual>();

            Sprite resourceSprite = SpriteProvider.Instance.GetResourceSprite(nodeData.InputIngredients[i].resourceType);
            inputIngredient.UpdateSprite(resourceSprite);
            inputIngredient.UpdateCount(nodeData.InputIngredients[i].count);

            inputIngredient.gameObject.SetActive(true);
        }

        outputIngredientImage.sprite = SpriteProvider.Instance.GetResourceSprite(nodeData.OutputResourceType);
    }

    public void UpdateRequiredIngredients() {
        for (int i = 0; i < requiredIngredientVisuals.Count; i++) {
            int requiredIngredientCount = nodeData.RequiredIngredientsToUnlock[i].count;
            requiredIngredientVisuals[i].UpdateCount(requiredIngredientCount);

            if (requiredIngredientCount == 0) {
                requiredIngredientVisuals[i].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < requiredIngredientVisuals.Count; i++) {
            if (requiredIngredientVisuals[i].gameObject.activeInHierarchy) {
                return;
            }
        }

        unlockedData.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
