namespace Web_WorkTrackingApi
{
    public class WorkTracking_Project
    {
        public int Index { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public bool Status { get; set; } = false;
    }
    public class WorkTracking_Task
    {
        public int Index { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ProjectIndex { get; set; }
        public bool Status { get; set; } = false;
    }
    public class WorkTracking_JobPost
    {
        public int Index { get; set; }
        public string Date { get; set; } = string.Empty;
        public int Hours { get; set; }
        public string Description { get; set; } = string.Empty;
        public int TaskIndex { get; set; }
    }
}