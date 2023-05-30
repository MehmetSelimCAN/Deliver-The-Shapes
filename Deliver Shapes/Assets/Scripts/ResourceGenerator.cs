using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour {

    private Node node;
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
        foreach (Node connectedNode in nodeGenerationTimerMax.Keys) {
            nodeGenerationTimer[connectedNode] -= Time.deltaTime;
        }

        if (!node.IsLocked && node.IsConnected) {
            foreach (Node transferTo in node.ConnectedNodesLinks.Keys) {
                if (nodeGenerationTimer[transferTo] < 0) {
                    SpawnResource(transferTo);
                }
            }
        }
    }

    private void SpawnResource(Node transferTo) {
        Resource resource = Instantiate(resourcePrefab, transform.position, resourcePrefab.transform.rotation);
        resource.MoveTo(transferTo);
        nodeGenerationTimer[transferTo] = nodeGenerationTimerMax[transferTo];
    }
}
