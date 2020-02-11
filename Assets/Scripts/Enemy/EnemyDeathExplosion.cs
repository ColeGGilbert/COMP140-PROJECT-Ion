using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathExplosion : MonoBehaviour
{
    bool explode;

    void LateUpdate()
    {
        if (transform.parent == null && !explode)
        {
            StartCoroutine(ExplosionEffect());
            explode = true;
        }
        if (!GetComponent<ParticleSystem>().isPlaying && explode)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator ExplosionEffect()
    {
        GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(.2f);
        GetComponent<ParticleSystem>().Stop();
    }
}
