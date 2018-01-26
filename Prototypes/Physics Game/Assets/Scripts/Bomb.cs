using System;
using Exploder2D;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public static string Tag = "Bomb";
    
    [SerializeField] private Vector2 _maxVelocity;
    [SerializeField] private float _maxRotationSpeed;
    
    private Rigidbody2D _rigidbody2D;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    
    private void FixedUpdate()
    {
        _rigidbody2D.velocity = new Vector2(Mathf.Clamp(_rigidbody2D.velocity.x, -_maxVelocity.x, _maxVelocity.x), 
            Mathf.Clamp(_rigidbody2D.velocity.y, -_maxVelocity.y, _maxVelocity.y));
        if (_rigidbody2D.angularVelocity < -_maxRotationSpeed)
        {
            _rigidbody2D.angularVelocity = -_maxRotationSpeed;
        }
        if (_rigidbody2D.angularVelocity > _maxRotationSpeed)
        {
            _rigidbody2D.angularVelocity = _maxRotationSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.CompareTag(Castle.Tag))
        {
            Destroy(gameObject);
        }
    }
}
