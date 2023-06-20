using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NodeLockedComponents : MonoBehaviour {

    private NodeData nodeData;

    [SerializeField] private Transform requiredIngredientsParent;
    [SerializeField] private Transform requiredIngredientTemplate;
    private List<IngredientVisual> requiredIngredientVisuals = new List<IngredientVisual>();

    [SerializeField] private TextMeshProUGUI earnedLinkCountText;

    [SerializeField] private Transform inputIngredientsParent;
    [SerializeField] private Transform inputIngredientTemplate;

    [SerializeField] private Image arrowImage;
    [SerializeField] private Image outputIngredientImage;

    private void Awake() {
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
        if (nodeData.InputIngredientsDictionary.Count == 0) {
            arrowImage.gameObject.SetActive(false);
            outputIngredientImage.transform.position = arrowImage.transform.position;
        }

        outputIngredientImage.sprite = SpriteProvider.Instance.GetResourceSprite(nodeData.OutputResourceType);
    }

    public void UpdateRequiredIngredientsVisual() {
        foreach (var requiredIngredientVisual in requiredIngredientVisuals) {
            if (nodeData.RequiredIngredientsDictionary.ContainsKey(requiredIngredientVisual.ResourceType)) {
                if (HasRequiredIngredientCompleted(requiredIngredientVisual.ResourceType)) {
                    RequiredIngredientCompleted(requiredIngredientVisual);
                }
                else {
                    requiredIngredientVisual.UpdateCount(nodeData.RequiredIngredientsDictionary[requiredIngredientVisual.ResourceType]);
                }
            }
        }
    }

    private bool HasRequiredIngredientCompleted(ResourceType resourceType) {
        bool hasCompleted = nodeData.RequiredIngredientsDictionary[resourceType] <= 0;
        return hasCompleted;
    }

    private void RequiredIngredientCompleted(IngredientVisual requiredIngredientVisual) {
        requiredIngredientVisual.gameObject.SetActive(false);
    }
}
