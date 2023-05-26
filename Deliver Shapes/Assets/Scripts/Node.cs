using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    [SerializeField] private bool isLocked;
    public bool IsLocked { get { return isLocked; } }

    [SerializeField] private bool isConnected;
    public bool IsConnected { get { return isConnected; } }

    [SerializeField] public List<Node> connectedNodes;

    public void ConnectNode(Node nodeToConnect) {
        isConnected = true;
        connectedNodes.Add(nodeToConnect);
    }

    private void OnMouseDown() {
        if (!isLocked) {
            LinkManager.Instance.SelectNode(this);
        }
    }
}
