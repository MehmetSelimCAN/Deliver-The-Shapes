using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeUnlockedComponents : MonoBehaviour {

    private NodeData nodeData;

    [SerializeField] private Transform inputIngredientsParent;
    [SerializeField] private Transform inputIngredientTemplate;

    [SerializeField] private Image arrowImage;
    [SerializeField] private Image outputIngredientImage;

    [SerializeField] private Transform currentIngredientsParent;
    [SerializeField] private Transform currentIngredientTemplate;
    private List<IngredientVisual> currentIngredientsVisual = new List<IngredientVisual>();

    private void Awake() {
        nodeData = GetComponentInParent<NodeData>();
    }

    private void Start() {
        if (nodeData.NodeType == NodeType.Other) {
            CreateInputIngredientsVisual();
            CreateCurrentIngredientsVisual();
        }

        CreateOutputIngredientVisual();
    }

    private void CreateInputIngredientsVisual() {
        foreach (var inputIngredient in nodeData.InputIngredients) {
            var inputIngredientVisual = Instantiate(inputIngredientTemplate, inputIngredientsParent).GetComponent<IngredientVisual>();

            inputIngredientVisual.ResourceType = inputIngredient.resourceType;
            inputIngredientVisual.UpdateSprite();
            inputIngredientVisual.UpdateCount(inputIngredient.count);
            inputIngredientVisual.gameObject.SetActive(true);
        }
    }

    private void CreateOutputIngredientVisual() {
        outputIngredientImage.sprite = SpriteProvider.Instance.GetResourceSprite(nodeData.OutputResourceType);
        if (nodeData.NodeType == NodeType.Main) {
            outputIngredientImage.gameObject.transform.position = transform.position;
            arrowImage.gameObject.SetActive(false);
        }
    }

    private void CreateCurrentIngredientsVisual() {
        foreach (var currentIngredient in nodeData.CurrentIngredients) {
            var currentIngredientVisual = Instantiate(currentIngredientTemplate, currentIngredientsParent).GetComponent<IngredientVisual>();

            currentIngredientVisual.ResourceType = currentIngredient.Key;
            currentIngredientVisual.UpdateSprite();
            currentIngredientVisual.UpdateCount();

            currentIngredientsVisual.Add(currentIngredientVisual);
            currentIngredientVisual.gameObject.SetActive(true);
        }
    }

    public void UpdateCurrentIngredientsVisual() {
        foreach (var ingredientVisual in currentIngredientsVisual) {
            ingredientVisual.UpdateCount(nodeData.CurrentIngredients[ingredientVisual.ResourceType]);
        }
    }
}
