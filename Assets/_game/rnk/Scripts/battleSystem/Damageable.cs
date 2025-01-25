using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _game.rnk.Scripts.battleSystem
{
    public class Damageable : MonoBehaviour
    {
        [SerializeField] GameObject deadge;
        [SerializeField] TMP_Text healthText;
        [SerializeField] Image avatarImage;
        [NonSerialized] public BaseCharacterState state;
        
        Tween punchTween;

        public void SetState(BaseCharacterState newState)
        {
            state = newState;
            SetDead(state.dead);
        }
        
        public void UpdateView()
        {
            healthText.text = state.health + "/" + state.maxHealth + " +" + state.armor;
        }
        
        public void SetDead(bool orly)
        {
            deadge.SetActive(orly);
            avatarImage.gameObject.SetActive(!orly);
            state.dead = orly;
            UpdateView();
        }

        public IEnumerator Hit(int dmg)
        {
            G.hud.meleeHit.Bahni(transform.position);
            G.audio.Play<SFX_GetDamage>();
            var initialArmor = state.armor;
            state.armor -= dmg;
            if (state.armor <= 0)
            {
                state.armor = 0;
                
                state.health -= dmg - initialArmor;
                if (state.health <= 0)
                {
                    state.health = 0;
                    SetDead(true);
                }    
            }
            
            UpdateView();
            Punch();
            yield return new WaitUntil(() => punchTween == null);
        }
        
        public IEnumerator Heal(int hp)
        {
            G.hud.healHit.Bahni(transform.position);
            G.audio.Play<SFX_Heal>();
            
            state.health += hp;
            if (state.health > state.maxHealth)
            {
                state.health = state.maxHealth;
            }
            UpdateView();
            Punch();
            yield return new WaitUntil(() => punchTween == null);
        }
        
        public IEnumerator Armor(int armor)
        {
            G.hud.armorHit.Bahni(transform.position);
            G.audio.Play<SFX_Armor>();
            
            state.armor += armor;
            
            UpdateView();
            Punch();
            yield return new WaitUntil(() => punchTween == null);
        }
        
        public void Punch()
        {
            punchTween?.Kill(true);
            punchTween=transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.2f).OnComplete(() => {
                punchTween = null;
            });
        }
    }
}