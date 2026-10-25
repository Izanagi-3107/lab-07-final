// Ho ten: Ngô Ngọc Thái
// MSSV: 202418984
// Bai tap: Lab W07

namespace LabW07.Exceptions;

public class DuplicateIdException : Exception
{
    public string DuplicateId { get; }

    public DuplicateIdException(string duplicateId)
        : base($"Ma {duplicateId} da ton tai.")
    {
        DuplicateId = duplicateId;
    }
}
