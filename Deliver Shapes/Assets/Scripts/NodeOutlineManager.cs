using System.Collections.Generic;
using UnityEngine;

public class NodeOutlineManager : Singleton<NodeOutlineManager> {

    [SerializeField] private Color selectedNodeOutlineColor;
    [SerializeField] private Color UnlockedNodeOutlineColor;
    [SerializeField] private Color UnlockedConnectedNodeOutlineColor;
    [SerializeField] private Color ConnectableNodeOutlineColor;

    public void UpdateNodeOutline(Node node) {
        node.nodeOutline.gameObject.SetActive(true);

        if (LinkManager.Instance.SelectedNode == node) {
            node.nodeOutline.color = selectedNodeOutlineColor;
            return;
        }

        if (node.IsConnected) {
            if (!node.IsLocked) {
                node.nodeOutline.color = UnlockedConnectedNodeOutlineColor;
            }
            else {
                node.nodeOutline.gameObject.SetActive(false);
            }
        }
        else {
            if (!node.IsLocked) {
                node.nodeOutline.color = UnlockedNodeOutlineColor;
            }
            else {
                node.nodeOutline.gameObject.SetActive(false);
            }
        }
    }

    public void HighlightConnectableNodeOutlines(List<Node> connectableNodes) {
        foreach (var node in connectableNodes) {
            HighlightConnectableNodeOutline(node);
        }
    }

    private void HighlightConnectableNodeOutline(Node node) {
        node.nodeOutline.gameObject.SetActive(true);
        node.nodeOutline.color = ConnectableNodeOutlineColor;
    }

    public void UnhighlightConnectableNodeOutlines(List<Node> connectableNodes) {
        foreach (var node in connectableNodes) {
            node.nodeOutline.gameObject.SetActive(false);
            UpdateNodeOutline(node);
        }
    }
}
