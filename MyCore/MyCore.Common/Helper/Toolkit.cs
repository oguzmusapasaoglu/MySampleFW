using Newtonsoft.Json;
public static class Toolkit
{
    public static List<TResult> FromJson<TResult>(this string jsonData) where TResult : class
        => (jsonData.IsNotNullOrEmpty()) ? JsonConvert.DeserializeObject<List<TResult>>(jsonData)
        : new List<TResult>();
    public static string ToJson<TRequest>(this TRequest data) where TRequest : class => JsonConvert.SerializeObject(data);
    public static int ToInt(this bool value) => (value) ? 1 : 0;
    public static bool IsNullOrEmpty(this object value) =>
        (value == null || value.ToString().Trim() == string.Empty);
    public static bool IsNullOrLessOrEqToZero(this object value)
    {
        return (value == null || value.ToLong() <= 0);
    }
    public static bool IsNotNullOrLessOrEqToZero(this object value)
    {
        return (value != null && value.ToLong() >= 0);
    }
    public static bool DataIsNullOrEmpty<T>(this IEnumerable<T>? value) =>
        (value == null || !value.Any());
    public static bool IsNotNullOrEmpty(this object value) =>
        (value == null || value.ToString().Trim() == string.Empty) ? false : true;
    public static int ToInt(this object value)
    {
        int ParmOut;
        return int.TryParse(value.ToString(), out ParmOut)
            ? ParmOut
            : 0;
    }
    public static int? ToNullOrInt(this object value)
    {
        if (value == null)
            return null;
        int ParmOut;
        return int.TryParse(value.ToString(), out ParmOut)
            ? ParmOut
            : 0;
    }
    public static long ToLong(this object value)
    {
        long ParmOut;
        return long.TryParse(value.ToString(), out ParmOut)
            ? ParmOut
            : 0;
    }
}