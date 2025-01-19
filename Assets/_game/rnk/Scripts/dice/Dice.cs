using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _game.rnk.Scripts.dice
{
    public class Dice : MonoBehaviour
    {
        [SerializeField] TMP_Text valueText;
        
        Tween punchTween;
        
        public void Roll()
        {
            StopAllCoroutines();
            StartCoroutine(ActuallyRoll());
        }

        private IEnumerator ActuallyRoll()
        {
            var value = 1 + Random.Range(0, 6); 
            
            G.audio.Play<SFX_Roll>();

            Punch();
            
            yield return new WaitForSeconds(0.10f);

            valueText.text = value.ToString();
        } 

        public void Punch()
        {
            punchTween.Kill(true);
            punchTween=transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.2f);
        }
    }
}
