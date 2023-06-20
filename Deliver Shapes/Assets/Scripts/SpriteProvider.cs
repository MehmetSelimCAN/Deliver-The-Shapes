using UnityEngine;

public class SpriteProvider : Singleton<SpriteProvider> {

    public Sprite triangleSprite;
    public Sprite squareSprite;
    public Sprite circleSprite;
    public Sprite pentagonSprite;
    public Sprite hexagonSprite;
    public Sprite starSprite;

    public Sprite GetResourceSprite(ResourceType resourceType) {
        switch (resourceType) {
            case ResourceType.Triangle:
                return triangleSprite;
            case ResourceType.Square:
                return squareSprite;
            case ResourceType.Circle:
                return circleSprite;
            case ResourceType.Pentagon:
                return pentagonSprite;
            case ResourceType.Hexagon:
                return hexagonSprite;
            case ResourceType.Star:
                return starSprite;
            default:
                return triangleSprite;
        }
    }
}
