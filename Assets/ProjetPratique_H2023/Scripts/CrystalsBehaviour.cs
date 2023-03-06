using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalsBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject m_CrystalObject;
    [SerializeField] private string m_CrystalTag;
    [SerializeField]private float m_MuliplieCooldown;
    [SerializeField] private float m_CrystalSpacing;
    [SerializeField] private float m_Bounds;
    [SerializeField] private float m_crystalsHeight;
    private List<Vector2> m_CrystalsPosition;
    private List<Vector2> m_PotentialPosition;
    private float m_Elapsed;

    void Start()
    {
        m_Elapsed = 0;

        m_PotentialPosition = new List<Vector2>();
        m_CrystalsPosition = new List<Vector2>();
        // Add All Present Crystals Positions in List "CrystalsPosition"
        foreach (var crystal in GameObject.FindGameObjectsWithTag(m_CrystalTag))
        {
            Vector3 crystalPos = crystal.transform.position;
            m_CrystalsPosition.Add(new Vector2(crystalPos.x, crystalPos.z));
        }

        GetNewPositions();
    }

    void Update()
    {
        if (m_Elapsed == 0.1f)
        {
            GetNewPositions();
        }
        m_Elapsed += Time.deltaTime;
        if (m_Elapsed > m_MuliplieCooldown)
        {
            Multiply();
            m_Elapsed = 0.1f;
        }
    }

    private void GetNewPositions()
    {
        // Add All Potential Places a new Crystal could be
        for (int i = m_CrystalsPosition.Count - 1; i >= 0; i--)
        {
            Vector2 currentCrystal = m_CrystalsPosition[i];
            
            Vector2 pos1 = currentCrystal + new Vector2(m_CrystalSpacing, m_CrystalSpacing);
            Vector2 pos2 = currentCrystal + new Vector2(-m_CrystalSpacing, m_CrystalSpacing);
            Vector2 pos3 = currentCrystal + new Vector2(m_CrystalSpacing, -m_CrystalSpacing);
            Vector2 pos4 = currentCrystal + new Vector2(-m_CrystalSpacing, -m_CrystalSpacing);
            
            Vector2[] surroundingPlacings = {pos1, pos2, pos3, pos4};
            Debug.Log(surroundingPlacings);
            for (int j = surroundingPlacings.Length - 1; j >= 0; j--)
            {
                Vector2 pos = surroundingPlacings[j];
                if (!m_CrystalsPosition.Contains(pos) && !m_PotentialPosition.Contains(pos))
                {

                    // if (pos.x > -m_Bounds && pos.x < m_Bounds && pos.y > -m_Bounds && pos.y < m_Bounds)
                    if (pos.magnitude < 25.0f)
                    {

                        m_PotentialPosition.Add(pos);
                    }
                }
            }
        }
    }

    private void Multiply()
    {
        // Create New Crystals
        foreach (var pos in m_PotentialPosition)
        {
            GameObject newCrystal = Instantiate(m_CrystalObject);
            newCrystal.transform.position = new Vector3(pos.x, m_crystalsHeight, pos.y);
            newCrystal.transform.rotation = Quaternion.identity;
            newCrystal.transform.parent = transform;
            m_CrystalsPosition.Add(pos);
        }
        
        // Empty Lists
        m_PotentialPosition = new List<Vector2>();
    }
}
