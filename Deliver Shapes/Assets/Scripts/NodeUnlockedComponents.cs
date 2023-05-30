using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeUnlockedComponents : MonoBehaviour {

    private NodeData nodeData;

    [SerializeField] private Transform inputIngredientsParent;
    [SerializeField] private Transform inputIngredientTemplate;

    [SerializeField] private Image outputIngredientImage;

    [SerializeField] private Transform collectableIngredientsParent;
    [SerializeField] private Transform collectableIngredientTemplate;
    private List<IngredientVisual> collectableIngredientsVisual = new List<IngredientVisual>();

    private Dictionary<ResourceType, int> collectableIngredients = new Dictionary<ResourceType, int>();
    public Dictionary<ResourceType, int> CollectableIngredients { get { return collectableIngredients; } }


    private void Awake() {
        nodeData = GetComponentInParent<NodeData>();

        CreateInputIngredientsVisual();
        CreateOutputIngredientVisual();
        CreateCurrentIngredientsVisual();
    }

    private void CreateInputIngredientsVisual() {
        for (int i = 0; i < nodeData.InputIngredients.Count; i++) {
            var inputIngredient = Instantiate(inputIngredientTemplate, inputIngredientsParent).GetComponent<IngredientVisual>();

            inputIngredient.ResourceType = nodeData.InputIngredients[i].resourceType;
            inputIngredient.UpdateSprite();
            inputIngredient.UpdateCount(nodeData.InputIngredients[i].count);
            collectableIngredients.Add(inputIngredient.ResourceType, 0);
            inputIngredient.gameObject.SetActive(true);
        }
    }

    private void CreateOutputIngredientVisual() {
        outputIngredientImage.sprite = SpriteProvider.Instance.GetResourceSprite(nodeData.OutputResourceType);
        collectableIngredients.Add(nodeData.OutputResourceType, 0);
    }

    private void CreateCurrentIngredientsVisual() {
        foreach (var collectableIngredientResourceType in collectableIngredients.Keys) {
            var collectableIngredient = Instantiate(collectableIngredientTemplate, collectableIngredientsParent).GetComponent<IngredientVisual>();

            collectableIngredient.ResourceType = collectableIngredientResourceType;
            collectableIngredient.UpdateSprite();
            collectableIngredient.UpdateCount();

            collectableIngredientsVisual.Add(collectableIngredient);
            collectableIngredient.gameObject.SetActive(true);
        }
    }

    public void UpdateCurrentIngredientsVisual() {
        foreach (var ingredientVisual in collectableIngredientsVisual) {
            ingredientVisual.UpdateCount(CollectableIngredients[ingredientVisual.ResourceType]);
        }
    }
}
