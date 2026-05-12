using System.ComponentModel.DataAnnotations;

namespace GadgetShop.Models;

public sealed class CustomerAddressMutationRequest
{
    public long? Id { get; set; }
    [Required, StringLength(100)] public string FullName { get; set; } = string.Empty;
    [Required] public string AddressLine1 { get; set; } = string.Empty;
    public string? AddressLine2 { get; set; }
    [Required] public string City { get; set; } = string.Empty;
    [Required] public string State { get; set; } = string.Empty;
    [Required] public string PinCode { get; set; } = string.Empty;
    [Required, Phone] public string PhoneNumber { get; set; } = string.Empty;
    public bool IsDefault { get; set; }
}

public sealed class CustomerAddressDeleteRequest
{
    [Range(1, long.MaxValue)] public long Id { get; set; }
}

public sealed class CustomerAddressDto
{
    public long Id { get; set; }
    public string? FullName { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PinCode { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IsDefault { get; set; }
}
