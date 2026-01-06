using UnityEngine;

public class LaneDodgeSpawnerUI : MonoBehaviour
{
    [HideInInspector] public LaneDodgeManager manager;

    [Header("Prefab & Parent")]
    public LaneDodgeObstacleUI obstaclePrefab;
    public RectTransform obstaclesParent;

    [Header("Lanes")]
    public float laneOffset = 160f;

    [Header("Spawn")]
    public float spawnY = 350f;
    public float spawnInterval = 0.9f;

    [Header("Difficulty - time ramp")]
    public float speed = 520f;
    public float intervalMin = 0.35f;
    public float speedMax = 800f;
    public float difficultyRamp = 0.03f;   // rośnie z czasem

    [Header("Difficulty - score boost")]
    public float scoreSpeedBoost = 8f;      
    public float scoreIntervalBoost = 0.0025f;

    private float timer;
    private float startSpawnInterval;
    private float startSpeed;


    public void ResetSpawner()
    {
        timer = 0f;

        spawnInterval = startSpawnInterval;
        speed = startSpeed;

        if (obstaclesParent != null)
        {
            for (int i = obstaclesParent.childCount - 1; i >= 0; i--)
                Destroy(obstaclesParent.GetChild(i).gameObject);
        }
    }

    public void Tick(float dt)
    {
        // ramp trudności w czasie
        spawnInterval = Mathf.Max(intervalMin, spawnInterval - difficultyRamp * dt);
        speed = Mathf.Min(speedMax, speed + (difficultyRamp * 800f) * dt);

        timer += dt;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnOne();
        }
    }
    private void Awake()
    {
        startSpawnInterval = spawnInterval;
        startSpeed = speed;
    }


    public void OnScoreGained(int delta)
    {
        // skok trudności za punkty
        speed = Mathf.Min(speedMax, speed + scoreSpeedBoost * delta);
        spawnInterval = Mathf.Max(intervalMin, spawnInterval - scoreIntervalBoost * delta);
    }

    private void SpawnOne()
    {
        if (obstaclePrefab == null || obstaclesParent == null) return;

        int laneIndex = Random.Range(0, 3);
        float x = (laneIndex - 1) * laneOffset;

        LaneDodgeObstacleUI o = Instantiate(obstaclePrefab, obstaclesParent);
        o.manager = manager;
        o.spawner = this;
        o.SetStartPos(new Vector2(x, spawnY), speed);
    }
}
