using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour {

    private Node node;
    [SerializeField] private Resource resourcePrefab;
    private float resourceGenerationTimerMax = 0.5f;
    private float resourceGenerationTimer = 0.5f;

    private void Awake() {
        node = GetComponent<Node>();
    }

    private void Update() {
        if (!node.IsLocked && node.IsConnected) {
            resourceGenerationTimer -= Time.deltaTime;
            if (resourceGenerationTimer < 0) {
                SpawnResource();
            }
        }
    }

    private void SpawnResource() {
        Resource resource = Instantiate(resourcePrefab, transform.position, resourcePrefab.transform.rotation);
        resource.MoveTo(node.connectedNodes[0]);
        resourceGenerationTimer = resourceGenerationTimerMax;
    }
}
