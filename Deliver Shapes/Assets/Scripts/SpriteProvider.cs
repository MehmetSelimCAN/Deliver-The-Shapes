using UnityEngine;

public class SpriteProvider : Singleton<SpriteProvider> {

    public Sprite triangleSprite;
    public Sprite squareSprite;
    public Sprite circleSprite;

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
