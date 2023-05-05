namespace CourseViewerApi.Common.DTO
{
    public class ServiceResult
    {
        public bool Success { get; set; } = true;

        public List<string> Errors { get; set; } = new();
    }

    public class ServiceResult<T> : ServiceResult
    {
        public T ResultValue { get; set; }
    }
}
