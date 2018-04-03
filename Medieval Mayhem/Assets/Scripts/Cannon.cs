using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private Transform _ballSpawnPoint;
    [SerializeField] private float _firePower;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] public bool Active;

    [SerializeField] private AudioClip _aimingSound;
    [SerializeField] private AudioClip _fireSound;
    
    private AudioSource _audioSource;

    private void Fire()
    {
        var ball = Instantiate(_ballPrefab, _ballSpawnPoint.position,  Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));
        ball.GetComponent<Rigidbody2D>().AddForce(transform.up * _firePower, ForceMode2D.Impulse);
    }

    private void Update()
    {
        _audioSource.pitch = _rotationSpeed * 4;
        GetComponent<Animator>().speed = _rotationSpeed;
        if (Active)
        {
            StartCoroutine(FireBalls());
                Active = false;
        }
    }

    private IEnumerator FireBalls()
    {
        yield return new WaitForSeconds(2.5f);
        GetComponent<Animator>().enabled = true;
        _audioSource.clip = _aimingSound;
        _audioSource.Play();
        float slowTime = Random.Range(2.0f, 4.0f);
        var swayTime = Random.Range(2.0f, 4.0f);
        yield return new WaitForSeconds(swayTime);
        
        var alpha = _rotationSpeed;
        for (var t = 0.0f; t < 1.0f; t += Time.deltaTime / slowTime)
        {
            _rotationSpeed = Mathf.Lerp(alpha, 0.0f, t);
            yield return null;
        }

        yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
        
        GetComponent<Animator>().enabled = false;
        Fire();

        _audioSource.Stop();
        _audioSource.clip = _fireSound;
        _rotationSpeed = 1.0f;
        _audioSource.Play();
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        GetComponent<Animator>().enabled = false;
    }
    

}
