using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _game.rnk.Scripts.battle.dice
{
    [ExecuteInEditMode]
    public class HorizontalLayoutGroup3DModified : MonoBehaviour
    {
        public float spacing = 1.0f; // Spacing between objects
        public Alignment alignment = Alignment.Center; // Alignment of the objects
        [NonSerialized] public List<Dice3D> dices;

        public enum Alignment
        {
            Left,
            Center,
            Right
        }

        void Awake()
        {
            dices = new List<Dice3D>();
            foreach (var child in GetChildren())
            {
                dices.Add(child.GetComponent<Dice3D>());
            }
        }

        public void Claim(Dice3D dice)
        {
            dices.Add(dice);
        }

        public void Release(Dice3D dice)
        {
            dices.Remove(dice);
        }

        void Update()
        {
            ArrangeChildren(GetChildren());
        }

        void OnValidate()
        {
            ArrangeChildren(GetChildren());
        }

        Transform[] GetChildren()
        {
            Transform[] children = new Transform[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                children[i] = transform.GetChild(i);
            }
            return children;
        }
        
        void ArrangeChildren(Transform[] children)
        {
            // Calculate total width
            float totalWidth = 0f;
            foreach (var child in children)
            {
                LayoutElement layoutElement = child.GetComponent<LayoutElement>();
                if (layoutElement != null)
                {
                    totalWidth += layoutElement.preferredWidth;
                }
            }
            totalWidth += spacing * (children.Length - 1);

            // Calculate starting position based on alignment
            Vector3 startPosition = Vector3.zero;
            switch (alignment)
            {
                case Alignment.Left:
                    startPosition = transform.position - Vector3.right * (totalWidth / 2f);
                    break;
                case Alignment.Center:
                    startPosition = transform.position - Vector3.right * (totalWidth / 2f);
                    break;
                case Alignment.Right:
                    startPosition = transform.position + Vector3.right * (totalWidth / 2f);
                    break;
            }

            // Arrange children
            float currentX = startPosition.x;
            foreach (var child in children)
            {
                LayoutElement layoutElement = child.GetComponent<LayoutElement>();
                if (layoutElement != null)
                {
                    float childWidth = layoutElement.preferredWidth;
                    var targetPosition = new Vector3(currentX + childWidth / 2f, transform.position.y, transform.position.z);
#if UNITY_EDITOR
                    if (!Application.isPlaying)
                        child.transform.position = targetPosition;
#endif
                    var dice3D = child.GetComponent<Dice3D>();
                    if (dice3D != null)
                        dice3D.targetPosition = targetPosition;
                    else
                        child.transform.position = targetPosition;
                    currentX += childWidth + spacing;
                }
            }
        }
    }
}