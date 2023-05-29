using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    [SerializeField] private bool isLocked;
    public bool IsLocked { get { return isLocked; } }

    [SerializeField] private bool isConnected;
    public bool IsConnected { get { return isConnected; } }

    [SerializeField] public Dictionary<Node, int> connectedNodesLinks = new Dictionary<Node, int>();

    public void ConnectNode(Node nodeToConnect) {
        if (!connectedNodesLinks.ContainsKey(nodeToConnect)) {
            connectedNodesLinks.Add(nodeToConnect, 0);
            nodeToConnect.ConnectNode(this);
            isConnected = true;
        }

        connectedNodesLinks[nodeToConnect]++;
    }

    private void OnMouseDown() {
        if (!isLocked) {
            LinkManager.Instance.SelectNode(this);
        }
    }
}
