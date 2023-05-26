using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour {

    private LineRenderer lineRenderer;
    public LineRenderer LineRenderer { get { return lineRenderer; } }
    private EdgeCollider2D edgeCollider;
    public EdgeCollider2D EdgeCollider { get { return edgeCollider; } }

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

    private void OnMouseDown() {
        Debug.Log("Linke týklandý");
    }
}
