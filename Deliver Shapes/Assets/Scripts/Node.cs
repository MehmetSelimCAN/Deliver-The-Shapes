using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    [SerializeField] private bool isLocked;
    public bool IsLocked { get { return isLocked; } }

    [SerializeField] private bool isConnected;
    public bool IsConnected { get { return isConnected; } }

    [SerializeField] public Dictionary<Node, int> connectedNodesLinks = new Dictionary<Node, int>();

    private ResourceGenerator resourceGenerator;

    private NodeLockedData lockedData;

    private float nodeRadius;
    public float NodeRadius { get { return nodeRadius; } }

    private void Awake() {
        resourceGenerator = GetComponent<ResourceGenerator>();
        lockedData = GetComponentInChildren<NodeLockedData>();

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
            foreach (var requiredIngredient in lockedData.RequiredIngredientsToUnlock) {
                if (resource.ResourceType == requiredIngredient.resourceType) {
                    requiredIngredient.count--;
                }
            }
        }

        lockedData.UpdateRequiredIngredients();
    }

    private void OnMouseDown() {
        LinkManager.Instance.SelectNode(this);
    }
}
