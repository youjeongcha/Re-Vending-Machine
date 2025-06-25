using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ֿ� ����:
/*�� �̵��ص� ���� ������ ������ (�ܼ� static ���)
UIManager�� GachaManager���� GameData.Instance�� �ٷ� ���� ����
������ DontDestroyOnLoad ���� ����, �����͸� �̱��� ó��

 
 ���߿� ç���� ���
�̺�Ʈ ���
��ŷ ����
���� Ȯ�嵵 �� Ŀ�� ������.
 */

/*
 * // ���� ���� ���� ������Ʈ
GameData.Instance.Score += 100;

// ��í ��� ����
GameData.Instance.LastGachaCharacter = drawnCharacter;

// UI ��� ��
scoreText.text = GameData.Instance.Score.ToString();
*/

public class GameData
{
    private static GameData _instance;
    //�ܺο��� GameData.Instance�� ������ �� �ְ� ������ִ� ������Ƽ
    //null�̸� ���� �����ض�� �ǹ� (C# �ֽ� ����)
    public static GameData Instance => _instance ??= new GameData(); // �̱���

    public int Score = 0;
    public int Coin = 0;
    public CharacterData LastGachaCharacter = null;
    public string PlayerID = "guest";

    // Ȯ�� ���� ������
    public Dictionary<string, object> ExtraData = new();

    private GameData() { }

    public void ResetGame()
    {
        Score = 0;
        LastGachaCharacter = null;
        ExtraData.Clear();
    }
}