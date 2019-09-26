using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject gameBackground;
    private SpriteRenderer gameBackgroundSpriteRenderer;

    void Start()
    {
        gameBackgroundSpriteRenderer = gameBackground.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeOut(gameBackgroundSpriteRenderer,0.005f));//배경이 희미해지는 효과
    }

    IEnumerator FadeOut(SpriteRenderer spriteRenderer, float amount)
    {
        Color color = spriteRenderer.color;
        while(color.a > 0.0f)
        {
            color.a -= amount;
            spriteRenderer.color = color;
            yield return new WaitForSeconds(amount);
        }//매프레임마다 0.005초씩 줄어듬
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
