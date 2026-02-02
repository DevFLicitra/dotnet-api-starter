namespace Api.Contracts.Projects;

public sealed record ProjectResponse(
    Guid Id,
    string Name,
    string? Description,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt
    );


public sealed record ProjectCreateRequest(
    string Name,
    string? Description
    
    );


public sealed record ProjectUpdateRequest(
    string Name,
    string? Description
    
    );