namespace MinhaAcademiaTEM.Domain.Entities;

public class BaseEquipment : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string PhotoUrl { get; private set; } = string.Empty;
    public string VideoUrl { get; private set; } = string.Empty;

    public MuscleGroup MuscleGroup { get; private set; }

    private readonly List<Equipment> _customizations = [];
    public IReadOnlyCollection<Equipment> Customizations => _customizations.AsReadOnly();

    protected BaseEquipment()
    {
    }

    public BaseEquipment(string name, string photoUrl, string videoUrl, MuscleGroup muscleGroup)
    {
        Name = name.Trim();
        PhotoUrl = photoUrl.Trim();
        VideoUrl = videoUrl.Trim();
        MuscleGroup = muscleGroup;
    }

    public void UpdateName(string newName)
    {
        Name = newName.Trim();
    }

    public void UpdateMedia(string newPhotoUrl, string newVideoUrl)
    {
        PhotoUrl = newPhotoUrl.Trim();
        VideoUrl = newVideoUrl.Trim();
    }

    public void UpdateMuscleGroup(MuscleGroup newMuscleGroup)
    {
        MuscleGroup = newMuscleGroup;
    }
}