using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunAwayAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent = null;
    [SerializeField] private Transform chaser = null;
    void Start()
    {
        if (agent == null)
            if (!TryGetComponent(out agent));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
