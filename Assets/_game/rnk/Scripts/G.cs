using _game.rnk.Scripts.battleSystem;
using UnityEngine;

namespace _game.rnk.Scripts
{
    public static class G
    {
        public static Main main;
        public static AudioSystem audio;
        public static Crawler crawler;
        public static Battle battle;
        public static HUD hud;
        public static UI ui;
        public static Savefile save;
        public static CameraHandle camera;
        public static Feel feel;
        public static ScreenFader fader;
        public static RunState run;
        public static bool IsPaused;
        public static DiceInteractiveObject drag_dice;
        public static DiceInteractiveObject hover_dice;
    }

    public class ManagedBehaviour : MonoBehaviour
    {
        void Update()
        {
            if (!G.IsPaused)
                PausableUpdate();
        }

        protected virtual void PausableUpdate()
        {
        }

        void FixedUpdate()
        {
            if (!G.IsPaused)
                PausableFixedUpdate();
        }

        protected virtual void PausableFixedUpdate()
        {
        }
    }
}