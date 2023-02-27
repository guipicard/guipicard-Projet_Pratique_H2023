using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.ProBuilder.MeshOperations;

public class PlayerBehaviour : MonoBehaviour
{
    public float HP;
    
    [SerializeField] private float m_Speed;
    private Rigidbody m_rigidBody;
    private Animator m_Animator;

    private Camera mainCamera;
    [SerializeField] private Vector3 m_CameraOffset;

    [SerializeField] private GameObject m_Projectile;
    [SerializeField] private GameObject m_PojectileSpawner;
    
    // Inputs
    private bool m_LeftInput;
    private bool m_RightInput;
    private bool m_UpInput;
    private bool m_DownInput;

    private bool m_CanMove;
    private bool m_IsAttacking;
    
    // Ray
    private RaycastHit hitInfo;
    private Ray screenRay;

    void Start()
    {
        HP = 100;
        m_rigidBody = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        m_CanMove = true;
        m_IsAttacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
        Move();
        CameraFollow();
        Animate();
    }

    private void CameraFollow()
    {
        mainCamera.transform.position = transform.position - m_CameraOffset;
        mainCamera.transform.LookAt(transform.position);
    }

    private void Inputs()
    {
        m_LeftInput = Input.GetKey(KeyCode.A);
        m_RightInput = Input.GetKey(KeyCode.D);
        m_UpInput = Input.GetKey(KeyCode.W);
        m_DownInput = Input.GetKey(KeyCode.S);
    }

    private void Move()
    {
        if (m_CanMove)
        {
            Vector3 currentVelocity = m_rigidBody.velocity;
            if (m_LeftInput)
            {
                currentVelocity.x = -m_Speed;
            }

            if (m_RightInput)
            {
                currentVelocity.x = m_Speed;
            }

            if (m_UpInput)
            {
                currentVelocity.z = m_Speed;
            }

            if (m_DownInput)
            {
                currentVelocity.z = -m_Speed;
            }

            m_rigidBody.velocity = currentVelocity;
            transform.LookAt(transform.position + new Vector3(currentVelocity.x, 0, currentVelocity.z));
        }
    }

    private void Animate()
    {
        m_Animator.SetBool("Running", m_rigidBody.velocity != Vector3.zero);
        Attack();
    }

    private void CanMoveToggle()
    {
        m_CanMove = !m_CanMove;
    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            screenRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(screenRay, out hitInfo))
            {
                if (hitInfo.collider.CompareTag("Ennemy"))
                {
                    transform.LookAt(hitInfo.collider.transform.position);
                    m_rigidBody.velocity = Vector3.zero;
                    m_Animator.SetTrigger("Stab");
                }
            }
        }
    }

    private void LauchProjectile()
    {
        GameObject newProj = Instantiate(m_Projectile);
        newProj.transform.rotation = transform.localRotation;
        newProj.transform.position = m_PojectileSpawner.transform.position;
    }
}