using System;
using Exploder2D;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public static string Tag = "Bomb";
    
    [SerializeField] private Vector2 _maxVelocity;
    [SerializeField] private float _maxRotationSpeed;
    [SerializeField] private AudioClip _castleExplodeSound;
    [SerializeField] private AudioClip _hitSound;

    private AudioSource _audioSource;
    private Rigidbody2D _rigidbody2D;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        //_audioSource.Play();
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
                
        Debug.LogFormat("Velocity: ({0}, {1})", _rigidbody2D.velocity.x, _rigidbody2D.velocity.y);
        Debug.LogFormat("Angular velocity: ({0})", _rigidbody2D.angularVelocity);
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        _audioSource.PlayOneShot(_hitSound);
//        if (collision2D.gameObject.CompareTag(Castle.Tag))
//        {
//            //_audioSource.Stop();
//            AudioSource.PlayClipAtPoint(_castleExplodeSound, Vector3.zero, 1.5f);
//            Destroy(gameObject);
//        }
    }
}
