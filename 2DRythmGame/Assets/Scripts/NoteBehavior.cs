using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;

public class NoteBehavior : MonoBehaviour
{
    // Start is called before the first frame update

    public int noteType;
    private GameManager.judges judge;
    private string value;
    private string key_value;
    private KeyCode keyCode;
   
    void Start()
    {
        value = GameManager.instance.value;
        if (noteType == 1) key_value = "1";
        else if (noteType == 2) key_value = "2";
        else if (noteType == 3) key_value = "3";
        else if (noteType == 4) key_value = "4";
    }

    public void Initialize()
    {
        judge = GameManager.judges.NONE;    //노트가 활성화되었을때는 판정을 NONE
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * GameManager.instance.noteSpeed); //노트가 정해진 스피드로 매프레임마다 내려감
        //사용자가 노트 키를 입력한 겨우 
        if(key_value == value)
        {
            //해당 노트에 대한 판정을 진행합니다.
            GameManager.instance.processJudge(judge, noteType);
            //노트가 판정 선에 닿기 시작한 이후로는 해당 노트를 제거합니다.
            if (judge != GameManager.judges.NONE) gameObject.SetActive(false);//비활성화
        }
    }

    //각 노트의 현재 위치에 대하여 판정을 수행합니다.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Bad Line")
        {
            judge = GameManager.judges.BAD;
        }
        else if (other.gameObject.tag == "Good Line") 
        {
            judge = GameManager.judges.GOOD;
        }
        else if (other.gameObject.tag == "Perfect Line") 
        {
            judge = GameManager.judges.PERFECT;
        }
        else if(other.gameObject.tag == "Miss Line")
        {
            judge = GameManager.judges.MISS;
            GameManager.instance.processJudge(judge, noteType);
            gameObject.SetActive(false);// 오브젝트를 제거할 필요가 없음 
        }
    }
}
