using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeData : MonoBehaviour {

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

    [SerializeField] private ResourceType outputResourceType;
    public ResourceType OutputResourceType { get { return outputResourceType; } }

    [SerializeField] private int maximumResourceCapacity = 10;
    public int MaximumResourceCapacity { get { return maximumResourceCapacity; } }

}
