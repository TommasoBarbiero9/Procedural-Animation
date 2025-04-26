using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIController : MonoBehaviour
{
    #region Public variables
    #endregion
    #region Private variables
    [SerializeField]
    private Transform target;
    private NavMeshAgent agent;
    #endregion
    #region Lifecycle
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        agent.SetDestination(target.position);
    }
    #endregion
    #region Public methods
    #endregion
    #region Private methods
    #endregion
}
