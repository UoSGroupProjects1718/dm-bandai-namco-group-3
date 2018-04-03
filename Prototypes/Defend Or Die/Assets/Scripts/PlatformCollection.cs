using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlatformCollection : MonoBehaviour
{
    [SerializeField] private List<GameObject> _redPlatforms = new List<GameObject>();
    [SerializeField] private List<GameObject> _bluePlatforms = new List<GameObject>();

    public List<Tuple<GameObject, GameObject>> GetRandomPlatforms(int amount)
    {
        var platforms = new List<Tuple<GameObject, GameObject>>();

        var usedPlatformIndexes = new List<int>();
        for (var i = 0; i < amount; i++)
        {
            int randomIndex;
            do
                randomIndex = Random.Range(0, Math.Min(_redPlatforms.Count, _bluePlatforms.Count));
            while
                (usedPlatformIndexes.Contains(randomIndex));
            
            usedPlatformIndexes.Add(randomIndex);
            platforms.Add(new Tuple<GameObject, GameObject>(_redPlatforms[randomIndex], _bluePlatforms[randomIndex]));
        }

        return platforms;
    }
}
