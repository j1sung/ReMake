using UnityEngine;

public class QuestTrigger : MonoBehaviour
{
    public QuestData targetQuest; // 해금할 업적 설정

    public void TriggerQuest()
    {
        // 달성 사운드 이펙트... 추가?
        ResultManager.instance.UnlockQuest(targetQuest);
    }
}
