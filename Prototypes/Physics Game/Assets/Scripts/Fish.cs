using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _body;
    [SerializeField] private Rigidbody2D _rigidbody2D;

    public void SetColour(Color color)
    {
        _body.color = color;
        Debug.LogFormat("Set Fish's [{0}] to \"{1}\".", color.GetType().Name, color);
    }

    public void SetPhysicsEnabled(bool state)
    {
        RigidbodyType2D rigidbodyType2D = state ? RigidbodyType2D.Dynamic : RigidbodyType2D.Dynamic;
        _rigidbody2D.bodyType = rigidbodyType2D;
        Debug.LogFormat("Set Fish's [{0}] to \"{1}\".", rigidbodyType2D.GetType().Name, rigidbodyType2D);
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
        Debug.LogFormat("Set Fish's position to {0}.", position);
    }
}
