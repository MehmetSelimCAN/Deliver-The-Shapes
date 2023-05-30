using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeUnlockedData : MonoBehaviour {

    private NodeData nodeData;

    [SerializeField] private Transform inputIngredientsParent;
    [SerializeField] private Transform inputIngredientTemplate;

    [SerializeField] private Image outputIngredientImage;

    [SerializeField] private Transform currentIngredientsParent;
    [SerializeField] private Transform currentIngredientTemplate;
    private List<IngredientVisual> currentIngredientVisuals = new List<IngredientVisual>();

    private void Awake() {
        nodeData = GetComponentInParent<NodeData>();

        CreateInputOutputIngredientsVisual();
        CreateCurrentIngredientsVisual();
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

    private void CreateCurrentIngredientsVisual() {
        CreateCurrentInputIngredientsVisual();
        CreateCurrentOutputIngredientsVisual();
    }

    private void CreateCurrentInputIngredientsVisual() {
        for (int i = 0; i < nodeData.InputIngredients.Count; i++) {
            var inputIngredient = Instantiate(currentIngredientTemplate, currentIngredientsParent).GetComponent<IngredientVisual>();

            Sprite inputResourceSprite = SpriteProvider.Instance.GetResourceSprite(nodeData.InputIngredients[i].resourceType);
            inputIngredient.UpdateSprite(inputResourceSprite);
            inputIngredient.UpdateCount(0);
            currentIngredientVisuals.Add(inputIngredient);
            inputIngredient.gameObject.SetActive(true);
        }
    }

    private void CreateCurrentOutputIngredientsVisual() {
        var outputIngredient = Instantiate(currentIngredientTemplate, currentIngredientsParent).GetComponent<IngredientVisual>();

        Sprite outputResourceSprite = SpriteProvider.Instance.GetResourceSprite(nodeData.OutputResourceType);
        outputIngredient.UpdateSprite(outputResourceSprite);
        outputIngredient.UpdateCount(0);
        currentIngredientVisuals.Add(outputIngredient);
        outputIngredient.gameObject.SetActive(true);
    }
}
