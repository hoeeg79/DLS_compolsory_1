
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseService.Entities;

public class Emails
{
    [Key] // Explicitly mark it as the primary key
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
    public int FileId { get; set; }
    
    public required string FileName { get; set; }
    
    public required byte[] FileContent { get; set; }
}