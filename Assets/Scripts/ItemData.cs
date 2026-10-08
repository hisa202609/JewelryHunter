using UnityEngine;

[CreateAssetMenu(menuName = "Item/ScoreItem", fileName = "ScoreItem")]
public class ItemData : ScriptableObject
{
    public int value = 0;           // アイテム値
    public string itemName = "";    // アイテム名
    public Sprite itemSprite;       // アイテム画像
}
