namespace MinhaAcademiaTEM.Domain.Entities;

public class Equipment : CoachEntity
{
    public string Name { get; private set; } = string.Empty;
    public string VideoUrl { get; private set; } = string.Empty;

    public MuscleGroup MuscleGroup { get; private set; }

    public Guid BaseEquipmentId { get; private set; }
    public BaseEquipment BaseEquipment { get; private set; } = null!;
    public bool IsActive { get; private set; } = true;

    protected Equipment()
    {
    }

    public Equipment(
        string name,
        string videoUrl,
        MuscleGroup muscleGroup,
        Guid baseEquipmentId,
        Guid coachId)
        : base(coachId)
    {
        Name = name.Trim();
        VideoUrl = videoUrl.Trim();
        MuscleGroup = muscleGroup;
        BaseEquipmentId = baseEquipmentId;
        IsActive = true;
    }

    public void UpdateInfo(string name, string videoUrl, MuscleGroup muscleGroup)
    {
        Name = name.Trim();
        VideoUrl = videoUrl.Trim();
        MuscleGroup = muscleGroup;
    }

    public void SetActive(bool isActive) => IsActive = isActive;
}