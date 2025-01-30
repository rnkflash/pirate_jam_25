using System;
using UnityEngine;

namespace _game.rnk.Scripts.crawler
{
    public class AlwaysFollowWorldSpaceObject : MonoBehaviour
    {
        public Transform followTarget;
        Camera camera;

        void Start()
        {
            camera = Camera.main;
        }
        void Update()
        {
            if (followTarget != null)
            {
                Vector3 screenPos = camera.WorldToScreenPoint(followTarget.position);
                //Vector3 uiPos = new Vector3(screenPos.x, Screen.height - screenPos.y, screenPos.z);
                transform.position = screenPos;
            }
        }
    }
}