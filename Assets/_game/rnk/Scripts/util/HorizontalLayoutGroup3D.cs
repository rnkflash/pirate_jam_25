using UnityEngine;

namespace _game.rnk.Scripts.util
{
    [ExecuteInEditMode]
    public class HorizontalLayoutGroup3D : MonoBehaviour
    {
        public float spacing = 1.0f; // Spacing between objects
        public Alignment alignment = Alignment.Center; // Alignment of the objects

        public enum Alignment
        {
            Left,
            Center,
            Right
        }

        void Update()
        {
            ArrangeChildren();
        }

        void OnValidate()
        {
            ArrangeChildren();
        }

        void ArrangeChildren()
        {
            // Get all child objects
            Transform[] children = new Transform[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                children[i] = transform.GetChild(i);
            }

            // Calculate total width
            float totalWidth = 0f;
            foreach (var child in children)
            {
                Renderer renderer = child.GetComponent<Renderer>();
                if (renderer != null)
                {
                    totalWidth += renderer.bounds.size.x;
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
                Renderer renderer = child.GetComponent<Renderer>();
                if (renderer != null)
                {
                    Vector3 childSize = renderer.bounds.size;
                    child.position = new Vector3(currentX + childSize.x / 2f, transform.position.y, transform.position.z);
                    currentX += childSize.x + spacing;
                }
            }
        }
    }
}