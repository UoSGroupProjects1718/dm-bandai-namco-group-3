using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes.Main.Scripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private List<Draggable> _redBlocks;
        [SerializeField] private List<Draggable> _greenBlocks;

        [SerializeField] private Rigidbody2D _ball;
    
        private bool _gameActive;
        private bool _redTurnActive;
        private bool _ballDropped;
    
        private void Start()
        {
            foreach(var draggable in _greenBlocks)
            {
                draggable.gameObject.SetActive(false);
            }
            _redTurnActive = true;
            _gameActive = true;
        }

        private IEnumerator RestartLevel()
        {
            yield return new WaitForSeconds(2.5f);
            SceneManager.LoadScene(0);
        }


        private void FixedUpdate()
        {
            _ball.velocity = Vector2.ClampMagnitude(_ball.velocity, 50.0f);
            Debug.LogFormat("Velocity: \"{0}\".", _ball.velocity);
        }

        private void Update()
        {
                       
            if (!_gameActive)
            {
                return;
            }
        
            if (_redTurnActive && _redBlocks.All(draggable => !draggable.IsDraggable()))
            {
                _redTurnActive = false;
                foreach(var draggable in _redBlocks)
                {
                    draggable.gameObject.SetActive(false);
                }
                foreach(var draggable in _greenBlocks)
                {
                    draggable.gameObject.SetActive(true);
                }
            } 
        
            if (!_redTurnActive && _greenBlocks.All(draggable => !draggable.IsDraggable()) && !_ballDropped)
            {
                foreach(var draggable in _redBlocks)
                {
                    draggable.gameObject.SetActive(true);
                }
                _ball.bodyType = RigidbodyType2D.Dynamic;
                Vector2 randomVelocity = new Vector2(Random.Range(-20.0f, 29.81f), Random.Range(-20.0f, 49.81f));
                Debug.LogFormat("Random Velocity: \"{0}\".", randomVelocity);
                _ball.velocity = randomVelocity;
                _ballDropped = true;
            }

            if (_ballDropped && _ball.bodyType == RigidbodyType2D.Static)
            {
                _gameActive = false;
                StartCoroutine(RestartLevel());
            }
        }
    }
}
