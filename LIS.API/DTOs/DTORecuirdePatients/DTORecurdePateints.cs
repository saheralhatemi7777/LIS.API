namespace APiUsers.DTOs
{
    public class DTORecurdePateints
    {
        public int Recordid { get;  set; }

        public string? pateintName { get; set; }
        public DateTime pateintBirthDate { get; set; }
        public bool pateintGender { get; set; }
        public string? pateintphoneNumber { get; set; }
        public string? pateintAddress { get; set;}
        public string? testName {get; set;}
        public string? TestName {get; internal set;}
        public object? Tests { get; internal set; }
        public int? TestId { get; internal set; }
        public int? Price { get; internal set; }
    }
}
