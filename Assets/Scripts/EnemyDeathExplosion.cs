using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathExplosion : MonoBehaviour
{
    bool explode;

    void LateUpdate()
    {
        if (transform.parent == null)
        {
            GetComponent<ParticleSystem>().Play();
            explode = true;
        }
        if (GetComponent<ParticleSystem>().isStopped && explode)
        {
            Destroy(gameObject);
        }
    }
}
