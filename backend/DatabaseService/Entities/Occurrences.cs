using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DatabaseService.Entities;

[PrimaryKey(nameof(WordId), nameof(FileId))]
public class Occurrences
{
    public required int WordId { get; set; }
    public required int FileId { get; set; }
    public required int Count { get; set; }
    public Words Word { get; set; }
    public Emails Email { get; set; }
}