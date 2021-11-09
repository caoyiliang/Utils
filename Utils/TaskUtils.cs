namespace Utils
{
    public static class TaskUtils
    {
        public static readonly Task<object> NullTask = Task.FromResult<object>(null);
    }
}
