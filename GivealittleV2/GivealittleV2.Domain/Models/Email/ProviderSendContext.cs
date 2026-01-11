public sealed record ProviderSendContext(
    string ToEmail,
    string Subject,
    string Body,
    bool IsHtml,
    string FromEmail,
    object ProviderConfig);