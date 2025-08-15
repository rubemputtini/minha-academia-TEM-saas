namespace MinhaAcademiaTEM.Domain.Entities;

public class EquipmentNote : CoachEntity
{
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;
    public string Message { get; private set; } = string.Empty;

    protected EquipmentNote()
    {
    }

    public EquipmentNote(Guid coachId, Guid userId, string message) : base(coachId)
    {
        UserId = userId;
        Update(message);
    }

    public void Update(string message)
    {
        Message = message.Trim();
    }
}