using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    // Start is called before the first frame update
    //하나의 노트에 대한 정보를 담는 노트(Note) 클래스를 정의
    class Note
    {
        public int noteType { get; set; }
        public int order { get; set; }
        public Note(int noteType,int order)
        {
            this.noteType = noteType;
            this.order = order;
        }
    
    }
    private ObjectPooler noteObjectPooler;
    private float x, z, startY = 8.0f;  //화면에 위쪽에서 
    public GameObject[] Notes;//노트 프리펩에 담아서 떨어뜨리도록
    private List<Note> notes = new List<Note>();//실제로 떨어지게 될 노트를 담는 List
    private float beatInterval = 1.0f;  //노트간의 시간 간격 
    
    void MakeNote(Note note)
    {
        GameObject obj = noteObjectPooler.getObject(note.noteType);
        //설정된 시작 라인으로 노트를 이동시킵니다.
        x = obj.transform.position.x;
        z = obj.transform.position.z;
        obj.transform.position = new Vector3(x, startY, z);//위치값을 강제로 이위치로 변경 
        obj.GetComponent<NoteBehavior>().Initialize();
        obj.SetActive(true);
    }
    IEnumerator AwaitMakeNote(Note note)
    {//특정시간을 기다렸다가 노트가 떨어지게 (코루틴에서 쓰는 자료형) 
        int noteType = note.noteType;
        int order = note.order;
        yield return new WaitForSeconds(order * beatInterval);//유니티엔진에게 특정 명령 (몇초동안 기다린동안 아래쪽 명령어를 실행하라
        MakeNote(note);
    }
    void Start()
    {
        noteObjectPooler = gameObject.GetComponent<ObjectPooler>();
        notes.Add(new Note(1, 1));  
        notes.Add(new Note(2, 2));
        notes.Add(new Note(3, 3));
        notes.Add(new Note(4, 4));
        notes.Add(new Note(1, 5));
        notes.Add(new Note(2, 6));
        notes.Add(new Note(3, 7));
        notes.Add(new Note(4, 8));
        //모든 노트를 정해진 시간에 출발하도록 설정
        for(int i = 0; i < notes.Count; i++)
        {
            StartCoroutine(AwaitMakeNote(notes[i]));//특정시간에 실제로 노트가 생성될수있도록 
        }
       

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
