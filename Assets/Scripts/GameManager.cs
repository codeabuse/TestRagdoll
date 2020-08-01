using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField]
    private BulletData bulletData;
    [Range(0.5f, 5f)]
    public float SlowTimeDuration = 2f;
    [SerializeField]
    private AnimationCurve SlowTimeCurve;

    [Space, Header("Game Components")]
    [SerializeField]
    private Bullet bulletPrefab;
    [SerializeField]
    private Raycaster raycaster;
    [SerializeField]
    private EnemyBehavior enemy;
    [SerializeField]
    private Transform bulletSpawnPoint;

    private void Awake()
    {
        if (Instance)
        {
            Debug.LogError($"Only one GameManager could exist! The one on the \'{name}\' game object is redundant");
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SpawnBullet();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            enemy.Restore();
        }
    }

    private void SpawnBullet()
    {
        var barrelPosition = bulletSpawnPoint.position;
        Quaternion bulletRotation;
        
        if (raycaster.DoPointerRaycast(out var hit))
        {
            bulletRotation = Quaternion.LookRotation((hit.point - barrelPosition).normalized);
        }
        else
        {
            bulletRotation = bulletSpawnPoint.rotation;
        }
        
        var bullet = Instantiate(bulletPrefab, barrelPosition, bulletRotation);
        bullet.Init(bulletData);
    }

    public void SlowTime()
    {
        StartCoroutine(SlowTimeCoroutine());
    }

    public void RestoreTime()
    {
        StopAllCoroutines();
        Time.timeScale = 1f;
    }

    private IEnumerator SlowTimeCoroutine()
    {
        var timer = 0f;
        while (timer <= SlowTimeDuration)
        {
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
            var lerpRatio = timer / SlowTimeDuration;
            Time.timeScale = SlowTimeCurve.Evaluate(lerpRatio);
        }
    }
}
