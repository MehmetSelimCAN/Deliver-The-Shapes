using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour {

    [SerializeField] private ResourceType resourceType;
    public ResourceType ResourceType { get { return resourceType; } }
    private float movementSpeed = 10f;

    public void MoveTo(Node node) {
        StartCoroutine(Moving(node));
    }

    private IEnumerator Moving(Node node) {
        while (Vector2.Distance(transform.position, node.transform.position) > node.NodeRadius) {
            transform.position = Vector2.MoveTowards(transform.position, node.transform.position, movementSpeed * Time.deltaTime);

            yield return null;
        }

        if (node.CanGetResource(resourceType)) {
            node.GetResource(this);
        }

        Destroy(gameObject);
    }
}
