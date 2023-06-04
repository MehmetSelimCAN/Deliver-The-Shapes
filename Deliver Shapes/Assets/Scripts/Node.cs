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
    public ResourceGenerator ResourceGenerator { get { return resourceGenerator; } }

    private NodeData nodeData;
    private NodeVisualManager nodeVisualManager;

    private float nodeRadius;
    public float NodeRadius { get { return nodeRadius; } }

    private void Awake() {
        resourceGenerator = GetComponent<ResourceGenerator>();
        nodeData = GetComponent<NodeData>();
        nodeVisualManager = GetComponent<NodeVisualManager>();
        nodeRadius = GetComponent<CircleCollider2D>().radius;
    }

    public bool CanConnect(Node nodeToConnect) {
        if (isLocked) {
            if (nodeToConnect.isLocked) {
                return false;
            }

            foreach (var requiredIngredient in nodeData.RequiredIngredientsToUnlock) {
                if(requiredIngredient.resourceType == nodeToConnect.nodeData.OutputResourceType) {
                    return true;
                }
            }
        }
        else if (!isLocked) {
            if (nodeToConnect.IsLocked) {
                foreach (var requiredIngredient in nodeToConnect.nodeData.RequiredIngredientsToUnlock) {
                    if (requiredIngredient.resourceType == nodeData.OutputResourceType) {
                        return true;
                    }
                }
            }
            else {
                foreach (var inputIngredient in nodeToConnect.nodeData.InputIngredients) {
                    if (inputIngredient.resourceType == nodeData.OutputResourceType) {
                        return true;
                    }
                }

                foreach (var inputIngredient in nodeData.InputIngredients) {
                    if (inputIngredient.resourceType == nodeToConnect.nodeData.OutputResourceType) {
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

    public bool CanGetResource(ResourceType resourceType) {
        if (isLocked) {
            foreach (var requiredIngredient in nodeData.RequiredIngredientsToUnlock) {
                if (requiredIngredient.resourceType == resourceType && requiredIngredient.count > 0) {
                    return true;
                }
            }
        }
        else {
            foreach (var inputIngredient in nodeData.InputIngredients) {
                if (inputIngredient.resourceType == resourceType) {
                    if (nodeData.CurrentIngredients[resourceType] < nodeData.MaximumResourceCapacity) {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public void GetResource(Resource resource) {
        if (nodeData.NodeType == NodeType.Final) {
            Destroy(resource.gameObject);
            Debug.Log("Win");
            return;
        }

        if (isLocked) {
            foreach (var requiredIngredient in nodeData.RequiredIngredientsToUnlock) {
                if (resource.ResourceType == requiredIngredient.resourceType) {
                    requiredIngredient.count--;
                }
            }

            nodeVisualManager.UpdateRequiredIngredientsVisual();
        }

        else {
            if (nodeData.CurrentIngredients.ContainsKey(resource.ResourceType)) {
                if (nodeData.CurrentIngredients[resource.ResourceType] < nodeData.MaximumResourceCapacity) {
                    nodeData.CurrentIngredients[resource.ResourceType]++;
                    nodeVisualManager.UpdateCurrentIngredientsVisual();
                    if (CanInputsChangeToOutput()) {
                        ChangeInputsToOutput();
                    }
                }
            }
        }

        Destroy(resource.gameObject);
    }

    private bool CanInputsChangeToOutput() {
        if (nodeData.CurrentIngredients[nodeData.OutputResourceType] >= nodeData.MaximumResourceCapacity) {
            return false;
        }

        foreach (var inputIngredient in nodeData.InputIngredients) {
            if (nodeData.CurrentIngredients[inputIngredient.resourceType] < inputIngredient.count) {
                return false;
            } 
        }

        return true;
    }

    private void ChangeInputsToOutput() {
        foreach (var inputIngredient in nodeData.InputIngredients) {
            nodeData.CurrentIngredients[inputIngredient.resourceType] -= inputIngredient.count;
        }

        nodeData.CurrentIngredients[nodeData.OutputResourceType]++;
        nodeVisualManager.UpdateCurrentIngredientsVisual();
    }

    public void BreakLink(Node connectedNode) {
        if (connectedNodesLinks[connectedNode] > 1) {
            connectedNodesLinks[connectedNode]--;
        }
        else {
            connectedNodesLinks.Remove(connectedNode);
        }

        resourceGenerator?.UpdateResourceGenerationTimer();
    }

    public void Unlock() {
        isLocked = false;
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
