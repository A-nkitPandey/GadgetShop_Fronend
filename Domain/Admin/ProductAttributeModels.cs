using System.ComponentModel.DataAnnotations;

namespace GadgetShop.Models;

public class CreateAttributeMasterRequest
{
    [Required] public string AttributeCode { get; set; } = string.Empty;
    [Required] public string AttributeName { get; set; } = string.Empty;
    [Required] public string DataType { get; set; } = string.Empty;
    public bool IsVariantAttribute { get; set; }
    public bool IsFilterable { get; set; }
    public int DisplayOrder { get; set; }
}

public sealed class UpdateAttributeMasterRequest : CreateAttributeMasterRequest
{
    [Range(1, long.MaxValue)] public long Id { get; set; }
}

public sealed class AttributeMasterGetByIdRequest
{
    [Range(1, long.MaxValue)] public long Id { get; set; }
}

public sealed class AttributeMasterListRequest : PaginationRequest { }

public sealed class AttributeMasterStatusUpdateRequest
{
    [Range(1, long.MaxValue)] public long Id { get; set; }
    public bool IsActive { get; set; }
}

public class CreateAttributeValueRequest
{
    [Range(1, long.MaxValue)] public long AttributeId { get; set; }
    [Required] public string ValueCode { get; set; } = string.Empty;
    [Required] public string ValueText { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
}

public sealed class UpdateAttributeValueRequest : CreateAttributeValueRequest
{
    [Range(1, long.MaxValue)] public long Id { get; set; }
}

public sealed class AttributeValueGetByIdRequest
{
    [Range(1, long.MaxValue)] public long Id { get; set; }
}

public sealed class AttributeValueListRequest : PaginationRequest
{
    public long? AttributeId { get; set; }
}

public sealed class AttributeValueStatusUpdateRequest
{
    [Range(1, long.MaxValue)] public long Id { get; set; }
    public bool IsActive { get; set; }
}

public sealed class SaveVariantAttributeMappingRequest
{
    [Range(1, long.MaxValue)] public long VariantId { get; set; }
    public List<VariantAttributeAssignmentRequest> Mappings { get; set; } = new();
}

public sealed class VariantAttributeAssignmentRequest
{
    [Range(1, long.MaxValue)] public long AttributeId { get; set; }
    [Range(1, long.MaxValue)] public long AttributeValueId { get; set; }
}

public sealed class VariantAttributeMappingGetByVariantIdRequest
{
    [Range(1, long.MaxValue)] public long VariantId { get; set; }
}

public sealed class AttributeMasterDto
{
    public long Id { get; set; }
    public string AttributeCode { get; set; } = string.Empty;
    public string AttributeName { get; set; } = string.Empty;
    public string DataType { get; set; } = string.Empty;
    public bool IsVariantAttribute { get; set; }
    public bool IsFilterable { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
}

public sealed class AttributeValueDto
{
    public long Id { get; set; }
    public long AttributeId { get; set; }
    public string AttributeName { get; set; } = string.Empty;
    public string ValueCode { get; set; } = string.Empty;
    public string ValueText { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
}

public sealed class VariantAttributeMappingDto
{
    public long AttributeId { get; set; }
    public string AttributeName { get; set; } = string.Empty;
    public long AttributeValueId { get; set; }
    public string ValueCode { get; set; } = string.Empty;
    public string ValueText { get; set; } = string.Empty;
}
