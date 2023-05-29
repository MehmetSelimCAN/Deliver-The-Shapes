using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour {

    private Node node;
    [SerializeField] private Resource resourcePrefab;
    private float resourceGenerationTimerMax = 1.0f;
    private float resourceGenerationTimer = 0.5f;

    private void Awake() {
        node = GetComponent<Node>();
    }

    private void Update() {
        if (!node.IsLocked && node.IsConnected) {
            resourceGenerationTimer -= Time.deltaTime;
            if (resourceGenerationTimer < 0) {
                for (int i = 0; i < node.connectedNodes.Count; i++) {
                    SpawnResource(node.connectedNodes[i]);
                }
            }
        }
    }

    private void SpawnResource(Node moveToNode) {
        Resource resource = Instantiate(resourcePrefab, transform.position, resourcePrefab.transform.rotation);
        resource.MoveTo(moveToNode);
        resourceGenerationTimer = resourceGenerationTimerMax;
    }
}
