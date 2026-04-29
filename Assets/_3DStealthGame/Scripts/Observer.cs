using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    public Transform player;
    public GameEnding gameEnding;

    bool m_IsPlayerInRange;

    void Start()
    {
        if (gameEnding == null)
        {
            gameEnding = FindFirstObjectByType<GameEnding>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.root == player)
        {
            m_IsPlayerInRange = true;
            Debug.Log("Player entered range");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.root == player)
        {
            m_IsPlayerInRange = false;
            Debug.Log("Player left range");
        }
    }

    void Update()
    {
        if (m_IsPlayerInRange)
        {
            Vector3 origin = transform.position + Vector3.up * 0.5f;
            Vector3 target = player.position + Vector3.up * 0.5f;
            Vector3 direction = (target - origin).normalized;

            float distance = Vector3.Distance(origin, target);

            Ray ray = new Ray(origin, direction);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, distance))
            {
                Debug.Log("Ray hit: " + hit.collider.name);

                if (hit.transform.root == player)
                {
                    Debug.Log("PLAYER CAUGHT");
                    gameEnding.CaughtPlayer();
                }
            }
        }
    }
}
