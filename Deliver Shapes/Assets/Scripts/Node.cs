using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    [SerializeField] private bool isLocked;
    public bool IsLocked { get { return isLocked; } }

    [SerializeField] private bool isConnected;
    public bool IsConnected { get { return isConnected; } }

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

    public void ConnectNode(Node nodeToConnect) {
        if (!connectedNodesLinks.ContainsKey(nodeToConnect)) {
            connectedNodesLinks.Add(nodeToConnect, 0);
        }
        if (!nodeToConnect.connectedNodesLinks.ContainsKey(this)) {
            nodeToConnect.connectedNodesLinks.Add(this, 0);
        }

        isConnected = true;
        nodeToConnect.isConnected = true;

        connectedNodesLinks[nodeToConnect]++;
        nodeToConnect.connectedNodesLinks[this]++;

        resourceGenerator?.UpdateResourceGenerationTimer();
        nodeToConnect.resourceGenerator?.UpdateResourceGenerationTimer();
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
            if (nodeUnlockedComponents.CollectableIngredients[resource.ResourceType] < nodeData.MaximumResourceCapacity) {
                nodeUnlockedComponents.CollectableIngredients[resource.ResourceType]++;
                nodeUnlockedComponents.UpdateCurrentIngredientsVisual();
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
