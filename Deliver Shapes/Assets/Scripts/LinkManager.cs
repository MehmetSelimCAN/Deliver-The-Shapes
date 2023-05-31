using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class LinkManager : MonoBehaviour {

    public static LinkManager Instance { get; private set; }

    [SerializeField] private Link[] links;
    private Link availableLink;

    private Node selectedNode;

    private void Awake() {
        Instance = this;
        availableLink = FindAvailableLink();
    }

    private void Update() {
        if (selectedNode != null) {
            availableLink.LineRenderer.SetPosition(1, GetMousePosition());
        }
    }

    public void SelectNode(Node node) {
        //If the selected nodes are same, do nothing.
        if (selectedNode != null && node == selectedNode) {
            selectedNode = null;
            return;
        }


        if (selectedNode == null) {
            selectedNode = node;
            availableLink.LineRenderer.SetPosition(0, node.transform.position);
            return;
        }

        ConnectTwoNodes(selectedNode, node);
    }

    private void ConnectTwoNodes(Node node1, Node node2) {
        if (node1.TryToConnect(node2)) {
            CreateLink(node1, node2);
            selectedNode = null;
        }
    }

    private void CreateLink(Node node1, Node node2) {
        Vector2 pointA = node1.transform.position;
        Vector2 pointB = node2.transform.position;
        List<Vector2> points = new List<Vector2>{ pointA, pointB };

        availableLink.AdjustPoints(points);
        availableLink.ConnectNodes(node1, node2);

        availableLink = FindAvailableLink();
    }

    private Link FindAvailableLink() {
        foreach (Link link in links) {
            if (!link.gameObject.activeInHierarchy) {
                link.gameObject.SetActive(true);
                return link;
            }
        }

        return null;
    }

    private Vector2 GetMousePosition() {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return mousePosition;
    }
}
