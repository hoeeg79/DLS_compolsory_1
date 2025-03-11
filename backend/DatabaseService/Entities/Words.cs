using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseService.Entities;

public class Words
{
    [Key] // Explicitly mark it as the primary key
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
    public int WordId { get; set; }
    
    public required string Word { get; set; }
}