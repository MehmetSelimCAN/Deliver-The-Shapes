using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeData : MonoBehaviour {

    [SerializeField] private NodeType nodeType;
    public NodeType NodeType { get { return nodeType; } }

    [System.Serializable]
    public class RequiredIngredient {
        public int count;
        public ResourceType resourceType;
    }

    [SerializeField] private List<RequiredIngredient> requiredIngredientsToUnlock = new List<RequiredIngredient>();
    private Dictionary<ResourceType, int> requiredIngredientsDictionary = new Dictionary<ResourceType, int>();
    public Dictionary<ResourceType, int> RequiredIngredientsDictionary { get { return requiredIngredientsDictionary; } }

    [SerializeField] private int earnedLinkCount;
    public int EarnedLinkCount { get { return earnedLinkCount; } }

    [SerializeField] private List<RequiredIngredient> inputIngredients = new List<RequiredIngredient>();
    private Dictionary<ResourceType, int> inputIngredientsDictionary = new Dictionary<ResourceType, int>();
    public Dictionary<ResourceType, int> InputIngredientsDictionary { get { return inputIngredientsDictionary; } }

    [SerializeField] private Dictionary<ResourceType, int> currentIngredientsDictionary = new Dictionary<ResourceType, int>();
    public Dictionary<ResourceType, int> CurrentIngredientsDictionary { get { return currentIngredientsDictionary; } }

    [SerializeField] private ResourceType outputResourceType;
    public ResourceType OutputResourceType { get { return outputResourceType; } }

    private int maximumResourceCapacity = 10;
    public int MaximumResourceCapacity { get { return maximumResourceCapacity; } }

    private void Awake() {
        InitializeInputIngridentsDictionary();
        InitializeCurrentIngredientsDictionary();
        InitializeRequiredIngridentsDictionary();
    }

    private void InitializeInputIngridentsDictionary() {
        foreach (var inputIngredient in inputIngredients) {
            inputIngredientsDictionary.Add(inputIngredient.resourceType, inputIngredient.count);
        }
    }

    private void InitializeRequiredIngridentsDictionary() {
        foreach (var requiredIngredient in requiredIngredientsToUnlock) {
            requiredIngredientsDictionary.Add(requiredIngredient.resourceType, requiredIngredient.count);
        }
    }

    private void InitializeCurrentIngredientsDictionary() {
        foreach (var inputIngredient in inputIngredients) {
            currentIngredientsDictionary.Add(inputIngredient.resourceType, 0);
        }

        if (nodeType == NodeType.DependentGeneratorNode) {
            currentIngredientsDictionary.Add(outputResourceType, 0);
        }
    }
}
