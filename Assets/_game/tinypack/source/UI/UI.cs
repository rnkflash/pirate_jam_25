using System.Collections;
using _game.rnk.Scripts;
using _game.rnk.Scripts.battleSystem;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public TextMeshProUGUI debug_text;
    public UIPauseMenu pause;
    public GameObject win;
    public GameObject defeat;

    public TutorialMask tutorial;
    
    public Image hitLight;

    public GameObject disableInput;
    
    public GameObject click_to_continue;
    
    public TMP_Text say_text;
    public TMP_Text say_text_shadow;

    void Awake()
    {
        G.ui = this;
        click_to_continue.SetActive(false);
        tutorial.Hide();
    }

    void Start()
    {
        Reset();
    }

    void Reset()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause.Toggle();
        }
    }

    public void Punch(Transform healthTransform)
    {
        healthTransform.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.2f);
    }

    public void QPunch(Transform healthTransform)
    {
        healthTransform.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.1f);
    }

    public IEnumerator ScaleCountIn(Transform healthValueTransform)
    {
        healthValueTransform.transform.DOKill(true);
        yield return new WaitForEndOfFrame();
        yield return healthValueTransform.transform.DOScale(Vector3.one * 1.2f, 0.01f).WaitForCompletion();
    }
    
    public IEnumerator ScaleCountOut(Transform healthValueTransform)
    {
        healthValueTransform.transform.DOKill(true);
        yield return healthValueTransform.transform.DOScale(Vector3.one * 1f, 0.1f).WaitForCompletion();
    }

    public void DisableInput()
    {
        disableInput.SetActive(true);
    }
    
    public void EnableInput()
    {
        disableInput.SetActive(false);
    }
    
    public IEnumerator Say(string text)
    {
        StartCoroutine(Print(say_text, text));
        yield return Print(say_text_shadow, text);
    }

    public IEnumerator Unsay()
    {
        StartCoroutine(Unprint(say_text, say_text.text));
        yield return Unprint(say_text_shadow, say_text_shadow.text);
    }
    
    public static IEnumerator Print(TMP_Text text, string actionDefinition, string fx = "wave")
    {
        var visibleLength = TextUtils.GetVisibleLength(actionDefinition);
        if (visibleLength == 0) yield break;

        for (var i = 0; i < visibleLength; i++)
        {
            text.text = $"<link={fx}>{TextUtils.CutSmart(actionDefinition, 1 + i)}</link>";
            yield return new WaitForEndOfFrame();

            G.audio.Play<SFX_TypeChar>();
        }
    }

    IEnumerator Unprint(TMP_Text text, string actionDefinition)
    {
        var visibleLength = TextUtils.GetVisibleLength(actionDefinition);
        if (visibleLength == 0) yield break;

        var str = "";

        for (var i = visibleLength - 1; i >= 0; i--)
        {
            str = TextUtils.CutSmart(actionDefinition, i);
            text.text = $"<link=wave>{str}</link>";
            yield return new WaitForEndOfFrame();
        }

        text.text = "";
    }


    public void AdjustSay(float i)
    {
        say_text.transform.DOMoveY(i, 0.25f);
    }
}