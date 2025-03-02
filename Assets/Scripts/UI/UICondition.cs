using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICondition : MonoBehaviour
{
    public Condition health;
    public Condition hunger;
    public Condition stamina;

    // Start is called before the first frame update
    void Start()
    {
        // player의 uiCondition에 현재 오브젝트를 대입
        CharacterManager.Instance.Player.condition.uiCondition = this;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
