using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HudTextColor
{
    White,
    Red,
    Green,
    Yellow
}
public class HudText : MonoBehaviour 
{
    public UILabel Text;
    public Color WhiteColor;
    public Color WhiteColorOutline;
    public Color RedColor;
    public Color RedColorOutline;
    public Color GreenColor;
    public Color GreenColorOutline;
    public Color YellowColor;
    public Color YellowColorOutline;

    void Start()
    {
        gameObject.SetActive(false);
    }
    public void Show(string text, HudTextColor color = HudTextColor.White, float delay = 1.0f)
    {
        Text.text = text;

        switch(color)
        {
            case HudTextColor.White:
                Text.color = WhiteColor;
                Text.effectColor = WhiteColorOutline;
                break;
            case HudTextColor.Green:
                Text.color = GreenColor;
                Text.effectColor = GreenColorOutline;
                break;
            case HudTextColor.Red:
                Text.color = RedColor;
                Text.effectColor = RedColorOutline;
                break;
            case HudTextColor.Yellow:
                Text.color = YellowColor;
                Text.effectColor = YellowColorOutline;
                break;
        }

        gameObject.SetActive(true);
        gameObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

        StopAllCoroutines();
        StartCoroutine(ShowHudText(delay));
    }

    private IEnumerator ShowHudText(float delay)
    {
        TweenAlpha.Begin(gameObject, 0.1f, 1.0f);
        yield return new WaitForSeconds(0.1f);
        TweenScale.Begin(gameObject, 0.2f, new Vector3(1.2f, 1.2f, 1.2f));
        yield return new WaitForSeconds(0.2f);
        TweenScale.Begin(gameObject, 0.2f, Vector3.one);
        yield return new WaitForSeconds(delay);
        TweenAlpha.Begin(gameObject, 0.2f, 0.0f);
        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(false);
    }
}
