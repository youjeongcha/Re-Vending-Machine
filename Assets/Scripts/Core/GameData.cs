using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//주요 장점:
/*씬 이동해도 전역 데이터 유지됨 (단순 static 방식)
UIManager나 GachaManager에서 GameData.Instance로 바로 접근 가능
복잡한 DontDestroyOnLoad 관리 없이, 데이터만 싱글톤 처리

 
 나중에 챌린지 모드
이벤트 모드
랭킹 연동
같은 확장도 다 커버 가능해.
 */

/*
 * // 게임 도중 점수 업데이트
GameData.Instance.Score += 100;

// 가챠 결과 저장
GameData.Instance.LastGachaCharacter = drawnCharacter;

// UI 출력 시
scoreText.text = GameData.Instance.Score.ToString();
*/

public class GameData
{
    private static GameData _instance;
    //외부에서 GameData.Instance로 접근할 수 있게 만들어주는 프로퍼티
    //null이면 새로 대입해라는 의미 (C# 최신 문법)
    public static GameData Instance => _instance ??= new GameData(); // 싱글톤

    public int Score = 0;
    public int Coin = 0;
    public CharacterData LastGachaCharacter = null;
    public string PlayerID = "guest";

    // 확장 가능 데이터
    public Dictionary<string, object> ExtraData = new();

    private GameData() { }

    public void ResetGame()
    {
        Score = 0;
        LastGachaCharacter = null;
        ExtraData.Clear();
    }
}