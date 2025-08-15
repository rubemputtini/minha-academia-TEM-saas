namespace MinhaAcademiaTEM.Application.Caching;

public static class CacheKeys
{
    public static string AllBaseEquipments => "base_equipments";

    public static string AllUsers(int page, int pageSize, int totalUsers) =>
        $"users_page_{page}_pageSize_{pageSize}_totalUsers_{totalUsers}";

    public static string AllCoaches(int page, int pageSize, int totalCoaches) =>
        $"coaches_page_{page}_pageSize_{pageSize}_totalCoaches_{totalCoaches}";

    public static string CoachClients(Guid coachId) =>
        $"coach_{coachId}_clients";

    public static string CoachClientsPaged(Guid coachId, int page, int pageSize, int totalClients) =>
        $"coach_{coachId}_clients_page_{page}_pageSize_{pageSize}_totalClients_{totalClients}";

    public static string CoachEquipments(Guid coachId) =>
        $"coach_{coachId}_equipments";

    public static string CoachActiveEquipments(Guid coachId) =>
        $"coach_{coachId}_active_equipments";

    public static string UserEquipmentSelections(Guid userId) =>
        $"user_{userId}_equipments_selections";

    public static string UserAvailableEquipmentSelections(Guid userId) =>
        $"user_{userId}_available_equipments_selections";

    public static string UserEquipmentNotes(Guid userId) =>
        $"user_{userId}_notes";
}