using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ShapeUtil
{
    public static Vector2Int RotateCW(Vector2Int p) => new Vector2Int(p.y, -p.x);

    public static Vector2Int[] GetRotated(IList<Vector2Int> cells, int rot /* 0~3 */)
    {
        var arr = new Vector2Int[cells.Count];
        for (int i=0; i < cells.Count; i++)
        {
            Vector2Int v = cells[i];
            for (int r = 0; r < rot; r++) v = RotateCW(v);
            arr[i] = v;
        }
        return arr;
    }

    public static Vector2Int GetBoundsSize(IList<Vector2Int> cells)
    {
        if (cells == null || cells.Count == 0) return Vector2Int.one;

        int minX = int.MaxValue, minY = int.MaxValue;
        int maxX = int.MinValue, maxY = int.MinValue;

        foreach(var c in cells)
        {
            if (c.x < minX) minX = c.x;
            if (c.y < minY) minY = c.y;
            if (c.x > maxX) maxX = c.x;
            if (c.y > maxY) maxY = c.y;
        }
        return new Vector2Int(maxX - minX + 1, maxY - minY + 1);
    }

    // 줄 정규화: '0'과 '1'만 깨끗하게 남기기
    public static string NormalizeRow(string row)
    {
        if(string.IsNullOrEmpty(row)) return string.Empty;
        return new string(row.Where(ch => ch == '0' || ch == '1').ToArray());
    }

    // maskRows('1'=점유) -> 셀 목록
    public static Vector2Int[] MaskToCells(string[] rows)
    {
        var list = new List<Vector2Int>();
        if (rows == null || rows.Length == 0) return list.ToArray();
        /*처리할 데이터가 없다고 해서 null을 반환하면, 이 함수를 호출한 다른 코드에서 에러가 발생할 확률이 매우 높음 
         하지만 이 코드처럼 내용물이 없는 빈 배열(new Vector2Int[0])을 반환하면, 
        함수를 호출한 쪽에서는 데이터가 있든 없든 동일한 방식으로 코드를 처리할 수 있어 훨씬 안전하고 편리함*/
        
        // y=0을 "위쪽"으로 간주 -> (h-1-y)로 뒤집음
        int h = rows.Length;
        for (int y = 0; y < h; y++)
        {
            string row = NormalizeRow(rows[y]);
            for (int x = 0; x < row.Length; x++)
            {
                if (row[x] == '1')
                {
                    // y축 뒤집어서 (0,0)이 좌하단이 되게
                    list.Add(new Vector2Int(x, (h - 1) - y));
                }
            }
        }
        return list.ToArray();
    }

    // 정규화 폭(피벗 계산용)
    public static int NormalizedWidth(string[] rows)
    {
        int w = 0;
        if (rows == null) return 0;
        foreach (var r in rows)
            w = Mathf.Max(w, NormalizeRow(r).Length);
        return w;
    }

    public static void GetAABB(IList<Vector2Int> cells, out Vector2Int min, out Vector2Int max)
    {
        if (cells == null || cells.Count == 0) { min = max = Vector2Int.zero; return; }
        int minX = int.MaxValue, minY = int.MaxValue, maxX = int.MinValue, maxY = int.MinValue;
        for (int i = 0; i < cells.Count; i++)
        {
            var c = cells[i];
            if (c.x < minX) minX = c.x; if (c.y < minY) minY = c.y;
            if (c.x > maxX) maxX = c.x; if (c.y > maxY) maxY = c.y;
        }
        min = new Vector2Int(minX, minY);
        max = new Vector2Int(maxX, maxY);
    }
}