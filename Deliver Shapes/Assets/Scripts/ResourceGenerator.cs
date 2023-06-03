using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour {

    private Node node;
    [SerializeField] private NodeData nodeData;
    [SerializeField] private NodeUnlockedComponents nodeUnlockedComponents;
    [SerializeField] private Resource resourcePrefab;
    private Dictionary<Node, float> nodeGenerationTimerMax = new Dictionary<Node, float>();
    private Dictionary<Node, float> nodeGenerationTimer = new Dictionary<Node, float>();
    private float defaultGenerationTimerMax = 0.5f;

    private void Awake() {
        node = GetComponent<Node>();
    }

    public void UpdateResourceGenerationTimer() {
        nodeGenerationTimerMax.Clear();
        nodeGenerationTimer.Clear();

        foreach (Node connectedNode in node.ConnectedNodesLinks.Keys) {
            float newGenerationTimer = defaultGenerationTimerMax / node.ConnectedNodesLinks[connectedNode];
            nodeGenerationTimerMax.Add(connectedNode, newGenerationTimer);
            nodeGenerationTimer.Add(connectedNode, newGenerationTimer);
        }
    }

    private void Update() {
        if (node.IsLocked) return;
        if (node.ConnectedNodesLinks.Count == 0) return;

        foreach (Node connectedNode in nodeGenerationTimerMax.Keys) {
            if (connectedNode.CanGetResource(resourcePrefab.ResourceType)) {
                nodeGenerationTimer[connectedNode] -= Time.deltaTime;
            }
        }

        foreach (Node transferTo in node.ConnectedNodesLinks.Keys) {
            if (nodeGenerationTimer[transferTo] < 0) {
                if (transferTo.CanGetResource(resourcePrefab.ResourceType)) {
                    SpawnResource(transferTo);
                }
            }
        }
    }

    private void SpawnResource(Node transferTo) {
        switch (nodeData.nodeType) {
            case NodeData.NodeType.MainNode:
                SpawnResourceGameObject(transferTo);
                break;
            case NodeData.NodeType.Other:
                if (nodeData.CurrentIngredients[resourcePrefab.ResourceType] > 0) {
                    SpawnResourceGameObject(transferTo);
                    nodeData.CurrentIngredients[resourcePrefab.ResourceType]--;
                    nodeUnlockedComponents.UpdateCurrentIngredientsVisual();
                }
                break;
        }
    }

    private void SpawnResourceGameObject(Node transferTo) {
        Resource resource = Instantiate(resourcePrefab, transform.position, resourcePrefab.transform.rotation);
        resource.MoveTo(transferTo);
        nodeGenerationTimer[transferTo] = nodeGenerationTimerMax[transferTo];
    }
}
