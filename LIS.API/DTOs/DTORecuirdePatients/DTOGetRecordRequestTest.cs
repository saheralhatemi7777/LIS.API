
public class PatientFullRecordDto
{
    // بيانات المريض
    public int PatientId { get; set; }
    public string FullName { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string username { get; set; }

    // بيانات السجل + الطلبات + التحاليل
    public List<RecordData> Records { get; set; }

    public class RecordData
    {
        public int RecordId { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }

        public List<RequestData> Requests { get; set; }
    }

    public class RequestData
    {
        public int RequestId { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }

        public List<TestData> Tests { get; set; }
    }

    public class TestData
    {
        public int TestId { get; set; }
        public string TestNameEn { get; set; }
        public string TestNameAr { get; set; }
        public string SampleType { get; set; }
        public string NormalRange { get; set; }
        public decimal TestPrice { get; set; }
        public int? ResultValue { get; set; }
        public List<TestResultData> Results { get; set; }
    }

    public class TestResultData
    {
        public int ResultId { get; set; }
        public string ResultValue { get; set; }
        public DateTime CreatedAt { get; set; }

    }

}
