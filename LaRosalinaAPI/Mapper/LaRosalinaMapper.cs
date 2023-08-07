using AutoMapper;
using LaRosalinaAPI.Models;

namespace LaRosalinaAPI.Mapper
{
    public class LaRosalinaMapper : Profile
    {
        public LaRosalinaMapper()
        {
            //Categorias Gastos
            CreateMap<categorias_gasto, CategoriaGastosDto>().ReverseMap();

            //Comprobantes
            CreateMap<comprobante, ComprobanteDto>().ReverseMap();

            //Cuentas
            CreateMap<cuenta, CuentaDto>().ReverseMap();

            //Departamentos
            CreateMap<departamento, DepartamentoDto>().ReverseMap();

            //Estados Reservas
            CreateMap<estado_reserva, EstadoReservaDto>().ReverseMap();

            //Gastos
            CreateMap<gasto, GastoDto>().ReverseMap();

            //Huespedes
            CreateMap<huespede, HuespedDto>().ReverseMap();

            //Movimientos
            CreateMap<movimiento, MovimientoDto>().ReverseMap();

            //Movimientos tipos
            CreateMap<movimientos_tipo, MovimientoTiposDto>().ReverseMap();

            //Presupuestos
            CreateMap<presupuesto, PresupuestoDto>().ReverseMap();

            //Presuuestos detaññe
            CreateMap<presupuestos_detalle, PresupuestoDetalleDto>().ReverseMap();

            //Reservas
            CreateMap<reserva, ReservaDto>().ReverseMap();
            CreateMap<reserva, ReservaAddDto>().ReverseMap();

            //Usuarios
            CreateMap<usuario, UsuarioDto>().ReverseMap();
            CreateMap<usuario, UsuarioAddDto>().ReverseMap();
            CreateMap<usuario, UsuarioUpdateDto>().ReverseMap();
        }
    }
}
