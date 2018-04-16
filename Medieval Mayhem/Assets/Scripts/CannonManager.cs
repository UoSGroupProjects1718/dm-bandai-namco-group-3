using System.Collections;
using Exploder2D;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(AudioSource))]
public class CannonManager : MonoBehaviour
{
    private const string AnimCannonState = "State";
    private const int AnimCannonStateIdle = 0;
    private const int AnimCannonStateSwaying = 1;

    private const int AudioPitchScaleFactor = 4;

    private const string LayerGround = "No Bounce Ground";
    
    [SerializeField] private Rigidbody2D _rbCannon;
    [SerializeField] private Animator _animCannonBarrel;
    [SerializeField] private AudioClip _sfxCannonSwaying;
    [SerializeField] private AudioClip _sfxCannonHitGround;
    [SerializeField] private AudioClip _sfxCannonFire;
    
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _slowTimeSecondsMin;
    [SerializeField] private float _slowTimeSecondsMax;
    [SerializeField] private float _timeBeforeRotatingSeconds;
    [SerializeField] private float _timeBeforeFiringSeconds;

    [SerializeField] private GameObject _objCannonBall;
    [SerializeField] private Transform _tsfCannonBallSpawnPoint;
    
    [SerializeField] private Exploder2DObject _expTreeStump;


    [SerializeField] private float _firePower;

    private AudioSource _audioSource;
    private Rigidbody2D _rigidbody2D;
    private Collider2D _collider2D;

    private bool _hitGround;
    
    public void Fire()
    {
        StartCoroutine(AimCannon());
    }

    private void Awake()
    {
        _animCannonBarrel.enabled = false;

        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.bodyType = RigidbodyType2D.Static;

        _collider2D = GetComponent<Collider2D>();
        
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.layer == LayerMask.NameToLayer(LayerGround))
        {
            _hitGround = true;
        }
    }

    private void Shoot()
    {
        var ball = Instantiate(_objCannonBall, _tsfCannonBallSpawnPoint.position,  Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));
        ball.GetComponent<Rigidbody2D>().AddForce(_tsfCannonBallSpawnPoint.transform.up * _firePower, ForceMode2D.Impulse);
        _audioSource.clip = _sfxCannonFire;
        _audioSource.Play();
        _rbCannon.bodyType = RigidbodyType2D.Dynamic;
    }

    private void Update()
    {
        _animCannonBarrel.speed = _rotationSpeed;
        _audioSource.pitch = _rotationSpeed * AudioPitchScaleFactor;
    }
    
    private IEnumerator AimCannon()
    {
        if(AudioManager.Instance != null)
            AudioManager.Instance.Stop(2.5f);
        
        // Drop the cannon from the sky and wait for it to hit the ground
        _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;        
        while (!_hitGround)
            yield return null;
        
        // Disable the colliders/rigidbody used for the falling entrance
        _rigidbody2D.bodyType = RigidbodyType2D.Static;
        _collider2D.enabled = false;
        
        // Explode the tree stump
        _expTreeStump.Explode();

        // Play the impact sound
        _audioSource.clip = _sfxCannonHitGround;
        _audioSource.Play();
        
        // Delay the aiming of the cannon
        yield return new WaitForSeconds(_timeBeforeRotatingSeconds);
        
        // Begin the aiming animation and sound effect
        _audioSource.clip = _sfxCannonSwaying;
        _audioSource.Play();
        _animCannonBarrel.enabled = true;
        
        // Slow down the rotation speed of the cannon over time until it reaches zero
        var alpha = _rotationSpeed;
        for (var t = 0.0f; t < 1.0f; t += Time.deltaTime / Random.Range(_slowTimeSecondsMin, _slowTimeSecondsMax))
        {
            _rotationSpeed = Mathf.Lerp(alpha, 0.0f, t);
            yield return null;
        }
        
        // Pause before firing
        yield return new WaitForSeconds(_timeBeforeFiringSeconds);
        
        // Disable the aiming animation
        _animCannonBarrel.enabled = false;
        _rotationSpeed = 1.0f;

        // Fire the cannon
        Shoot();
    }
    

}
