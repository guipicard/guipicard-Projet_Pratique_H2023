using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;
using UnityEngine.ProBuilder.MeshOperations;

public class AiBehaviour : MonoBehaviour
{
    [SerializeField] private Transform player;
    private NavMeshAgent m_NavmeshAgent;
    private Animator m_Animator;

    private bool m_CanMove;
    private float m_PlayerDistance;
    [SerializeField] private float m_TriggerDistance;
    [SerializeField] private float m_attackDistance;

    private bool m_Canstab;
    private bool m_IsStabing;

    // Start is called before the first frame update
    void Start()
    {
        m_NavmeshAgent = GetComponent<NavMeshAgent>();
        m_Animator = GetComponent<Animator>();
        m_CanMove = false;
        m_Canstab = false;
        m_IsStabing = false;
        m_NavmeshAgent.stoppingDistance = m_attackDistance;
    }

    // Update is called once per frame
    void Update()
    {
        m_PlayerDistance = Vector3.Distance(player.position, transform.position);

        if (!m_IsStabing && m_Canstab)
        {
            m_IsStabing = true;
            m_Animator.SetTrigger("Stab");
            Debug.Log("stab");
        }
        
        if (m_PlayerDistance <= m_attackDistance && m_CanMove) // range
        {
            Debug.Log("Range");
            m_NavmeshAgent.destination = transform.position;
            m_NavmeshAgent.velocity = Vector3.zero;
            m_Canstab = true;
            m_CanMove = false;
            m_Animator.SetBool("Running", false);
        }
        else if (m_PlayerDistance < m_TriggerDistance && m_CanMove) // close
        {
            Debug.Log("Close");
            m_NavmeshAgent.destination = player.position;
            m_Animator.SetBool("Running", true);
            transform.LookAt(player.position);
        }
        else if (m_PlayerDistance > m_TriggerDistance) // far
        {
            Debug.Log("Far");
            m_NavmeshAgent.destination = transform.position;
            m_Animator.SetBool("Running", false);
        }
    }

    private void ResetState()
    {
        m_CanMove = true;
        m_IsStabing = false;
        m_Canstab = false;
        Debug.Log("toggle");
    }
}