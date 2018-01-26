using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private Transform _ballSpawnPoint;
    [SerializeField] private float _firePower;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private bool _active;

    private void Fire()
    {
        var ball = Instantiate(_ballPrefab, _ballSpawnPoint.position,  Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));
        ball.GetComponent<Rigidbody2D>().AddForce(transform.up * _firePower, ForceMode2D.Impulse);
    }

    private void Update()
    {
        GetComponent<Animator>().speed = _rotationSpeed;
    }

    private IEnumerator FireBalls()
    {
        float slowTime = Random.Range(0.5f, 1.5f);
        var swayTime = Random.Range(2.0f, 6.0f);
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
        
//        yield return new WaitForSeconds(1.0f);
//
//        GetComponent<Animator>().enabled = true;
//        alpha = _rotationSpeed;
//        for (var t = 0.0f; t < 1.0f; t += Time.deltaTime / slowTime)
//        {
//            _rotationSpeed = Mathf.Lerp(alpha, 1.0f, t);
//            yield return null;
//        }
//        
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    private void Start()
    {
        GetComponent<Animator>().enabled = false;
        if (_active)
        {
            StartCoroutine(FireBalls());
            GetComponent<Animator>().enabled = true;
        }
    }
    

}
