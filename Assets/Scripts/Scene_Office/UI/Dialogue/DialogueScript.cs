using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "SO/Dialogue Script")]
public class DialogueScript : ScriptableObject
{
    public List<DialogueCtx> lines = new();
}