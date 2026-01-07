public sealed record EmailSendRequestDTO(
    string ToEmail,
    string ToFirstName,
    string TemplateKey,
    IReadOnlyDictionary<string,object?> EmailDTO
);