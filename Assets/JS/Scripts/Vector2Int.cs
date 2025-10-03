/*[System.Serializable]
public struct Vector2Int
{
    public int x, y;

    public Vector2Int(int num1, int num2)
    {
        x = num1;
        y = num2;
    }

    public static string String(Vector2Int a)
    {
        return ("(" + a.x + ", " + a.y + ")");
    }
    public static Vector2Int One
    {
        get { return new Vector2Int(1, 1); }
    }
    public static Vector2Int OneNeg
    {
        get { return new Vector2Int(-1, -1); }
    }
    public static Vector2Int Zero
    {
        get{ return new Vector2Int(0, 0); }
    }
    public static Vector2Int Up
    {
        get { return new Vector2Int(0, 1); }
    }
    public static Vector2Int Down
    {
        get { return new Vector2Int(0, -1); }
    }
    public static Vector2Int Left
    {
        get { return new Vector2Int(-1, 0); }
    }
    public static Vector2Int Right
    {
        get { return new Vector2Int(1, 0); }
    }
    public static Vector2Int operator +(Vector2Int a, Vector2Int b)
    {
        return new Vector2Int(a.x + b.x, a.y + b.y);
    }
    public static Vector2Int operator +(Vector2Int a, int b)
    {
        return new Vector2Int(a.x + b, a.y + b);
    }
    public static Vector2Int operator -(Vector2Int a, Vector2Int b)
    {
        return new Vector2Int(a.x - b.x, a.y - b.y);
    }
    public static Vector2Int operator -(Vector2Int a, int b)
    {
        return new Vector2Int(a.x - b, a.y - b);
    }
    public static Vector2Int operator *(Vector2Int a, int b)
    {
        return new Vector2Int(a.x * b, a.y * b);
    }
    public static Vector2Int operator /(Vector2Int a, int b)
    {
        return new Vector2Int(a.x / b, a.y / b);
    }
    public static int Area(Vector2Int a)
    {
        return (a.x * a.y);
    }
    public static float Slope(Vector2Int a)
    {
        return (a.y / a.x);
    }
    public static void Swap(ref Vector2Int a)
    {
        int temp = a.x;
        a.x = a.y;
        a.y = temp;
    }
    public static bool operator ==(Vector2Int a, Vector2Int b)
    {
        if ((a.x == b.x) && (a.x == b.x))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static bool operator !=(Vector2Int a, Vector2Int b)
    {
        if ((a.x != b.x) || (a.x != b.x))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static bool OrGreater(Vector2Int a, Vector2Int b)
    {
        if (a.x > b.x || a.y > b.y)
        {
            return true;
        }
        return false;
    }
    public static bool OrGreater(Vector2Int a, int b)
    {
        if (a.x > b || a.y > b)
        {
            return true;
        }
        return false;
    }
    public static bool OrLesser(Vector2Int a, Vector2Int b)
    {
        if (a.x < b.x || a.y < b.y)
        {
            return true;
        }
        return false;
    }
    public static bool OrLesser(Vector2Int a, int b)
    {
        if (a.x < b || a.y < b)
        {
            return true;
        }
        return false;
    }
    public override bool Equals(object o)
    {
       return true;
    }
    public override int GetHashCode()
    {
        return 0;
    }
}*/