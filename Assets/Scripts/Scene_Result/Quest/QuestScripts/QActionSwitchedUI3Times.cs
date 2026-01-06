using UnityEngine;

public class QActionSwitchedUI3Times : MonoBehaviour
{
    [SerializeField] private GameObject roomUI;
    [SerializeField] private GameObject bagUI;

    private bool lastRoomActive;
    private bool lastBagActive;
    [SerializeField]private int switchCount = 0;

    void Start()
    {
        // 초기 상태 저장
        lastRoomActive = roomUI.activeSelf;
        lastBagActive = bagUI.activeSelf;
    }

    void Update()
    {
        // 상태 변화 감지
        if (roomUI.activeSelf != lastRoomActive)
        {
            lastRoomActive = roomUI.activeSelf;

            // true로 켜질 때만 전환으로 카운트
            if (roomUI.activeSelf)
                OnUIChanged();
        }

        if (bagUI.activeSelf != lastBagActive)
        {
            lastBagActive = bagUI.activeSelf;

            if (bagUI.activeSelf)
                OnUIChanged();
        }
    }

    private void OnUIChanged()
    {
        switchCount++;

        // 3회 이상이면 업적 이벤트 발송
        if (switchCount >= 3)
        {
            QuestEventManager.TriggerEvent(QuestEventId.switchedUI3Times);
            Debug.Log("[업적] Room-Bag 전환 3회 달성!");
        }
    }
}
