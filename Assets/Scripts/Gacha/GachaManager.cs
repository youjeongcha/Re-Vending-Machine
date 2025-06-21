using System.Collections.Generic;
using UnityEngine;



public class GachaManager : MonoBehaviour
{
    public List<CharacterData> characterPool;
    public GameObject gachaResultPopup;
    public GachaTable gachaTable;
    /*Assets > Create > Game > GachaTable�� ���� ����s
    entries ����Ʈ�� CharacterData�� weight, rarity �Է�
    GachaManager���� ����ó�� ȣ��:
    */

    public void Roll()
    {
        if (CoinManager.Instance.UseCoins(100))
        {
            /*int index = Random.Range(0, characterPool.Count);
            CharacterData drawn = characterPool[index];*/
    CharacterData drawn = gachaTable.GetRandomCharacter();
            CollectionManager.Instance.Register(drawn);
            UIManager.Instance.ShowGachaResult(drawn);
        }
    }
}