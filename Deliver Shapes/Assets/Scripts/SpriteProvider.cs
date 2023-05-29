using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteProvider : MonoBehaviour {

    public static SpriteProvider Instance { get; private set; }

    public Sprite triangleSprite;
    public Sprite squareSprite;
    public Sprite circleSprite;

    private void Awake() {
        Instance = this;
    }

    public Sprite GetResourceSprite(ResourceType resourceType) {
        switch (resourceType) {
            case ResourceType.Triangle:
                return triangleSprite;
            case ResourceType.Square:
                return squareSprite;
            case ResourceType.Circle:
                return circleSprite;
            default:
                return triangleSprite;
        }
    }
}
