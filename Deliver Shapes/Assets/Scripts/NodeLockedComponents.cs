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
        CreateInputIngredientsVisual();
        CreateOutputIngredientsVisual();
    }

    private void CreateRequiredIngredientsVisual() {
        foreach (var requiredIngredientResourceType in nodeData.RequiredIngredientsDictionary.Keys) {
            var requiredIngredientVisual = Instantiate(requiredIngredientTemplate, requiredIngredientsParent).GetComponent<IngredientVisual>();

            requiredIngredientVisual.UpdateResourceType(requiredIngredientResourceType);
            requiredIngredientVisual.UpdateSprite();
            requiredIngredientVisual.UpdateCount(nodeData.RequiredIngredientsDictionary[requiredIngredientResourceType]);

            requiredIngredientVisual.gameObject.SetActive(true);
            requiredIngredientVisuals.Add(requiredIngredientVisual);
        }
    }

    private void UpdateEarnLinkCount() {
        earnedLinkCountText.SetText("+" + nodeData.EarnedLinkCount);
    }

    private void CreateInputIngredientsVisual() {
        foreach (var inputIngredientResourceType in nodeData.InputIngredientsDictionary.Keys) {
            var inputIngredientVisual = Instantiate(inputIngredientTemplate, inputIngredientsParent).GetComponent<IngredientVisual>();

            inputIngredientVisual.UpdateResourceType(inputIngredientResourceType);
            inputIngredientVisual.UpdateSprite();
            inputIngredientVisual.UpdateCount(nodeData.InputIngredientsDictionary[inputIngredientResourceType]);

            inputIngredientVisual.gameObject.SetActive(true);
        }
    }

    private void CreateOutputIngredientsVisual() {
        outputIngredientImage.sprite = SpriteProvider.Instance.GetResourceSprite(nodeData.OutputResourceType);
    }

    public void UpdateRequiredIngredientsVisual() {
        foreach (var requiredIngredientVisual in requiredIngredientVisuals) {
            if (nodeData.RequiredIngredientsDictionary.ContainsKey(requiredIngredientVisual.ResourceType)) {
                if (nodeData.RequiredIngredientsDictionary[requiredIngredientVisual.ResourceType] <= 0) {
                    requiredIngredientVisual.gameObject.SetActive(false);
                }
                else {
                    requiredIngredientVisual.UpdateCount(nodeData.RequiredIngredientsDictionary[requiredIngredientVisual.ResourceType]);
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
