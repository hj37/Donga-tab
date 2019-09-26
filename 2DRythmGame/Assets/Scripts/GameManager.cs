using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Windows.Forms;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{//싱글톤 기법으로 구현함
    SerialPort sp = new SerialPort("COM3", 115200);
    public static GameManager instance { get; set; }
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);//오류발견시 파괴
    }

    public float noteSpeed;
    // Start is called before the first frame update
    /*
     * BAD : 1
     * GOOD : 2
     * PERFECT : 3
     * MISS : 4
     */

    public GameObject scoreUI;
    private float score;
    private Text scoreText;
    public GameObject comboUI;
    private int combo;
    private Text ComboText;
    private Animator comboAnimator;


    public enum judges { NONE = 0, BAD, GOOD, PERFECT, MISS };//판정값 초기에는 0, BAD라는 문자는 1 
    public GameObject judgeUI;
    private Sprite[] judgeSprites;
    private Image judgementSpriteRenderer;
    private Animator judgementSpriteAnimator;


    public GameObject[] trails;
    private SpriteRenderer[] trailSpriteRenderers;
    public string value;
    public KeyCode keycode;
    void Start()
    {
        sp.Open();
        sp.ReadTimeout = 1;

        judgementSpriteRenderer = judgeUI.GetComponent<Image>();
        judgementSpriteAnimator = judgeUI.GetComponent<Animator>();
        scoreText = scoreUI.GetComponent<Text>();
        ComboText = comboUI.GetComponent<Text>();
        comboAnimator = comboUI.GetComponent<Animator>();
        //판정 결과를 보여주는 스프라이트 이미지를 미리 초기화합니다.
        judgeSprites = new Sprite[4];
        judgeSprites[0] = Resources.Load<Sprite>("Sprites/Bad");
        judgeSprites[1] = Resources.Load<Sprite>("Sprites/Good");
        judgeSprites[2] = Resources.Load<Sprite>("Sprites/Miss");
        judgeSprites[3] = Resources.Load<Sprite>("Sprites/Perfect");
        //리소스폴더에 있는걸 특정한걸 가져옴 

        trailSpriteRenderers = new SpriteRenderer[trails.Length];
        for (int i = 0; i < trails.Length; i++)
        {
            trailSpriteRenderers[i] = trails[i].GetComponent<SpriteRenderer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (sp.IsOpen)

        {

            try

            {


                value = sp.ReadLine();

                print(value);

                sp.BaseStream.Flush();

            }

            catch (System.Exception)

            {



            }

        }
      

        //사용자가 입력한 키에 해당하는 라인을 빛나게 처리합니다.
        if (value == "1") ShineTrail(0);
        if (value == "2") ShineTrail(1);
        if (value == "3") ShineTrail(2);
        if (value == "4") ShineTrail(3);
        //한 번 빛나게 된 라인은 반족적으로 다시 어둡게 처리합니다.

        for (int i = 0;
            i < trailSpriteRenderers.Length; i++)
        {
            Color color = trailSpriteRenderers[i].color;
            color.a -= 0.01f;
            trailSpriteRenderers[i].color = color;
        }
    }
    // 특정한 키를 눌러 해당 라인을 빛나게 처리합니다.
    public void ShineTrail(int index)
    {
        Color color = trailSpriteRenderers[index].color;
        color.a = 0.32f;
        trailSpriteRenderers[index].color = color;
    }
    //노트 판정 이후에 판정 결과를 화면에 보여줍니다.
    void showJudgement()
    {
        //점수 이미지를 보여줍니다.
        string scoreFormat = "000000";
        scoreText.text = score.ToString(scoreFormat);
        // 판정 이미지를 보여줍니다.
        judgementSpriteAnimator.SetTrigger("Show");
        //콤보가 2이상일때만 콤보 이미지를 보여줍니다.
        if (combo >= 2)
        {
            ComboText.text = "COMBO" + combo.ToString();
            comboAnimator.SetTrigger("Show");
        }
    }
    //노트 판정을 진행합니다.
    public void processJudge(judges judge, int noteType)
    {
        if (judge == judges.NONE) return;
        //MISS 판정을 받은 경우 콤보를 종료하고, 점수를 많이 깍습니다.
        if (judge == judges.MISS)
        {
            judgementSpriteRenderer.sprite = judgeSprites[2];
            combo = 0;
            if (score >= 15) score -= 15;

        }
        //BAD 판정을 받은 경우 콤보를 종료하고 , 점수를 조금 깍습니다.
        else if (judge == judges.BAD)
        {
            judgementSpriteRenderer.sprite = judgeSprites[0];
            combo = 0;
            if (score >= 5) score -= 5;
        }
        //PERFECT 혹은 GOOD 판정을 받은 경우 콤보 및 점수를 올립니다.
        else
        {
            if (judge == judges.PERFECT)
            {
                judgementSpriteRenderer.sprite = judgeSprites[3];
                score += 20;
            }
            else if (judge == judges.GOOD)
            {
                judgementSpriteRenderer.sprite = judgeSprites[1];
                score += 15;
            }
            combo += 1;
            score += (float)combo * 0.1f;
        }
        showJudgement();

    }

}
