using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private const float c_maxSpecificImpulse = 33f;
    
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

        var resultingImpulse = transform.forward * ImpactForce + additionalImpulse;
        resultingImpulse = LimitImpulse(resultingImpulse, body.mass);
        body.AddForce(resultingImpulse, ForceMode.Impulse);
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

    private static Vector3 LimitImpulse(Vector3 force, float mass)
    {
        if (force.sqrMagnitude < c_maxSpecificImpulse * c_maxSpecificImpulse)
            return force;
        
        var maxMagnitude = mass * c_maxSpecificImpulse;
        return force.normalized  * maxMagnitude;
    }
}
