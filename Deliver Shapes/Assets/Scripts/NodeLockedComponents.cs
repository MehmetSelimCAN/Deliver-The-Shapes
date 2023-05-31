using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NodeLockedComponents : MonoBehaviour {

    private Node node;
    private NodeData nodeData;
    [SerializeField] private NodeUnlockedComponents nodeUnlockedComponents;

    [SerializeField] private Transform requiredIngredientsParent;
    [SerializeField] private Transform requiredIngredientTemplate;
    private List<IngredientVisual> requiredIngredientVisuals = new List<IngredientVisual>();

    [SerializeField] private TextMeshProUGUI earnedLinkCountText;

    [SerializeField] private Transform inputIngredientsParent;
    [SerializeField] private Transform inputIngredientTemplate;

    [SerializeField] private Image outputIngredientImage;

    private void Awake() {
        node = GetComponentInParent<Node>();
        nodeData = GetComponentInParent<NodeData>();

        CreateRequiredIngredientsVisual();
        UpdateEarnLinkCount();
        CreateInputOutputIngredientsVisual();
    }

    private void CreateRequiredIngredientsVisual() {
        for (int i = 0; i < nodeData.RequiredIngredientsToUnlock.Count; i++) {
            var requiredIngredientVisual = Instantiate(requiredIngredientTemplate, requiredIngredientsParent).GetComponent<IngredientVisual>();

            requiredIngredientVisual.ResourceType = nodeData.RequiredIngredientsToUnlock[i].resourceType;
            requiredIngredientVisual.UpdateSprite();
            requiredIngredientVisual.UpdateCount(nodeData.RequiredIngredientsToUnlock[i].count);

            requiredIngredientVisual.gameObject.SetActive(true);
            requiredIngredientVisuals.Add(requiredIngredientVisual);
        }
    }

    private void UpdateEarnLinkCount() {
        earnedLinkCountText.SetText("+" + nodeData.EarnedLinkCount);
    }

    private void CreateInputOutputIngredientsVisual() {
        for (int i = 0; i < nodeData.InputIngredients.Count; i++) {
            var inputIngredient = Instantiate(inputIngredientTemplate, inputIngredientsParent).GetComponent<IngredientVisual>();

            inputIngredient.ResourceType = nodeData.InputIngredients[i].resourceType;
            inputIngredient.UpdateSprite();
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

        nodeUnlockedComponents.gameObject.SetActive(true);
        gameObject.SetActive(false);
        node.Unlock();
    }
}
