using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterParkourController : MonoBehaviour
{
    [SerializeField] private EnvironmentChecker environmentChecker;

    private void Update()
    {
        environmentChecker.CheckObstacle();
    }
}
