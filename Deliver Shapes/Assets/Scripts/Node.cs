using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    private bool isLocked;

    private void OnMouseDown() {
        if (!isLocked) {
            LinkManager.Instance.SelectNode(this);
        }
    }
}
