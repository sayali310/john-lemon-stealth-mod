using UnityEngine;

public class OrbProjectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 3f;
    public GameObject hitEffectPrefab;
    public AudioClip hitSound;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject enemy = null;

        if (other.CompareTag("Ghost") || other.CompareTag("Gargoyle"))
        {
            enemy = other.gameObject;
        }
        else if (other.transform.root.CompareTag("Ghost") || other.transform.root.CompareTag("Gargoyle"))
        {
            enemy = other.transform.root.gameObject;
        }

        if (enemy != null)
        {
            Debug.Log("Hit enemy: " + enemy.name);

            if (ScoreManager.instance != null)
                ScoreManager.instance.AddScore(1);

            if (hitEffectPrefab != null)
                Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);

            if (hitSound != null)
                AudioSource.PlayClipAtPoint(hitSound, transform.position, 2f);

            Destroy(enemy);
            Destroy(gameObject);
        }
    }
}