using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;
using UnityEngine.ProBuilder.MeshOperations;

public class PlayerBehaviour : MonoBehaviour
{
    public float HP;
    [SerializeField] private Transform m_BulletSpawner;
    [SerializeField] private Transform m_Bullet;

    [SerializeField] private string m_DamageTag;
    
    private NavMeshAgent m_NavMeshAgent;
    private Animator m_Animator;

    private Camera mainCamera;
    [SerializeField] private Vector3 m_CameraOffset;

    // // Inputs
    // private bool m_LeftInput;
    // private bool m_RightInput;
    // private bool m_UpInput;
    // private bool m_DownInput;

    private bool m_CanMove;

    private Ray m_MouseRay;
    private RaycastHit m_HitInfo;
    
    void Start()
    {
        HP = 100.0f;
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        m_Animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        m_CanMove = true;
    }
    
    void Update()
    {
        CameraFollow();
        Inputs();
        Move();
        Animate();
    }

    private void CameraFollow()
    {
        mainCamera.transform.position = transform.position - m_CameraOffset;
        mainCamera.transform.LookAt(transform.position);
    }

    private void Inputs()
    {
        // m_LeftInput = Input.GetKey(KeyCode.A);
        // m_RightInput = Input.GetKey(KeyCode.D);
        // m_UpInput = Input.GetKey(KeyCode.W);
        // m_DownInput = Input.GetKey(KeyCode.S);
    }

    private void Move()
    {
        // if (m_CanMove)
        // {
        //     Vector3 currentVelocity = m_rigidBody.velocity;
        //     if (m_LeftInput)
        //     {
        //         currentVelocity.x = -m_Speed;
        //     }
        //
        //     if (m_RightInput)
        //     {
        //         currentVelocity.x = m_Speed;
        //     }
        //
        //     if (m_UpInput)
        //     {
        //         currentVelocity.z = m_Speed;
        //     }
        //
        //     if (m_DownInput)
        //     {
        //         currentVelocity.z = -m_Speed;
        //     }
        //
        //     m_rigidBody.velocity = currentVelocity;
        //     transform.LookAt(transform.position + new Vector3(currentVelocity.x, 0, currentVelocity.z));
        // }

        if (Input.GetMouseButtonDown(0))
        {
            m_MouseRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(m_MouseRay, out m_HitInfo))
            {
                if (m_HitInfo.collider.CompareTag("Enemy"))
                {
                    Attack();
                }
                else if (m_HitInfo.collider.CompareTag("Ground"))
                {
                    m_NavMeshAgent.stoppingDistance = 0;
                    transform.LookAt(m_HitInfo.point);
                    m_NavMeshAgent.destination = m_HitInfo.point;
                }
                else if (m_HitInfo.collider.gameObject.layer == 6)
                {
                    Mine();
                }
            }
        }
    }

    private void Animate()
    {
        m_Animator.SetBool("Running", m_NavMeshAgent.velocity != Vector3.zero);
    }

    private void Attack()
    {
        m_NavMeshAgent.destination = transform.position;
        transform.LookAt(m_HitInfo.collider.transform.position);
        m_Animator.SetTrigger("Stab");
    }

    private void LaunchBasicAttack()
    {
        Transform newBullet = Instantiate(m_Bullet);
        newBullet.position = m_BulletSpawner.position;
        newBullet.rotation = transform.rotation;
    }

    private void CanMoveToggle()
    {
        m_CanMove = !m_CanMove;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(m_DamageTag))
        {
            HP -= 10;
            Destroy(other.gameObject);

            Debug.Log(HP);
        }
    }

    private void Mine()
    {
        m_NavMeshAgent.stoppingDistance = 2.0f;
        transform.LookAt(m_HitInfo.collider.transform.position);
        m_NavMeshAgent.destination = m_HitInfo.point;
    }
}