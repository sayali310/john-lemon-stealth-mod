using UnityEngine;
using UnityEngine.InputSystem;

public class OrbShooter : MonoBehaviour
{
    public InputAction shootAction;

    public GameObject projectilePrefab;
    public Transform firePoint;

    public float cooldown = 0.5f;
    public float targetRange = 12f;

    float m_Timer;
    Transform currentTarget;

    void Start()
    {
        shootAction.Enable();
    }

    void Update()
    {
        m_Timer += Time.deltaTime;

        currentTarget = FindNearestVisibleEnemy();

        if (currentTarget != null)
        {
            Vector3 direction = currentTarget.position - firePoint.position;
            direction.y = 0f;

            if (direction != Vector3.zero)
            {
                firePoint.rotation = Quaternion.LookRotation(direction);
            }
        }

        if (shootAction.WasPressedThisFrame() && m_Timer >= cooldown)
        {
            if (currentTarget != null)
            {
                Shoot();
                m_Timer = 0f;
            }
        }
    }

    Transform FindNearestVisibleEnemy()
    {
        string[] enemyTags = { "Ghost", "Gargoyle" };

        Transform nearestEnemy = null;
        float nearestDistance = targetRange;

        foreach (string tag in enemyTags)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(tag);

            foreach (GameObject enemy in enemies)
            {
                Vector3 origin = firePoint.position;
                Vector3 target = enemy.transform.position + Vector3.up * 0.5f;
                Vector3 direction = target - origin;
                float distance = direction.magnitude;

                if (distance > targetRange)
                    continue;

                if (Physics.Raycast(origin, direction.normalized, out RaycastHit hit, distance))
                {
                    if (hit.transform == enemy.transform || hit.transform.IsChildOf(enemy.transform))
                    {
                        if (distance < nearestDistance)
                        {
                            nearestDistance = distance;
                            nearestEnemy = enemy.transform;
                        }
                    }
                }
            }
        }

        return nearestEnemy;
    }

    void Shoot()
    {
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    }
}