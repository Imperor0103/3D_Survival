// 사용하지 않는 using문 지워라
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private static CharacterManager _instance;
    public static CharacterManager Instance
    {
        get
        {
            if (_instance == null)  // 방어코드
            {
                /// 씬에 GameObject를 올려놓지 않더라도 Player의 Awake에서 호출하여 생성한다
                _instance = new GameObject("CharacterManager").AddComponent<CharacterManager>();
            }
            return _instance;
        }
    }
    public Player _player;
    public Player Player
    {
        get { return _player; }
        set { _player = value; }
    }
    private void Awake()
    {
        // Awake가 실행되고 있다면, 이미 게임오브젝트에 스크립트가 붙어있는 상태로 실행이 된 것이니, 게임오브젝트 생성할 필요 없이 자신의 스크립트 연결
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else // 이미 _instance가 있는 경우, 현재 생성하고 있는 것을 파괴
        {
            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
