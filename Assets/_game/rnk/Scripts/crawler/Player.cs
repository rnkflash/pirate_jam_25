using System;
using UnityEngine;

namespace _game.rnk.Scripts.crawler
{
    public class Player: MonoBehaviour
    {
        InputHandler inputHandler;
        MovementQueue movementQueue;

        void Awake()
        {
            inputHandler = GetComponent<InputHandler>();
            movementQueue = GetComponent<MovementQueue>();
        }

        public void EnableControls()
        {
            inputHandler.enabled = true;
        }
        
        public void DisableControls()
        {
            inputHandler.enabled = false;
            movementQueue.FlushQueue();
        }

        public void OnEncounter(Encounter encounter)
        {
            G.crawler.OnEncounter(encounter);
        }
    }
}