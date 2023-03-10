using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;
using UnityEngine.ProBuilder.MeshOperations;

public class AiBehaviour : MonoBehaviour
{
    public float HP = 100;

    [SerializeField] private Transform player;
    private NavMeshAgent m_NavmeshAgent;
    private Animator m_Animator;

    private float m_PlayerDistance;
    [SerializeField] private float m_TriggerDistance;
    [SerializeField] private float m_attackDistance;

    private bool m_IsStabbing;
    private bool m_OutOfRange;

    // Start is called before the first frame update
    void Start()
    {
        m_NavmeshAgent = GetComponent<NavMeshAgent>();
        m_Animator = GetComponent<Animator>();
        m_IsStabbing = false;
        m_OutOfRange = true;
    }

    // Update is called once per frame
    void Update()
    {
        m_PlayerDistance = Vector3.Distance(player.position, transform.position);
        StateToggler();
    }

    private void StateToggler()
    {
        if (!m_IsStabbing)
        {
            if (m_PlayerDistance < m_attackDistance)
            {
                Attack();
            }
            else if (m_PlayerDistance < m_TriggerDistance)
            {
                Move();
            }
            else
            {
                if (!m_OutOfRange)
                {
                    m_OutOfRange = true;
                    m_NavmeshAgent.SetDestination(transform.position);
                }
            }
            Animate();
        }
        else
        {
            m_NavmeshAgent.SetDestination(transform.position);
            transform.LookAt(player.position);
        }
    }


    private void Move()
    {
        if (m_OutOfRange)
        {
            m_OutOfRange = false;
        }

        m_NavmeshAgent.destination = player.position;
    }

    private void Attack()
    {
        if (m_OutOfRange)
        {
            m_OutOfRange = false;
        }

        m_IsStabbing = true;
        m_NavmeshAgent.velocity = Vector3.zero;
        m_Animator.SetTrigger("Stab");
    }

    private void Animate()
    {
        m_Animator.SetBool("Running", m_NavmeshAgent.velocity != Vector3.zero);
    }

    private void EndAttack()
    {
        m_IsStabbing = false;
    }
}