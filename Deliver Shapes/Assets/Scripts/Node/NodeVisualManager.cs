using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeVisualManager : MonoBehaviour {

    [SerializeField] private NodeLockedComponents nodeLockedComponents;
    [SerializeField] private NodeUnlockedComponents nodeUnlockedComponents;

    public void UpdateRequiredIngredientsVisual() {
        nodeLockedComponents.UpdateRequiredIngredientsVisual();
    }

    public void UpdateCurrentIngredientsVisual() {
        nodeUnlockedComponents.UpdateCurrentIngredientsVisual();
    }

    public void ShowLockedUI() {
        nodeUnlockedComponents.gameObject.SetActive(false);
        nodeLockedComponents.gameObject.SetActive(true);
    }

    public void HideLockedUI() {
        nodeLockedComponents.gameObject.SetActive(false);
        nodeUnlockedComponents.gameObject.SetActive(true);
    }
}
