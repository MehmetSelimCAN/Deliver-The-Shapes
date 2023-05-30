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

    [SerializeField] private int earnedLineCount;
    public int EarnedLineCount { get { return earnedLineCount; } }

    [SerializeField] private List<RequiredIngredient> inputIngredients = new List<RequiredIngredient>();
    public List<RequiredIngredient> InputIngredients { get { return inputIngredients; } }

    [SerializeField] private ResourceType outputResourceType;
    public ResourceType OutputResourceType { get { return outputResourceType; } }

}
