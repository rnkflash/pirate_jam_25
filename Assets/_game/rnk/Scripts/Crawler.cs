using System;
using _game.rnk.Scripts.battleSystem;
using _game.rnk.Scripts.crawler;
using UnityEngine;

namespace _game.rnk.Scripts
{
    public class Crawler : MonoBehaviour
    {
        public Player player;

        Encounter currentEncounter;
        
        void Awake()
        {
            G.crawler = this;
        }

        public void OnEncounter(Encounter encounter)
        {
            currentEncounter = encounter;
            switch (encounter)
            {
                case BattleEncounter battleEncounter:
                    player.DisableControls();
                    G.battle.StartBattle(battleEncounter);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(encounter));

            }
        }

        public void OnFinishEncounter()
        {
            currentEncounter.CleanUp();
            currentEncounter = null;
            player.EnableControls();
        }
        
        public void CharacterClicked(CharacterState state)
        {
            if (state.dead) return;
            if (!G.hud.inventory.gameObject.activeSelf)
            {
                player.DisableControls();
                G.hud.ShowInventory(state);
            }
            else
            {
                if (G.hud.inventory.state == state)
                {
                    G.hud.HideInventory();
                    player.EnableControls();
                }
                else
                {
                    G.hud.HideInventory();
                    G.hud.ShowInventory(state);
                }
            }
        }
    }
}