using System;
using Exploder2D;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public static string Tag = "Bomb";
    
    [SerializeField] private Vector2 _maxVelocity;
    [SerializeField] private float _maxRotationSpeed;
    [SerializeField] private AudioClip _hitSound;
    [SerializeField] private AudioClip _sfxTenseMusic;

    private AudioSource _audioSource;
    
    private Rigidbody2D _rigidbody2D;
    

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        if (AudioManager.Instance != null)
            AudioManager.Instance.Play(_sfxTenseMusic, 0.0f);
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

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.CompareTag(Castle.Tag))
        {
            for(int i = 0; i < 100; i++)
                Debug.Log("IT IS DOING SOMETHING: " + collider2D.gameObject.tag);
            if (AudioManager.Instance != null)
                AudioManager.Instance.Stop(0.0f);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.layer != LayerMask.NameToLayer("Platforms"))
        {
            _audioSource.PlayOneShot(_hitSound);
        }   
    }
}
