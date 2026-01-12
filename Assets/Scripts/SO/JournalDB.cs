using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JournalDB", menuName = "Scriptable Objects/JournalDB")]
public class JournalDB : ScriptableObject
{
    [SerializeField] private List<JournalData> journalData;

    public JournalData FindJournal(int n)
    {
        return journalData[n - 1];
    }
}
