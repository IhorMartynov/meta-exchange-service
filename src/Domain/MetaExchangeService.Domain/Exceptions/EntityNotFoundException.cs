using System.Runtime.Serialization;

namespace MetaExchangeService.Domain.Exceptions;

[Serializable]
public sealed class EntityNotFoundException : Exception
{
    public string? EntityName { get; set; }
    public string? EntityId { get; set; }

    public EntityNotFoundException() { }

    public EntityNotFoundException(string entityName, object entityId)
        : base($"Cannot find {entityName} with the id = {entityId}")
    {
        EntityName = entityName;
        EntityId = entityId.ToString();
    }

    private EntityNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        if (info == null)
            throw new ArgumentNullException(nameof(info));

        EntityName = info.GetString(nameof(EntityName));
        EntityId = info.GetString(nameof(EntityId));
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);

        if (info == null)
            throw new ArgumentNullException(nameof(info));

        info.AddValue(nameof(EntityName), EntityName);
        info.AddValue(nameof(EntityId), EntityId);
    }
}