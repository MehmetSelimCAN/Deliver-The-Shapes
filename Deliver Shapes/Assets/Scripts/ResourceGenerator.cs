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
                foreach (Node transferTo in node.connectedNodesLinks.Keys) {
                    for (int i = 0; i < node.connectedNodesLinks[transferTo]; i++) {
                        SpawnResource(transferTo);
                    }
                }
            }
        }
    }

    private void SpawnResource(Node transferTo) {
        Resource resource = Instantiate(resourcePrefab, transform.position, resourcePrefab.transform.rotation);
        resource.MoveTo(transferTo);
        resourceGenerationTimer = resourceGenerationTimerMax;
    }
}
