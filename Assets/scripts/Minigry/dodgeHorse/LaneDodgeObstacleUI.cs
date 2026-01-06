using UnityEngine;

public class LaneDodgeObstacleUI : MonoBehaviour
{
    [HideInInspector] public LaneDodgeManager manager;
    [HideInInspector] public LaneDodgeSpawnerUI spawner;

    public RectTransform rect;
    public float destroyY = -350f;

    private float speed;
    private bool scored = false;

    private void Awake()
    {
        if (rect == null) rect = GetComponent<RectTransform>();
    }

    public void SetStartPos(Vector2 pos, float moveSpeed)
    {
        rect.anchoredPosition = pos;
        speed = moveSpeed;
        scored = false;
    }

    private void Update()
    {
        float dt = Time.unscaledDeltaTime;
        rect.anchoredPosition += Vector2.down * speed * dt;

        // “unik” = gdy minê³a dó³ ekranu
        if (!scored && rect.anchoredPosition.y < 0f)
        {
            scored = true;
            manager?.AddScore(1);
        }

        // KOLIZJA NA RECTACH (pewniak w UI)
        if (manager != null && manager.player != null && manager.player.rect != null)
        {
            if (RectsOverlap(rect, manager.player.rect))
            {
                manager.GameOver();
                return;
            }
        }

        if (rect.anchoredPosition.y <= destroyY)
            Destroy(gameObject);
    }

    private bool RectsOverlap(RectTransform a, RectTransform b)
    {
        Vector3[] ac = new Vector3[4];
        Vector3[] bc = new Vector3[4];
        a.GetWorldCorners(ac);
        b.GetWorldCorners(bc);

        float aMinX = ac[0].x, aMaxX = ac[2].x;
        float aMinY = ac[0].y, aMaxY = ac[2].y;

        float bMinX = bc[0].x, bMaxX = bc[2].x;
        float bMinY = bc[0].y, bMaxY = bc[2].y;

        bool overlapX = aMinX <= bMaxX && aMaxX >= bMinX;
        bool overlapY = aMinY <= bMaxY && aMaxY >= bMinY;

        return overlapX && overlapY;
    }
}
