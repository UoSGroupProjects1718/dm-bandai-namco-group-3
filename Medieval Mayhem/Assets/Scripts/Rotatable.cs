using Lean.Touch;
using UnityEngine;

public class Rotatable : MonoBehaviour
{
    [Tooltip("Ignore fingers with StartedOverGui?")]
    public bool IgnoreGuiFingers;

    [Tooltip("Allows you to force rotation with a specific amount of fingers (0 = any)")]
    public int RequiredFingerCount;

    [Tooltip("Does rotation require an object to be selected?")]
    public LeanSelectable RequiredSelectable;

    [Tooltip("The rotation axis used for non-relative rotations")]
    public Vector3 RotateAxis = Vector3.forward;

    [Tooltip("Should the rotation be performanced relative to the finger center?")]
    public bool Relative;

    public bool IsRotatable;

    protected virtual void Update()
    {
        if (!IsRotatable || RequiredSelectable != null && RequiredSelectable.IsSelected == false)
            return;

        var fingers = LeanTouch.GetFingers(IgnoreGuiFingers, RequiredFingerCount);

        var center  = LeanGesture.GetScreenCenter(fingers);
        var degrees = LeanGesture.GetTwistDegrees(fingers);

        Rotate(center, degrees);
    }

    private void Rotate(Vector3 center, float degrees)
    {
        if (Relative)
        {
            var worldReferencePoint = Camera.main.ScreenToWorldPoint(center);
            transform.RotateAround(worldReferencePoint, Camera.main.transform.forward, degrees);
        }
        else
        {
            transform.rotation *= Quaternion.AngleAxis(degrees, RotateAxis);
        }
    }
}