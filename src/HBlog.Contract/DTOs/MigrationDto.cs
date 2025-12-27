namespace HBlog.Contract.DTOs;

public record MigrationDto
{
    public required MigrationDetail AppliedMigration { get; set; }
    public required MigrationDetail PendingMigration { get; set; }
}
public record MigrationDetail
{
    public required int Count { get; set;}
    public required IEnumerable<string> MigrationNames { get; set; }
}