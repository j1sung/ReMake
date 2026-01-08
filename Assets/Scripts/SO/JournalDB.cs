using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JournalDB", menuName = "Scriptable Objects/JournalDB")]
public class JournalDB : ScriptableObject
{
    [SerializeField] private List<JournalData> journalData;
    
    [SerializeField] private Sprite unsigned;
    [SerializeField] private Sprite signed;

    public JournalData FindJournal(int n)
    {
        return journalData[n - 1];
    }
}
