namespace Wpflab3.Models;

public class RegistrationProgram
{
    public long RegistrationProgramId { get; set; }
    public DateTime ProgramDate { get; set; }
    public long ProgramCode { get; set; }
    public string ProgramName { get; set; }
    public int Duration { get; set; }
    public int Regularity { get; set; }
    public double Price { get; set; }
}