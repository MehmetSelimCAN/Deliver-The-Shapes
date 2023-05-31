using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    [SerializeField] private bool isLocked;
    public bool IsLocked { get { return isLocked; } }

    private Dictionary<Node, int> connectedNodesLinks = new Dictionary<Node, int>();
    public Dictionary<Node, int> ConnectedNodesLinks { get { return connectedNodesLinks; } }

    private ResourceGenerator resourceGenerator;

    private NodeData nodeData;
    [SerializeField] private NodeUnlockedComponents nodeUnlockedComponents;
    [SerializeField] private NodeLockedComponents nodeLockedComponents;

    private float nodeRadius;
    public float NodeRadius { get { return nodeRadius; } }

    private void Awake() {
        resourceGenerator = GetComponent<ResourceGenerator>();
        nodeData = GetComponent<NodeData>();
        nodeRadius = GetComponent<CircleCollider2D>().radius;
    }

    public bool TryToConnect(Node nodeToConnect) {
        if (isLocked) {
            foreach (var requiredIngredient in nodeData.RequiredIngredientsToUnlock) {
                if(requiredIngredient.resourceType == nodeToConnect.nodeData.OutputResourceType) {
                    ConnectNode(nodeToConnect);
                    return true;
                }
            }
        }
        else if (!isLocked) {
            foreach (var inputIngredient in nodeData.InputIngredients) {
                if (inputIngredient.resourceType == nodeToConnect.nodeData.OutputResourceType) {
                    ConnectNode(nodeToConnect);
                    return true;
                }
            }
            foreach (var inputIngredient in nodeToConnect.nodeData.InputIngredients) {
                if (inputIngredient.resourceType == nodeData.OutputResourceType) {
                    ConnectNode(nodeToConnect);
                    return true;
                }
            }
            if (nodeToConnect.IsLocked) {
                foreach (var requiredIngredient in nodeToConnect.nodeData.RequiredIngredientsToUnlock) {
                    if (requiredIngredient.resourceType == nodeData.OutputResourceType) {
                        ConnectNode(nodeToConnect);
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public void ConnectNode(Node nodeToConnect) {
        if (!connectedNodesLinks.ContainsKey(nodeToConnect)) {
            connectedNodesLinks.Add(nodeToConnect, 0);
        }
        if (!nodeToConnect.connectedNodesLinks.ContainsKey(this)) {
            nodeToConnect.connectedNodesLinks.Add(this, 0);
        }

        connectedNodesLinks[nodeToConnect]++;
        nodeToConnect.connectedNodesLinks[this]++;

        resourceGenerator?.UpdateResourceGenerationTimer();
        nodeToConnect.resourceGenerator?.UpdateResourceGenerationTimer();
    }

    public bool CanGetResource(Resource resource) {
        if (isLocked) {
            if (nodeData.RequiredIngredientsToUnlock.Find(x => x.resourceType == resource.ResourceType && x.count > 0) != null) {
                return true;
            }
        }
        else {
            if (nodeUnlockedComponents.CollectableIngredients.ContainsKey(resource.ResourceType)) {
                if (nodeUnlockedComponents.CollectableIngredients[resource.ResourceType] < nodeData.MaximumResourceCapacity) {
                    return true;
                }
            }
        }

        return false;
    }

    public void GetResource(Resource resource) {
        if (isLocked) {
            foreach (var requiredIngredient in nodeData.RequiredIngredientsToUnlock) {
                if (resource.ResourceType == requiredIngredient.resourceType) {
                    requiredIngredient.count--;
                }
            }
            nodeLockedComponents.UpdateRequiredIngredients();
        }

        else {
            if (nodeUnlockedComponents.CollectableIngredients.ContainsKey(resource.ResourceType)) {
                if (nodeUnlockedComponents.CollectableIngredients[resource.ResourceType] < nodeData.MaximumResourceCapacity) {
                    nodeUnlockedComponents.CollectableIngredients[resource.ResourceType]++;
                    nodeUnlockedComponents.UpdateCurrentIngredientsVisual();
                }
            }
        }

        Destroy(resource.gameObject);
    }

    public void Unlock() {
        isLocked = false;
    }

    private void OnMouseDown() {
        LinkManager.Instance.SelectNode(this);
    }
}
