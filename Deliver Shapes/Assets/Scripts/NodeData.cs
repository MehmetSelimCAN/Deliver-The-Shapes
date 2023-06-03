using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeData : MonoBehaviour {

    public enum NodeType {
        MainNode,
        Other,
    }

    public NodeType nodeType;

    [System.Serializable]
    public class RequiredIngredient {
        public int count;
        public ResourceType resourceType;
    }

    [SerializeField] private List<RequiredIngredient> requiredIngredientsToUnlock = new List<RequiredIngredient>();
    public List<RequiredIngredient> RequiredIngredientsToUnlock { get { return requiredIngredientsToUnlock; } }

    [SerializeField] private int earnedLinkCount;
    public int EarnedLinkCount { get { return earnedLinkCount; } }

    [SerializeField] private List<RequiredIngredient> inputIngredients = new List<RequiredIngredient>();
    public List<RequiredIngredient> InputIngredients { get { return inputIngredients; } }

    [SerializeField] private Dictionary<ResourceType, int> currentIngredients = new Dictionary<ResourceType, int>();
    public Dictionary<ResourceType, int> CurrentIngredients { get { return currentIngredients; } }

    [SerializeField] private ResourceType outputResourceType;
    public ResourceType OutputResourceType { get { return outputResourceType; } }

    [SerializeField] private int maximumResourceCapacity = 10;
    public int MaximumResourceCapacity { get { return maximumResourceCapacity; } }

    private void Awake() {
        InitializeCurrentIngredients();
    }

    private void InitializeCurrentIngredients() {
        foreach (var inputIngredient in inputIngredients) {
            currentIngredients.Add(inputIngredient.resourceType, 0);
        }

        currentIngredients.Add(outputResourceType, 0);
    }
}
