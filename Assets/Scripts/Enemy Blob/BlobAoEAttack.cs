using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobAoEAttack : MonoBehaviour
{

    [SerializeField] int damage;
    [SerializeField] float expansionDuration;
    [SerializeField] float maxExpansion;

    bool _alreadyDamage;
    Vector3 _startScale;
    // Start is called before the first frame update
    void Start()
    {
        _startScale = transform.localScale;
        StartCoroutine(AttackExpansion());
    }

    IEnumerator AttackExpansion()
    {

        float timer = 0.0f;
        Vector3 scaleTemp = transform.localScale;
        while (timer <= expansionDuration)
        {
            //se agranda durante 2 segundos
            timer += Time.deltaTime;

            scaleTemp.x = Mathf.Lerp(_startScale.x, maxExpansion, timer / expansionDuration);
            scaleTemp.z = Mathf.Lerp(_startScale.z, maxExpansion, timer / expansionDuration);
            transform.localScale = scaleTemp;


            yield return null;
        }

        Destroy(gameObject);
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_alreadyDamage && other.gameObject.CompareTag("Player")) { 
            PlayerStats.myInstance.Damage(damage);
            _alreadyDamage = true;
        }
    }
}
