using System.ComponentModel.DataAnnotations;

public class DTORecordPatients
{
    [Key]
    public int RecurdId { get; set; }
    public int PatientID { get; set; }
    public DateTime RequestDate { get; set; }
    public int TechnicianiD { get; set; }
}

