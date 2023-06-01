using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour {

    private LineRenderer lineRenderer;
    public LineRenderer LineRenderer { get { return lineRenderer; } }
    private EdgeCollider2D edgeCollider;
    public EdgeCollider2D EdgeCollider { get { return edgeCollider; } }

    private List<Node> connectedNodes = new List<Node>();
    private bool IsConnected { get { return connectedNodes.Count == 2; } }

    List<Vector2> zeroPoints = new List<Vector2> { Vector2.zero, Vector2.zero };

    private void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();
    }

    public void AdjustPoints(List<Vector2> points) {
        AdjustLineRendererPoints(points);
        AdjustEdgeColliderPoints(points);
    }

    private void AdjustLineRendererPoints(List<Vector2> points) {
        lineRenderer.SetPosition(0, points[0]);
        lineRenderer.SetPosition(1, points[1]);
    }

    private void AdjustEdgeColliderPoints(List<Vector2> points) {
        edgeCollider.SetPoints(points);
    }

    public void ConnectNodes(Node node1, Node node2) {
        connectedNodes.Add(node1);
        connectedNodes.Add(node2);
    }

    private void BreakLink() {
        Node node1 = connectedNodes[0];
        Node node2 = connectedNodes[1];

        node1.BreakLink(node2);
        node2.BreakLink(node1);

        connectedNodes.Clear();

        AdjustLineRendererPoints(zeroPoints);
        AdjustEdgeColliderPoints(zeroPoints);

        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;

        gameObject.SetActive(false);
    }

    private void OnMouseEnter() {
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;

        Debug.Log("link sayýsý " + connectedNodes[0].ConnectedNodesLinks[connectedNodes[1]]);
    }

    private void OnMouseExit() {
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;
    }

    private void OnMouseDown() {
        if (IsConnected) {
            BreakLink();
        }
    }
}
