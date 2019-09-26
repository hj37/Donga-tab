using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    // Start is called before the first frame update
    //리스트 사용하기 리스트가 4개가 존재함 4개오를 하나로 또 모아서 만들수있음 2차원 리스트로 만들기 
    //Note 1: 10개 => 리스트
    //Note 2: 10개 => 리스트
    //Note 3: 10개 => 리스트
    //Note 4: 10개 => 리스트
    //오브젝트 풀링 기법 사용 
    public List<GameObject> Notes;
    private List<List<GameObject>> poolsOfNotes;
    public int noteCount = 10;
    private bool more = true;
    void Start()
    {
        poolsOfNotes = new List<List<GameObject>>();    //비어있는 리스트들의 리스트를 생성 
        for(int i = 0; i < Notes.Count; i++)    //4번반복  //반복문의 시작.Notes라는 리스트의 개수만큼 아래 작업 반복
        {
            poolsOfNotes.Add(new List<GameObject>());//poolsOfNotes에 빈 리스트를 원소로 추가
            for(int n = 0; n < noteCount; n++)//10번 반복 //반복문 시작.noteCount만큼 아래 작업 반복
            {
                GameObject obj = Instantiate(Notes[i]);//Notes의 i번째 원소를 생성, obj에 할당
                obj.SetActive(false);//obj 비활성화
                poolsOfNotes[i].Add(obj);//poolsOfNotes의 i번째 원소에 obj추가
            }//poolsOfNotes의 첫번째 원소에 obj가 10개 
        }//채워지면, poolsOfNotes의 두번째 원소를 생성해서 똑같이 obj를 10개 채워넣음 
    }//=> 최종적으로 PoolsOfNotes에는 노트가 10개씩 들어있는 리스트 4개가 원소로 추가됨 

    // Update is called once per frame

    public GameObject getObject(int noteType)
    {
        foreach(GameObject obj in poolsOfNotes[noteType-1])
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }
        if(more)    //필요한만큼 설정해줌 40개보다 많아졌을때
        {
            GameObject obj = Instantiate(Notes[noteType - 1]);
            poolsOfNotes[noteType - 1].Add(obj);
            return obj;
        }
        return null; 
    }
    void Update()
    {
        
    }
}
