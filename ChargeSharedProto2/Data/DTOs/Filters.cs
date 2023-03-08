using System.ComponentModel.DataAnnotations;
using ChargeSharedProto2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.CompilerServices;

namespace ChargeSharedProto2.Data.DTOs;

public class Filters
{
    public bool useFilters { get; set; } = false;

    public string? adresPostalCity { get; set; } = "";

    public double? minPrice { get; set; } = Double.MinValue;
    public double? maxPrice { get; set; } = Double.MaxValue;
    public ChargerType chargerType { get; set; } = ChargerType.Null;

    public bool filterFastCharge { get; set; } = false;
    public bool fastCharge { get; set; }
    public DateOnly availibleDate { get; set; }
    public TimeOnly availibleTime { get; set; }
}