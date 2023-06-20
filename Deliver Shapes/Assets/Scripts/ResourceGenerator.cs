using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour {

    private Node node;
    private NodeData nodeData;
    private NodeVisualManager nodeVisualManager;

    [SerializeField] private Resource resourcePrefab;

    private Dictionary<Node, float> nodeGenerationTimerMax = new Dictionary<Node, float>();
    private Dictionary<Node, float> nodeGenerationTimer = new Dictionary<Node, float>();
    private float defaultGenerationTimerMax = 0.5f;

    private void Awake() {
        node = GetComponent<Node>();
        nodeData = GetComponent<NodeData>();
        nodeVisualManager = GetComponent<NodeVisualManager>();
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
            if (connectedNode.CanGetResource(nodeData.OutputResourceType)) {
                nodeGenerationTimer[connectedNode] -= Time.deltaTime;
            }
        }

        foreach (Node transferTo in node.ConnectedNodesLinks.Keys) {
            if (nodeGenerationTimer[transferTo] < 0) {
                if (transferTo.CanGetResource(nodeData.OutputResourceType)) {
                    SpawnResource(transferTo);
                }
            }
        }
    }

    private void SpawnResource(Node transferTo) {
        switch (nodeData.NodeType) {
            case NodeType.ConstantGeneratorNode:
                SpawnResourceGameObject(transferTo);
                break;
            case NodeType.DependentGeneratorNode:
                if (nodeData.CurrentIngredientsDictionary[nodeData.OutputResourceType] > 0) {
                    SpawnResourceGameObject(transferTo);
                    nodeData.CurrentIngredientsDictionary[nodeData.OutputResourceType]--;
                    nodeVisualManager.UpdateCurrentIngredientsVisual();
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
