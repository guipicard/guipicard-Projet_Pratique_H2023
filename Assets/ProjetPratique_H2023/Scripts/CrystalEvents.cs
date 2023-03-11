using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalEvents : MonoBehaviour
{
    [SerializeField] private GameObject m_CrystalParts;

    private CrystalsBehaviour m_CrystalsBehaviour;
    
    void Start()
    {
        m_CrystalsBehaviour = transform.parent.GetComponent<CrystalsBehaviour>();
    }

    public void GetMined()
    {
        m_CrystalsBehaviour.m_CrystalsPosition.Remove(new Vector2(transform.position.x, transform.position.z));
        Instantiate(m_CrystalParts, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}