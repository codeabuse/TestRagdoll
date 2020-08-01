using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [Range(0.25f, 250f)]
    public float SpecificCinematicImpulseOnHit = 1f;
    public float CinematicImpulseLimit = 500f;
    
    private Animator animator;

    private bool cinematicDeatPlayed;

    public void Restore()
    {
        ToggleRagdoll(false);
        cinematicDeatPlayed = false;
        GameManager.Instance.RestoreTime();
    }

    public Vector3 GetCinematicDeathImpulse(Rigidbody bodyPart)
    {
        if (cinematicDeatPlayed)
        {
            return Vector3.zero;
        }
        
        GameManager.Instance.SlowTime();
        
        cinematicDeatPlayed = true;
        return Vector3.up * Mathf.Clamp(
                bodyPart.mass * SpecificCinematicImpulseOnHit, 
                1f, 
                CinematicImpulseLimit);
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ToggleRagdoll(bool value)
    {
        animator.enabled = !value;
    }
}
