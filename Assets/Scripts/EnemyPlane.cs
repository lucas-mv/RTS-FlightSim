using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlane : MonoBehaviour
{
    [SerializeField] GameObject[] planes;

    private void Start()
    {
        planes[Random.Range(0, planes.Length)].SetActive(true);
    }
}
