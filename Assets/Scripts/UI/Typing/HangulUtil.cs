public static class HangulUtil
{
    public static readonly char[] CHO =
    {
        'ㄱ','ㄲ','ㄴ','ㄷ','ㄸ','ㄹ','ㅁ','ㅂ','ㅃ','ㅅ','ㅆ','ㅇ','ㅈ','ㅉ','ㅊ','ㅋ','ㅌ','ㅍ','ㅎ'
    };

    public static readonly char[] JUNG =
    {
        'ㅏ','ㅐ','ㅑ','ㅒ','ㅓ','ㅔ','ㅕ','ㅖ','ㅗ','ㅘ','ㅙ','ㅚ',
        'ㅛ','ㅜ','ㅝ','ㅞ','ㅟ','ㅠ','ㅡ','ㅢ','ㅣ'
    };

    public static readonly char[] JONG =
    {
        '\0','ㄱ','ㄲ','ㄳ','ㄴ','ㄵ','ㄶ','ㄷ','ㄹ','ㄺ','ㄻ','ㄼ',
        'ㄽ','ㄾ','ㄿ','ㅀ','ㅁ','ㅂ','ㅄ','ㅅ','ㅆ','ㅇ','ㅈ','ㅊ',
        'ㅋ','ㅌ','ㅍ','ㅎ'
    };

    public static bool IsHangul(char c)
        => c >= 0xAC00 && c <= 0xD7A3;

    public static void Decompose(char c, out int cho, out int jung, out int jong)
    {
        int code = c - 0xAC00;
        cho = code / 588;
        jung = (code % 588) / 28;
        jong = code % 28;
    }

    public static char Compose(int cho, int jung, int jong)
    {
        return (char)(0xAC00 + cho * 588 + jung * 28 + jong);
    }
}