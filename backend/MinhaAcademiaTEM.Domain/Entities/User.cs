using Microsoft.AspNetCore.Identity;

namespace MinhaAcademiaTEM.Domain.Entities;

public sealed class User : IdentityUser<Guid>
{
    public string Name { get; private set; } = string.Empty;
    public Guid? CoachId { get; private set; }
    public Coach? Coach { get; private set; }


    private readonly List<EquipmentSelection> _equipmentSelections = [];
    public IReadOnlyCollection<EquipmentSelection> EquipmentSelections => _equipmentSelections.AsReadOnly();

    private User()
    {
    }

    public User(string name, string email)
    {
        Name = name.Trim();
        Email = email;
        UserName = email;
    }

    public void AssignCoach(Guid coachId) => CoachId = coachId;

    public void UpdateName(string name) => Name = name.Trim();
}