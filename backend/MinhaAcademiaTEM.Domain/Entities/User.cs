using Microsoft.AspNetCore.Identity;

namespace MinhaAcademiaTEM.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public string Name { get; set; } = string.Empty;
    public Guid? CoachId { get; set; }
    public Coach? Coach { get; set; }
    
    public ICollection<EquipmentSelection> EquipmentSelections { get; set; } = new List<EquipmentSelection>();
}