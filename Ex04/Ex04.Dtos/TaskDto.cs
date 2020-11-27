using Ex04.DataAccess.Example3.Enums;

namespace Ex04.Dtos
{
    public record TaskDto(
        int Id,
        string Description,
        TaskPriorityLevel PriorityLevel,
        bool IsCompleted);
}
