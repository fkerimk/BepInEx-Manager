namespace BepInExManager.Tools;

internal static class StringTools {

    public static bool EqualsAnyOfIgnoreCase (this string text, params string[] values){

        foreach (string value in values) {

            if (text.Equals(value, StringComparison.OrdinalIgnoreCase))
                return true;
        }

        return false;
    }
}