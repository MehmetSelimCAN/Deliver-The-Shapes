using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LinkUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI linkCountText;

    [SerializeField] private Image hasAvailableLinkIndicator;
    [SerializeField] private Color notAvailableLinkColor;
    [SerializeField] private Color availableLinkColor;

    public void UpdateLinkCountText() {
        int currentUsingLinkCount = LinkManager.Instance.CurrentUsingLinkCount;
        int maximumLinkCount = LinkManager.Instance.MaximumLinkCount;

        linkCountText.SetText(currentUsingLinkCount + " / " + maximumLinkCount);

        UpdateAvaliableLinkIndicatorColor();
    }

    private void UpdateAvaliableLinkIndicatorColor() {
        int currentUsingLinkCount = LinkManager.Instance.CurrentUsingLinkCount;
        int maximumLinkCount = LinkManager.Instance.MaximumLinkCount;
        if (currentUsingLinkCount < maximumLinkCount) {
            hasAvailableLinkIndicator.color = availableLinkColor;
            linkCountText.color = Color.white;
        }
        else {
            hasAvailableLinkIndicator.color = notAvailableLinkColor;
            linkCountText.color = notAvailableLinkColor;
        }
    }
}
