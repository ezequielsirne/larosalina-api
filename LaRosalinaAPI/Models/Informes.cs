using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace LaRosalinaAPI.Models;

public class InformeRequest
{
    public DateTime Desde { get; set; }
    public DateTime Hasta { get; set; }
}
public class InformesIndicadores
{
    public decimal ocupacion { get; set; }
    public decimal ppn { get; set; }
    public decimal ingresos { get; set; }
    public int cantidad { get; set; }
    public decimal saldo { get; set; }
    public decimal efectivo { get; set; }
    public decimal gastos { get; set; }
    public decimal resultado { get; set; }
}

public class InformesGasto
{
    public string descripcion { get; set; }
    public decimal total { get; set; }
    public decimal participacion { get; set; }
}

public class InformeEconomico
{
    public string mes { get; set; }
    public decimal ocupacion { get; set; }
    public decimal ppn { get; set; }
    public decimal ingresos { get; set; }
    public decimal gastos { get; set; }
    public decimal resultado { get; set; }
}

public class InformeEconomicoNoche
{
    public DateTime Mes { get; set; }
    public int? Noches { get; set; }
    public decimal? TotalPPN { get; set; }
    public decimal? Total { get; set; }
    public decimal? Saldo { get; set; }
}
public class InformeFinanciero
{
    public string cuenta { get; set; }
    public decimal? inicial { get; set; }
    public decimal? ingresos { get; set; }
    public decimal? gastos { get; set; }
    public decimal? salientes { get; set; }
    public decimal? entrantes { get; set; }
    public decimal? final { get; set; }
};

