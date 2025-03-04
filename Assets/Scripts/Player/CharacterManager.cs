// ������� �ʴ� using�� ������
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private static CharacterManager _instance;
    public static CharacterManager Instance
    {
        get
        {
            if (_instance == null)  // ����ڵ�
            {
                /// ���� GameObject�� �÷����� �ʴ��� Player�� Awake���� ȣ���Ͽ� �����Ѵ�
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
        // Awake�� ����ǰ� �ִٸ�, �̹� ���ӿ�����Ʈ�� ��ũ��Ʈ�� �پ��ִ� ���·� ������ �� ���̴�, ���ӿ�����Ʈ ������ �ʿ� ���� �ڽ��� ��ũ��Ʈ ����
        if (_instance != null)  // �����ߴ�
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else // �̹� _instance�� �ִ� ���, ���� �����ϰ� �ִ� ���� �ı�
        {
            Destroy(gameObject);
        }
    }
}
