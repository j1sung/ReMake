using UnityEngine;

public class GameSaveParticipant: GameSaveParticipantBehaviour
{
    // 유일하게 씬 이동이 있는 참가자는 Order 가장 크게 하기(씬 이동 기준)
    public override int Order => 2;
    public override ApplyPhase Phase => ApplyPhase.BeforeSceneLoad;

    public override void Capture(GameSaveData data)
    {
        // data.sceneName = GameManager.Instance.CurrentScene;
        // stageNum 저장
        // Debug.Log("stageNum Capture!");
    }

    public override void Apply(GameSaveData data)
    {
        // stageNum 적용
        // Debug.Log("stageNum Apply!");
    }
}
