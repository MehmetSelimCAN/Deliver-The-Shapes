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
    }

    private void Start() {
        CreateRequiredIngredientsVisual();
        UpdateEarnLinkCount();
        CreateInputOutputIngredientsVisual();
    }

    private void CreateRequiredIngredientsVisual() {
        foreach (var requiredIngredient in nodeData.RequiredIngredientsToUnlock) {
            var requiredIngredientVisual = Instantiate(requiredIngredientTemplate, requiredIngredientsParent).GetComponent<IngredientVisual>();

            requiredIngredientVisual.ResourceType = requiredIngredient.resourceType;
            requiredIngredientVisual.UpdateSprite();
            requiredIngredientVisual.UpdateCount(requiredIngredient.count);

            requiredIngredientVisual.gameObject.SetActive(true);
            requiredIngredientVisuals.Add(requiredIngredientVisual);
        }
    }

    private void UpdateEarnLinkCount() {
        earnedLinkCountText.SetText("+" + nodeData.EarnedLinkCount);
    }

    private void CreateInputOutputIngredientsVisual() {
        foreach (var inputIngredient in nodeData.InputIngredients) {
            var inputIngredientVisual = Instantiate(inputIngredientTemplate, inputIngredientsParent).GetComponent<IngredientVisual>();

            inputIngredientVisual.ResourceType = inputIngredient.resourceType;
            inputIngredientVisual.UpdateSprite();
            inputIngredientVisual.UpdateCount(inputIngredient.count);

            inputIngredientVisual.gameObject.SetActive(true);
        }

        outputIngredientImage.sprite = SpriteProvider.Instance.GetResourceSprite(nodeData.OutputResourceType);
    }

    public void UpdateRequiredIngredientsVisual() {
        foreach (var requiredIngredientVisual in requiredIngredientVisuals) {
            foreach (var requiredIngredient in nodeData.RequiredIngredientsToUnlock) {
                if (requiredIngredientVisual.ResourceType == requiredIngredient.resourceType) {
                    if (requiredIngredient.count == 0) {
                        requiredIngredientVisual.gameObject.SetActive(false);
                    }
                    else {
                        requiredIngredientVisual.UpdateCount(requiredIngredient.count);
                    }
                }
            }
        }

        if (IsUnlockable()) {
            UnlockNode();
        }
    }

    private bool IsUnlockable() {
        foreach (var requiredIngredientVisual in requiredIngredientVisuals) {
            if (requiredIngredientVisual.gameObject.activeInHierarchy) {
                return false;
            }
        }

        return true;
    }

    private void UnlockNode() {
        nodeUnlockedComponents.gameObject.SetActive(true);
        gameObject.SetActive(false);
        node.Unlock();
    }
}
