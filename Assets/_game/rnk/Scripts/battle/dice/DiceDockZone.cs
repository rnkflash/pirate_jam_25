using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _game.rnk.Scripts.battle.dice
{
    [ExecuteInEditMode]
    public class DiceDockZone : MonoBehaviour
    {
        [SerializeField] float spacing = 1.0f; // Spacing between objects
        [SerializeField] Alignment alignment = Alignment.Center; // Alignment of the objects
        
        [NonSerialized] public List<Dice3D> dices;
        [NonSerialized] public UnityAction<Dice3D> onClickDice;

        public enum Alignment
        {
            Left,
            Center,
            Right
        }

        void Awake()
        {
            dices = GetChildren().Select(transform1 => transform1.GetComponent<Dice3D>()).ToList();
        }

        public void Claim(Dice3D dice)
        {
            dice.zone?.Release(dice);
            dice.transform.SetParent(transform);
            dices.Add(dice);
        }

        public void Release(Dice3D dice)
        {
            dices.Remove(dice);
        }

        void Update()
        {
            if (Application.isPlaying)
                ArrangeChildren();
        }

        void OnValidate()
        {
            dices = GetChildren().Select(transform1 => transform1.GetComponent<Dice3D>()).ToList();
            ArrangeChildren(false);
        }
        
        void OnDrawGizmos()
        {
            // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, new Vector3(1,1,1));
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
        
        void ArrangeChildren(bool movable = true)
        {
            // Calculate total width
            float totalWidth = 0f;
            foreach (var child in dices)
            {
                LayoutElement layoutElement = child.GetComponent<LayoutElement>();
                if (layoutElement != null)
                {
                    totalWidth += layoutElement.preferredWidth;
                }
            }
            totalWidth += spacing * (dices.Count - 1);

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
            foreach (var child in dices)
            {
                LayoutElement layoutElement = child.GetComponent<LayoutElement>();
                if (layoutElement != null)
                {
                    float childWidth = layoutElement.preferredWidth;
                    var targetPosition = new Vector3(currentX + childWidth / 2f, transform.position.y, transform.position.z);
                    
                    if (!Application.isPlaying)
                        child.transform.position = targetPosition;
                    if (movable)
                        child.targetPosition = targetPosition;
                    else
                        child.transform.position = targetPosition;
                    
                    currentX += childWidth + spacing;
                }
            }
        }
    }
}