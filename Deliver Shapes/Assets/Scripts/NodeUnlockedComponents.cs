using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeUnlockedComponents : MonoBehaviour {

    private NodeData nodeData;

    [SerializeField] private Transform inputIngredientsParent;
    [SerializeField] private Transform inputIngredientTemplate;

    [SerializeField] private Image outputIngredientImage;

    [SerializeField] private Transform currentIngredientsParent;
    [SerializeField] private Transform currentIngredientTemplate;
    private List<IngredientVisual> currentIngredientVisuals = new List<IngredientVisual>();
    private Dictionary<ResourceType, int> currentIngredients = new Dictionary<ResourceType, int>();
    public Dictionary<ResourceType, int> CurrentIngredients { get { return currentIngredients; } }

    private void Awake() {
        nodeData = GetComponentInParent<NodeData>();

        CreateInputOutputIngredientsVisual();
        CreateCurrentIngredientsVisual();
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

    private void CreateCurrentIngredientsVisual() {
        CreateCurrentInputIngredientsVisual();
        CreateCurrentOutputIngredientsVisual();
    }

    private void CreateCurrentInputIngredientsVisual() {
        for (int i = 0; i < nodeData.InputIngredients.Count; i++) {
            var inputIngredient = Instantiate(currentIngredientTemplate, currentIngredientsParent).GetComponent<IngredientVisual>();


            inputIngredient.ResourceType = nodeData.InputIngredients[i].resourceType;
            inputIngredient.UpdateSprite();
            inputIngredient.UpdateCount(0);
            currentIngredientVisuals.Add(inputIngredient);
            currentIngredients.Add(inputIngredient.ResourceType, 0);
            inputIngredient.gameObject.SetActive(true);
        }
    }

    private void CreateCurrentOutputIngredientsVisual() {
        var outputIngredient = Instantiate(currentIngredientTemplate, currentIngredientsParent).GetComponent<IngredientVisual>();

        outputIngredient.ResourceType = nodeData.OutputResourceType;
        outputIngredient.UpdateSprite();
        outputIngredient.UpdateCount(0);
        currentIngredientVisuals.Add(outputIngredient);
        currentIngredients.Add(outputIngredient.ResourceType, 0);
        outputIngredient.gameObject.SetActive(true);
    }

    public void UpdateCurrentIngredientsVisual() {
        foreach (var ingredientVisual in currentIngredientVisuals) {
            ingredientVisual.UpdateCount(currentIngredients[ingredientVisual.ResourceType]);
        }
    }
}
