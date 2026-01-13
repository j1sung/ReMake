using System.Collections.Generic;
using UnityEngine;

public class ResultSaveParticipant : GameSaveParticipantBehaviour
{
    public override int Order => 4;
    public override ApplyPhase Phase => ApplyPhase.BeforeSceneLoad;

    public override void Capture(GameSaveData data)
    {
        // stageNum 저장
        data.stageNum = ResultManager.instance.CurrentStageInfo;

        // resultId 저장
        data.resultId.Clear(); // 항상 새로 덮어씌움
        for (int i = 0; i < ResultManager.instance.endingResult.Count; i++)
        {
            data.resultId.Add(ResultManager.instance.endingResult[i].id);
        }

        // objeByStage 저장
        data.objeByStage.Clear();
        for (int i = 0; i < ResultManager.instance.endingObjes.Count; i++)
        {
            EndingObje srcStage = ResultManager.instance.endingObjes[i];

            StageObjeSave stageSave = new StageObjeSave();   // objeIds는 생성자에서 이미 new 됨

            if (srcStage?.objs != null)
            {
                for (int j = 0; j < srcStage.objs.Count; j++)
                {
                    ObjeData obje = srcStage.objs[j];
                    if (obje == null) continue;

                    // ObjeData에서 저장할 문자열 필드 선택 (보통 id)
                    stageSave.objeIds.Add(obje.id);
                }
            }

            data.objeByStage.Add(stageSave);
        }

        //questId 저장
        data.questId.Clear();
        foreach (var q in ResultManager.instance.unlockedQuests)
        {
            data.questId.Add(q);
        }
    }
    public override void Apply(GameSaveData data)
    {
        // stageNum 적용
        ResultManager.instance.SetStage(data.stageNum);

        // resultId 적용
        ResultManager.instance.SetResult(data.resultId);


        // objeByStage 적용
        ResultManager.instance.SetObje(data.objeByStage);

        //questId 적용
        ResultManager.instance.SetQuest(data.questId);
    }
}
