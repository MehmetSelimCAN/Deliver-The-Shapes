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
    public int MaximumLinkCount { get { return maximumLinkCount; } }

    [SerializeField] private int currentUsingLinkCount;
    public int CurrentUsingLinkCount { get { return currentUsingLinkCount; } }

    private Node selectedNode;
    public Node SelectedNode { get { return selectedNode; } }
    private bool isPreviewingLink;

    private LinkUI linkUI;

    private Node[] allNodes;

    private void Awake() {
        Instance = this;
        availableLink = FindAvailableLink();

        linkUI = GetComponent<LinkUI>();

        allNodes = FindObjectsOfType<Node>();
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
            List<Node> connectableNodes = FindConnectableNodes(node);
            NodeOutlineManager.Instance.UpdateNodeOutline(node);
            NodeOutlineManager.Instance.UnhighlightConnectableNodeOutlines(connectableNodes);
            return;
        }


        if (selectedNode == null) {
            selectedNode = node;
            availableLink.LineRenderer.SetPosition(0, node.transform.position);
            List<Node> connectableNodes = FindConnectableNodes(selectedNode);
            NodeOutlineManager.Instance.UpdateNodeOutline(selectedNode);
            NodeOutlineManager.Instance.HighlightConnectableNodeOutlines(connectableNodes);
            return;
        }

        ConnectTwoNodes(selectedNode, node);
    }

    private void ConnectTwoNodes(Node node1, Node node2) {
        if (node1.CanConnect(node2)) {
            selectedNode = null;
            node1.ConnectNode(node2);
            CreateLink(node1, node2);
        }
    }

    private void CreateLink(Node node1, Node node2) {
        Vector2 pointA = node1.transform.position;
        Vector2 pointB = node2.transform.position;
        List<Vector2> points = new List<Vector2>{ pointA, pointB };

        availableLink.AdjustPoints(points);
        availableLink.ConnectNodes(node1, node2);

        currentUsingLinkCount++;
        linkUI.UpdateLinkCountText();

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
        currentUsingLinkCount--;
        linkUI.UpdateLinkCountText();

        availableLink = FindAvailableLink();
    }

    public void EarnLink(int earnedLinkCount) {
        maximumLinkCount += earnedLinkCount;
        linkUI.UpdateLinkCountText();

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

    public List<Node> FindConnectableNodes(Node selectedNode) {
        List<Node> connectableNodes = new List<Node>();

        foreach (var node in allNodes) {
            if (node.IsLocked) {
                if (node.NodeData.RequiredIngredientsDictionary.ContainsKey(selectedNode.NodeData.OutputResourceType)) {
                    connectableNodes.Add(node);
                }
            }
            else {
                if (node.NodeData.InputIngredientsDictionary.ContainsKey(selectedNode.NodeData.OutputResourceType)) {
                    connectableNodes.Add(node);
                }
            }
        }

        return connectableNodes;
    }

    private Vector2 GetMousePosition() {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return mousePosition;
    }
}
