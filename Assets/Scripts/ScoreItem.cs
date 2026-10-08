using UnityEngine;

public class ScoreItem : MonoBehaviour
{
    public ItemData itemdata;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = itemdata.itemSprite;
    }
}
