namespace MarquitoUtils.Main.Exceptions.Tools
{
    public static class ExceptionUtils
    {
        public static T GetData<T>(this Exception exception, string dataName)
        {
            return (T)exception.Data[dataName];
        }
    }
}
