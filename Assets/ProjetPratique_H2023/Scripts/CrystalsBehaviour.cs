using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CrystalsBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject m_CrystalObject;
    [SerializeField] private string m_CrystalTag;
    [SerializeField] private float m_MuliplieCooldown;
    [SerializeField] private float m_CrystalSpacing;
    [SerializeField] private float m_Bounds;
    [SerializeField] private float m_crystalsHeight;
    [SerializeField] private GameObject m_AiPrefab;

    public List<Vector2> m_CrystalsPosition;
    public List<Vector2> m_PotentialPosition;

    private float m_Elapsed;
    private Ray m_Ray;
    private RaycastHit m_HitInfo;
    public int m_AiAlive;
    private int m_ActiveCrystals;

    void Start()
    {
        m_AiAlive = 0;
        m_Elapsed = 0;

        m_PotentialPosition = new List<Vector2>();
        m_CrystalsPosition = new List<Vector2>();
        // Add All Present Crystals Positions in List "CrystalsPosition"
        foreach (var crystal in GameObject.FindGameObjectsWithTag(m_CrystalTag))
        {
            Vector3 crystalPos = crystal.transform.position;
            m_CrystalsPosition.Add(new Vector2(crystalPos.x, crystalPos.z));
        }

        SpawnAi();
    }

    void Update()
    {
        if (m_Elapsed == 0.1f)
        {
            Multiply();
            SpawnAi();
        }

        m_Elapsed += Time.deltaTime;
        if (m_Elapsed > m_MuliplieCooldown)
        {
            GetNewPositions();
            m_Elapsed = 0.1f;
        }
    }

    private void GetNewPositions()
    {
        foreach (var crystal in GameObject.FindGameObjectsWithTag(m_CrystalTag))
        {
            Vector3 crystalPos = crystal.transform.position;
            m_CrystalsPosition.Add(new Vector2(crystalPos.x, crystalPos.z));
        }
        // Add All Potential Places a new Crystal could be
        for (int i = m_CrystalsPosition.Count - 1; i >= 0; i--)
        {
            Vector2 currentCrystal = m_CrystalsPosition[i];

            Vector2 pos1 = currentCrystal + new Vector2(m_CrystalSpacing, m_CrystalSpacing);
            Vector2 pos2 = currentCrystal + new Vector2(-m_CrystalSpacing, m_CrystalSpacing);
            Vector2 pos3 = currentCrystal + new Vector2(m_CrystalSpacing, -m_CrystalSpacing);
            Vector2 pos4 = currentCrystal + new Vector2(-m_CrystalSpacing, -m_CrystalSpacing);

            Vector2[] surroundingPlacings = { pos1, pos2, pos3, pos4 };
            for (int j = surroundingPlacings.Length - 1; j >= 0; j--)
            {
                Vector2 pos = surroundingPlacings[j];
                if (!m_CrystalsPosition.Contains(pos) && !m_PotentialPosition.Contains(pos))
                {
                    m_Ray = new Ray(new Vector3(pos.x, m_crystalsHeight + 2.0f, pos.y), Vector3.down);
                    if (Physics.Raycast(m_Ray, out m_HitInfo, Mathf.Infinity))
                    {
                        if (m_HitInfo.collider.CompareTag("Ground") || m_HitInfo.collider.gameObject.layer == 6)
                        {
                            m_PotentialPosition.Add(pos);
                        }
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
            int chances = Random.Range(0, 5);
            if (chances == 1)
            {
                m_Ray = new Ray(new Vector3(pos.x, m_crystalsHeight + 2.0f, pos.y), Vector3.down);
                if (Physics.Raycast(m_Ray, out m_HitInfo, Mathf.Infinity))
                {
                    if (m_HitInfo.collider.gameObject.layer == 6)
                    {
                        // m_HitInfo.collider.gameObject.GetComponent<CrystalsBehaviour>().m_CrystalsPosition.Remove(new Vector2(m_HitInfo.collider.transform.position.x, m_HitInfo.collider.transform.position.z));
                        Destroy(m_HitInfo.collider.gameObject);
                    }
                    GameObject newCrystal = Instantiate(m_CrystalObject);
                    newCrystal.transform.position = new Vector3(pos.x, m_crystalsHeight, pos.y);
                    newCrystal.transform.rotation = Quaternion.identity;
                    newCrystal.transform.parent = transform;
                }
            }
        }

        // Empty Lists
        
        m_PotentialPosition = new List<Vector2>();
    }

    private void SpawnAi()
    {
        m_ActiveCrystals = m_CrystalsPosition.Count();
        if (m_AiAlive < m_ActiveCrystals / 20)
        {
            m_AiAlive++;
            Vector2 lastCrystal = m_CrystalsPosition[Random.Range(0, m_ActiveCrystals)];
            GameObject newAi = Instantiate(m_AiPrefab);
            newAi.transform.position = new Vector3(lastCrystal.x, m_crystalsHeight, lastCrystal.y);
            newAi.transform.rotation = Quaternion.identity;
            newAi.transform.parent = transform;
        }
        m_CrystalsPosition = new List<Vector2>();
    }
}