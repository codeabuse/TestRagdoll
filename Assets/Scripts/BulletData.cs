using UnityEngine;

[CreateAssetMenu(fileName = "New Bullet", menuName = "Bullet")]
public class BulletData : ScriptableObject
{
    public float Speed = 10f;
    public float ImpactForce = 420f;
    public float LifeTime = 2f;
}
