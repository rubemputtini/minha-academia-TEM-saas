namespace MinhaAcademiaTEM.Application.Caching;

public static class CacheKeys
{
    public static string AllUsers(int page, int pageSize) =>
        $"users_page_{page}_pageSize_{pageSize}";

    public static string AllCoaches(int page, int pageSize) =>
        $"coaches_page_{page}_pageSize_{pageSize}";

    public static string CoachClients(Guid coachId) =>
        $"coach_{coachId}_clients";

    public static string CoachClientsPaged(Guid coachId, int page, int pageSize) =>
        $"coach_{coachId}_clients_page_{page}_pageSize_{pageSize}";
}