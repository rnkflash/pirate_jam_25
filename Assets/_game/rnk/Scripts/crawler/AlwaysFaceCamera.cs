using UnityEngine;

namespace _game.rnk.Scripts.crawler
{
    public class AlwaysFaceCamera : MonoBehaviour
    {
        public bool reverseDirection = false;
        private Camera mainCamera;

        void Start()
        {
            mainCamera = Camera.main;
        }

        void Update()
        {
            if (mainCamera != null)
            {
                // Option 1: Always face camera directly
                transform.LookAt(transform.position - mainCamera.transform.rotation * Vector3.forward, 
                    mainCamera.transform.rotation * Vector3.up);

                // Optionally reverse if needed
                if (reverseDirection)
                {
                    transform.Rotate(0, 180, 0);
                }
            }
        }
    }
}