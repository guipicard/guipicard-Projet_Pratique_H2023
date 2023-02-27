using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileScript : MonoBehaviour
{
    [SerializeField] private float m_Speed;
    [SerializeField] private float m_Damage;

    private Vector3 InitPos;
    // Start is called before the first frame update
    void Start()
    {
        InitPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * m_Speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, InitPos) > 10.0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

            if (collision.gameObject.CompareTag("Ennemy"))
            {
                collision.gameObject.GetComponent<AiBehaviour>().HP -= m_Damage;
                Destroy(gameObject);
                Debug.Log(collision.gameObject.GetComponent<AiBehaviour>().HP);
            }
        
    }
}
