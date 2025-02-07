using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _game.rnk.Scripts.battle.dice
{
    public class PhysicsRaycaster3D : MonoBehaviour
    {
        Camera cam;
        public LayerMask mask;

        Transform currentTarget;

        void Awake()
        {
            cam = GetComponent<Camera>();
        }

        void Update()
        {
            /*Vector3 mousePos = Input.mousePosition;
            mousePos.z = 100f;
            mousePos = cam.ScreenToWorldPoint(mousePos);
            Debug.DrawRay(transform.position, mousePos - transform.position,             
                Color.blue);*/

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray,out hit,100,mask))
            {
                if (currentTarget == null)
                {
                    currentTarget = hit.transform;
                    currentTarget.GetComponent<IPointerEnterHandler>()?.OnPointerEnter(null);
                }
                else
                {
                    if (currentTarget != hit.transform)
                    {
                        currentTarget.GetComponent<IPointerExitHandler>()?.OnPointerExit(null);
                        currentTarget = hit.transform;
                        currentTarget.GetComponent<IPointerEnterHandler>()?.OnPointerEnter(null);
                    }
                }
            }
            else
            {
                if (currentTarget != null)
                {
                    currentTarget.GetComponent<IPointerExitHandler>()?.OnPointerExit(null);
                }
                currentTarget = null;
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                if (currentTarget != null)
                {
                    currentTarget.transform.GetComponent<IPointerClickHandler>().OnPointerClick(null);
                }
            }
        }
    }
}