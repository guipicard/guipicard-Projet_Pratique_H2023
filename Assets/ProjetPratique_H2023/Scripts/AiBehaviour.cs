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

    // Start is called before the first frame update
    void Start()
    {
        m_NavmeshAgent = GetComponent<NavMeshAgent>();
        m_Animator = GetComponent<Animator>();
        m_CanMove = false;
        m_Canstab = false;
    }

    // Update is called once per frame
    void Update()
    {
        m_PlayerDistance = Vector3.Distance(player.position, transform.position);

        if (m_CanMove)
        {
            Move();
        }
        
        if (m_PlayerDistance < m_attackDistance && m_CanMove)
        {
            m_Canstab = true;
            m_CanMove = false;
        }
        else if (m_PlayerDistance < m_TriggerDistance)
        {
            m_CanMove = true;
        }
        else
        {
            m_CanMove = false;
            m_NavmeshAgent.SetDestination(transform.position);
        }

        Animate();
    }


    private void Move()
    {
        m_NavmeshAgent.destination = player.position;
        
        transform.LookAt(player.position);
    }

    private void Animate()
    {
        if (m_Canstab)
        {
            m_NavmeshAgent.velocity = Vector3.zero;
            m_Animator.SetTrigger("Stab");
            m_Canstab = false;
        }

        m_Animator.SetBool("Running", m_NavmeshAgent.velocity != Vector3.zero);
    }

    private void CanMoveToggle()
    {
        m_CanMove = false;
    }
}