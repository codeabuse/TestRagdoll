using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed { get; private set; }
    public float ImpactForce { get; private set; }
    
    public float LifeTime { get; private set; }

    private Vector3 velocity;

    public void Init(BulletData data)
    {
        Speed = data.Speed;
        ImpactForce = data.ImpactForce;
        LifeTime = data.LifeTime;
        velocity = transform.forward * Speed;
        StartCoroutine(MoveBullet());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }

        var body = other.attachedRigidbody;
        if (!body)
        {
            return;
        }
        
        Vector3 additionalImpulse = Vector3.zero;

        var enemy = other.transform.root.GetComponent<EnemyBehavior>();
        if (enemy)
        {
            enemy.ToggleRagdoll(true);
            additionalImpulse = enemy.GetCinematicDeathImpulse(body);
        }
        
        body.AddForce(transform.forward * ImpactForce + additionalImpulse, ForceMode.Impulse);
        StopAllCoroutines();
        Destroy(gameObject);
    }

    private IEnumerator MoveBullet()
    {
        var timer = LifeTime;
        while (timer > 0)
        {
            yield return new WaitForEndOfFrame();
            transform.position += velocity * Time.deltaTime;
            timer -= Time.deltaTime;
        }

        Destroy(gameObject);
    }
}
