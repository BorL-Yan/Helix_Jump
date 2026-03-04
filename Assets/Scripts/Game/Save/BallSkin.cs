
using UnityEngine;

[System.Serializable]
public class BallSkin
{
    public BallSkinType skinType;
    public GameObject skin;
    public Sprite sprite;

    public BallSkin()
    {
        skinType = BallSkinType.Sphere;
    }

    public BallSkin(BallSkinType type, GameObject skin)
    {
        skinType = type;
        this.skin = skin;
    }
}
