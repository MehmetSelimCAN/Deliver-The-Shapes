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
        CreateInputIngredientsVisual();
        CreateCurrentIngredientsVisual();
        CreateOutputIngredientVisual();
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

    private void CreateCurrentIngredientsVisual() {
        foreach (var currentIngredientResourceType in nodeData.CurrentIngredientsDictionary.Keys) {
            var currentIngredientVisual = Instantiate(currentIngredientTemplate, currentIngredientsParent).GetComponent<IngredientVisual>();

            currentIngredientVisual.UpdateResourceType(currentIngredientResourceType);
            currentIngredientVisual.UpdateSprite();
            currentIngredientVisual.UpdateCount();

            currentIngredientsVisual.Add(currentIngredientVisual);
            currentIngredientVisual.gameObject.SetActive(true);
        }
    }

    private void CreateOutputIngredientVisual() {
        outputIngredientImage.sprite = SpriteProvider.Instance.GetResourceSprite(nodeData.OutputResourceType);
        if (nodeData.NodeType == NodeType.Main) {
            outputIngredientImage.gameObject.transform.position = transform.position;
            arrowImage.gameObject.SetActive(false);
        }
    }

    public void UpdateCurrentIngredientsVisual() {
        foreach (var ingredientVisual in currentIngredientsVisual) {
            ingredientVisual.UpdateCount(nodeData.CurrentIngredientsDictionary[ingredientVisual.ResourceType]);
        }
    }
}
