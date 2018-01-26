using System.Collections.Generic;
using Exploder2D;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    [SerializeField] private List<string> _destructiveTags;
    private bool _exploded;

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (_destructiveTags.Contains(collision2D.gameObject.tag) && !_exploded)
        {
            GetComponent<Exploder2DObject>().Explode();
            _exploded = true;
        }    
    }
    
}
