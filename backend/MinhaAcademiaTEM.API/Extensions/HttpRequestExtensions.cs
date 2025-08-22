namespace MinhaAcademiaTEM.API.Extensions;

public static class HttpRequestExtensions
{
    public static string GetIdempotencyKeyOrNew(this HttpRequest request)
    {
        if (request.Headers.TryGetValue("Idempotency-Key", out var values))
        {
            var key = values.FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(key))
                return key;
        }

        return Guid.NewGuid().ToString();
    }
}