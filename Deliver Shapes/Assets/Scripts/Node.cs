using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour {

    [SerializeField] private bool isLocked;
    public bool IsLocked { get { return isLocked; } }

    private Dictionary<Node, int> connectedNodesLinks = new Dictionary<Node, int>();
    public Dictionary<Node, int> ConnectedNodesLinks { get { return connectedNodesLinks; } }
    public bool IsConnected { get { return connectedNodesLinks.Count > 0; } }

    private ResourceGenerator resourceGenerator;
    public ResourceGenerator ResourceGenerator { get { return resourceGenerator; } }

    private NodeData nodeData;
    public NodeData NodeData { get { return nodeData; } }

    private NodeVisualManager nodeVisualManager;

    public Image nodeOutline;

    private float nodeRadius;
    public float NodeRadius { get { return nodeRadius; } }

    private void Awake() {
        resourceGenerator = GetComponent<ResourceGenerator>();
        nodeData = GetComponent<NodeData>();
        nodeVisualManager = GetComponent<NodeVisualManager>();
        nodeRadius = GetComponent<CircleCollider2D>().radius;
    }

    private void Start() {
        NodeOutlineManager.Instance.UpdateNodeOutline(this);
    }

    public bool CanConnect(Node nodeToConnect) {
        if (isLocked) {
            if (nodeToConnect.isLocked) {
                return false;
            }

            if (nodeData.RequiredIngredientsDictionary.ContainsKey(nodeToConnect.nodeData.OutputResourceType)) {
                return true;
            }
        }

        else if (!isLocked) {
            if (nodeToConnect.IsLocked) {
                if (nodeToConnect.nodeData.RequiredIngredientsDictionary.ContainsKey(nodeData.OutputResourceType)) {
                    return true;
                }
            }
            else {
                if (nodeToConnect.nodeData.InputIngredientsDictionary.ContainsKey(nodeData.OutputResourceType)) {
                    return true;
                }

                if (nodeData.InputIngredientsDictionary.ContainsKey(nodeToConnect.nodeData.OutputResourceType)) {
                    return true;
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

        NodeOutlineManager.Instance.UpdateNodeOutline(nodeToConnect);
        NodeOutlineManager.Instance.UpdateNodeOutline(this);

        resourceGenerator?.UpdateResourceGenerationTimer();
        nodeToConnect.resourceGenerator?.UpdateResourceGenerationTimer();
    }

    public bool CanGetResource(ResourceType resourceType) {
        if (isLocked) {
            if (!nodeData.RequiredIngredientsDictionary.ContainsKey(resourceType)) {
                return false;
            }
            if (nodeData.RequiredIngredientsDictionary[resourceType] <= 0) {
                return false;
            }
        }
        else {
            if (!nodeData.InputIngredientsDictionary.ContainsKey(resourceType)) {
                return false;
            }

            if (nodeData.CurrentIngredientsDictionary[resourceType] >= nodeData.MaximumResourceCapacity) {
                return false;
            }
        }

        return true;
    }

    public void GetResource(Resource resource) {
        if (nodeData.NodeType == NodeType.FinalNode) {
            GameManager.Instance.Win();
            return;
        }

        if (isLocked) {
            nodeData.RequiredIngredientsDictionary[resource.ResourceType]--;
            nodeVisualManager.UpdateRequiredIngredientsVisual();

            if (CanUnlock()) {
                Unlock();
            }
        }

        else {
            nodeData.CurrentIngredientsDictionary[resource.ResourceType]++;
            if (CanInputsChangeToOutput()) {
                ChangeInputsToOutput();
            }

            nodeVisualManager.UpdateCurrentIngredientsVisual();
        }
    }

    public bool CanUnlock() {
        foreach (var requiredIngredientResourceType in nodeData.RequiredIngredientsDictionary.Keys) {
            if (nodeData.RequiredIngredientsDictionary[requiredIngredientResourceType] > 0) {
                return false;
            }
        }

        return true;
    }

    public void Unlock() {
        nodeVisualManager.HideLockedUI();
        isLocked = false;
        LinkManager.Instance.EarnLink(nodeData.EarnedLinkCount);
        NodeOutlineManager.Instance.UpdateNodeOutline(this);
    }

    private bool CanInputsChangeToOutput() {
        if (nodeData.CurrentIngredientsDictionary[nodeData.OutputResourceType] < nodeData.MaximumResourceCapacity) {
            foreach (var inputIngredientResourceType in nodeData.InputIngredientsDictionary.Keys) {
                if (nodeData.CurrentIngredientsDictionary[inputIngredientResourceType] < nodeData.InputIngredientsDictionary[inputIngredientResourceType]) {
                    return false;
                }
            }

            return true;
        }

        return false;
    }

    private void ChangeInputsToOutput() {
        foreach (var inputIngredientResourceType in nodeData.InputIngredientsDictionary.Keys) {
            nodeData.CurrentIngredientsDictionary[inputIngredientResourceType] -= nodeData.InputIngredientsDictionary[inputIngredientResourceType];
        }

        nodeData.CurrentIngredientsDictionary[nodeData.OutputResourceType]++;
    }

    public void BreakLink(Node connectedNode) {
        if (connectedNodesLinks[connectedNode] > 1) {
            connectedNodesLinks[connectedNode]--;
        }
        else {
            connectedNodesLinks.Remove(connectedNode);
        }

        resourceGenerator?.UpdateResourceGenerationTimer();

        if (!IsConnected) {
            NodeOutlineManager.Instance.UpdateNodeOutline(this);
        }
    }

    private void OnMouseEnter() {
        LinkManager.Instance.PreviewLink(this);
    }

    private void OnMouseExit() {
        LinkManager.Instance.UnpreviewLink();
    }

    private void OnMouseDown() {
        LinkManager.Instance.SelectNode(this);
    }
}
