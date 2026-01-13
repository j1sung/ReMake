using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveCollider : MonoBehaviour
{
    public PolygonCollider2D target;
    [SerializeField] private bool flag;
    private bool _ready;

    private IEnumerator Start()
    {
        yield return null;   // 1프레임 뒤부터 “진짜 런타임”으로 간주
        _ready = true;
    }

    private void OnDisable()
    {
        if (!_ready) return;

        target.enabled = true;

        if(flag)
            enabled = false;
    }
}
