using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.ProBuilder.MeshOperations;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private Transform m_BulletSpawner;
    [SerializeField] private Transform m_Bullet;
    [SerializeField] private float m_Speed;
    private Rigidbody m_rigidBody;
    private Animator m_Animator;

    private Camera mainCamera;
    [SerializeField] private Vector3 m_CameraOffset;

    // Inputs
    private bool m_LeftInput;
    private bool m_RightInput;
    private bool m_UpInput;
    private bool m_DownInput;

    private bool m_CanMove;

    private Ray m_MouseRay;
    private RaycastHit m_HitInfo;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        m_CanMove = true;
    }
    
    // Update is called once per frame
    void Update()
    {
        CameraFollow();
        Inputs();
        Move();
        Animate();
        Attack();
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
    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_MouseRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(m_MouseRay, out m_HitInfo))
            {
                if (m_HitInfo.collider.CompareTag("Enemy"))
                {
                    transform.LookAt(m_HitInfo.collider.transform.position);
                    m_Animator.SetTrigger("Stab");
                }
            }
        }
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
}