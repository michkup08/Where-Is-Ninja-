using UnityEngine;

public class LaneDodgePlayerUI : MonoBehaviour
{
    [HideInInspector] public LaneDodgeManager manager;

    [Header("Lanes")]
    public RectTransform rect;
    public float laneOffset = 160f;      // odleg³oœæ w px miêdzy pasami
    public float moveSpeed = 18f;        // szybkoœæ przejœcia

    private int laneIndex = 1;           // 0..2
    private Vector2 targetPos;

    private void Awake()
    {
        if (rect == null) rect = GetComponent<RectTransform>();
    }

    public void ResetPlayer()
    {
        laneIndex = 1;
        targetPos = new Vector2(GetLaneX(laneIndex), rect.anchoredPosition.y);
        rect.anchoredPosition = targetPos;
    }

    private void Update()
    {
        // minigra dzia³a na unscaled time (bo Time.timeScale mo¿e byæ 0)
        float dt = Time.unscaledDeltaTime;

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            ChangeLane(-1);

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            ChangeLane(+1);

        rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, targetPos, moveSpeed * dt);
    }

    private void ChangeLane(int dir)
    {
        laneIndex = Mathf.Clamp(laneIndex + dir, 0, 2);
        targetPos = new Vector2(GetLaneX(laneIndex), rect.anchoredPosition.y);
    }

    private float GetLaneX(int index) => (index - 1) * laneOffset;
}
