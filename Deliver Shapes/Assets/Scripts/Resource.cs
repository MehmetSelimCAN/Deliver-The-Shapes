using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour {

    [SerializeField] private ResourceType resourceType;
    public ResourceType ResourceType { get { return resourceType; } }
    public float movementSpeed = 20f;

    public void MoveTo(Node node) {
        StartCoroutine(Moving(node));
    }

    private IEnumerator Moving(Node node) {
        while (true) {
            transform.position = Vector2.MoveTowards(transform.position, node.transform.position, movementSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
