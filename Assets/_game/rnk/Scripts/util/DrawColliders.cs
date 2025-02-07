/*/*
 * Written by Jonas Tingmose aka. Ultroman the Tacoman
 * 23 Feb. 2018
 * Revised 02 Jul. 2024 - Fixed capsule drawing when using other directions than Y-Axis.
 *
 * Attach this component to a GameObject, to draw all capsule, box and sphere colliders
 * on that specific gameobject, with an option to include all colliders on its children.
 * There is NO SUPPORT for wheel, spatial mapping, terrain or mesh colldiers.
 * If you code support for these, please let me know, so I can update the script code,
 * obviously credit you, and we can keep this useful tool available for all!
 *
 * NOTE!!! Requires the FREE Unity asset "Debug Drawing Extension" found here:
 * https://assetstore.unity.com/packages/tools/debug-drawing-extension-11396
 * Aside from the readme and example.cs, it's a single, static .cs file, which gives you
 * the amazing power, to draw many shapes, like cylinders, circles, arrows, local cubes,
 * cones etc., and of course capsules, which is what this DrawColliders script uses it for.
 #1#

using Assets.Utils.DebugDrawingExtension;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DrawColliders : MonoBehaviour {
    public bool AlwaysShowCollider = true;
    public bool IncludeCollidersInChildren = true;
    public bool IncludeInactiveChildColliders = true;
    public Color GizmoColor = Color.red;
    private List<Collider> _colliders = new List<Collider>(10);
    private BoxCollider _boxTemp;
    private SphereCollider _sphereTemp;
    private CapsuleCollider _capsuleTemp;
    private Color _prevColor;
    
    void OnDrawGizmos()
    {
        if (AlwaysShowCollider)
        {
            _colliders.Clear();
            if(IncludeCollidersInChildren)
                GetComponentsInChildren<Collider>(IncludeInactiveChildColliders, _colliders);
            else
                GetComponents<Collider>(_colliders);

            if (_colliders == null || _colliders.Count == 0)
                return;

            _prevColor = Gizmos.color;
            Gizmos.color = GizmoColor;

            Collider _temp;
            for (int i = 0; i < _colliders.Count; i++)
            {
                _temp = _colliders[i];
                if (!_temp.enabled)
                    continue;
                if ((_boxTemp = _temp as BoxCollider) != null)
                {
                    Matrix4x4 oldGizmosMatrix = Gizmos.matrix;

                    Gizmos.matrix = Matrix4x4.TRS(_boxTemp.transform.TransformPoint(_boxTemp.center), _boxTemp.transform.rotation, _boxTemp.transform.lossyScale);
                    Gizmos.DrawWireCube(Vector3.zero, _boxTemp.size);

                    Gizmos.matrix = oldGizmosMatrix;
                }
                else if((_sphereTemp = _temp as SphereCollider) != null)
                {
                    Matrix4x4 oldGizmosMatrix = Gizmos.matrix;

                    Gizmos.matrix = Matrix4x4.TRS(_sphereTemp.transform.TransformPoint(_sphereTemp.center), _sphereTemp.transform.rotation, Vector3.one * 
                        Mathf.Max(Mathf.Abs(_sphereTemp.transform.lossyScale.x), Mathf.Max(Mathf.Abs(_sphereTemp.transform.lossyScale.y), Mathf.Abs(_sphereTemp.transform.lossyScale.z))));
                    Gizmos.DrawWireSphere(Vector3.zero, _sphereTemp.radius);

                    Gizmos.matrix = oldGizmosMatrix;
                }
                else if((_capsuleTemp = _temp as CapsuleCollider) != null)
                {
                    Vector3 ls = _capsuleTemp.transform.lossyScale;
                    var directionVector = _capsuleTemp.transform.up;
                    var centerHalfScale = new Vector3(_capsuleTemp.center.x * ls.x, _capsuleTemp.center.y * ls.y, _capsuleTemp.center.z * ls.z);
                    // scale height by: multiplying by the largest of X, Y or Z lossyScale, depending on which .direction is used.
                    float halfHeight = _capsuleTemp.height * 0.5f;
                    // scale radius by: capsule radius, multiplied by the largest of X, Y and/or Z lossyScales, depending on which .direction is used.
                    var scaledRadius = 0f;

                    // direction == 0 is the X-axis
                    if (_capsuleTemp.direction == 0)
                    {
                        halfHeight *= Mathf.Abs(ls.x);
                        directionVector = (Quaternion.AngleAxis(90, Vector3.forward) * Vector3.up);
                        scaledRadius = _capsuleTemp.radius * Mathf.Max(Mathf.Abs(ls.y), Mathf.Abs(ls.z));
                    }
                    // direction == 1 is the Y-axis
                    else if (_capsuleTemp.direction == 1)
                    {
                        halfHeight *= Mathf.Abs(ls.y);
                        directionVector = (Quaternion.AngleAxis(90, Vector3.up) * Vector3.up);
                        scaledRadius = _capsuleTemp.radius * Mathf.Max(Mathf.Abs(ls.x), Mathf.Abs(ls.z));
                    }
                    // direction == 2 is the Z-axis
                    else if (_capsuleTemp.direction == 2)
                    {
                        halfHeight *= Mathf.Abs(ls.z);
                        directionVector = (Quaternion.AngleAxis(90, Vector3.right) * Vector3.up);
                        scaledRadius = _capsuleTemp.radius * Mathf.Max(Mathf.Abs(ls.x), Mathf.Abs(ls.y));
                    }

                    Matrix4x4 oldGizmosMatrix = Gizmos.matrix;
                    Gizmos.matrix = Matrix4x4.TRS(_capsuleTemp.transform.position, _capsuleTemp.transform.rotation, Vector3.one);
                    Vector3 capsuleBoundsStart = centerHalfScale + directionVector * halfHeight;
                    Vector3 capsuleBoundsEnd = centerHalfScale - directionVector * halfHeight;

                    DebugExtension.DrawCapsule(capsuleBoundsStart, capsuleBoundsEnd, GizmoColor,
                        scaledRadius
                        );

                    Gizmos.matrix = oldGizmosMatrix;
                }
            }

            Gizmos.color = _prevColor;
        }
    }
}*/