using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class LinkManager : MonoBehaviour {

    public static LinkManager Instance { get; private set; }

    [SerializeField] private Transform linksParent;
    [SerializeField] private List<Link> links;
    [SerializeField] private Link linkPrefab;
    private Link availableLink;
    [SerializeField] private int maximumLinkCount;

    private Node selectedNode;
    private bool isPreviewingLink;

    private void Awake() {
        Instance = this;
        availableLink = FindAvailableLink();
    }

    private void Update() {
        if (availableLink == null) return;

        if (selectedNode != null && !isPreviewingLink) {
            availableLink.LineRenderer.SetPosition(1, GetMousePosition());
        }
    }

    public void SelectNode(Node node) {
        if (availableLink == null) return;

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
        if (node1.CanConnect(node2)) {
            node1.ConnectNode(node2);
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
        for (int i = 0; i < maximumLinkCount; i++) {
            if (!links[i].IsConnected) {
                links[i].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < maximumLinkCount; i++) {
            if (!links[i].gameObject.activeInHierarchy) {
                links[i].gameObject.SetActive(true);
                return links[i];
            }
        }

        return null;
    }

    public void PreviewLink(Node node) {
        if (selectedNode != null && selectedNode.CanConnect(node)) {
            availableLink.LineRenderer.SetPosition(1, node.transform.position);
            isPreviewingLink = true;
        }
    }

    public void UnpreviewLink() {
        isPreviewingLink = false;
    }

    public void BreakLink() {
        availableLink = FindAvailableLink();
    }

    public void EarnLink(int earnedLinkCount) {
        maximumLinkCount += earnedLinkCount;
        CreateLinks(earnedLinkCount);
    }

    private void CreateLinks(int earnedLinkCount) {
        for (int i = 0; i < earnedLinkCount; i++) {
            Link newLink = Instantiate(linkPrefab, linksParent);
            links.Add(newLink);
        }

        if (availableLink == null) {
            availableLink = FindAvailableLink();
        }
    }

    private Vector2 GetMousePosition() {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return mousePosition;
    }
}
