using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class CrystalPartsBehaviour : MonoBehaviour
{
    [SerializeField] private float m_Speed;
    private GameObject m_Player;
    private List<Transform> m_Parents;
    private List<Transform> m_Children;
    private List<Rigidbody> m_Rigidbodys;
    // Start is called before the first frame update
    void Start()
    {
        m_Player = GameObject.FindWithTag("Player");
        foreach (Transform child in transform)
        {
            m_Parents.Add(child);
            m_Children.Add(child.GetChild(0));
            m_Rigidbodys.Add(child.GetChild(0).GetComponent<Rigidbody>());
            
            child.GetChild(0).GetComponent<Rigidbody>().useGravity = false;
            child.GetChild(0).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;    
        }
        
        // m_Rigidbodys.AddForce(new Vector3(0, 1, 1) * m_Speed, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var parent in m_Parents)
        {
            parent.LookAt(m_Player.transform.position + new Vector3(0, 1.0f, 0));
            parent.Translate(Vector3.forward * Time.deltaTime * m_Speed);
        }
        // transform.LookAt(m_Player.transform.position + new Vector3(0, 1.0f, 0));
        // transform.Translate(Vector3.forward * Time.deltaTime * m_Speed);
        // m_Rigidbody.velocity = Vector3.MoveTowards(transform.position, m_Player.transform.position, Mathf.Infinity);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
