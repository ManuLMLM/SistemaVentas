using SistemaVentas.AplicacionWeb.Models.ViewModels;
using SistemaVentas.Entity;
using System.Globalization;
using AutoMapper;
namespace SistemaVentas.AplicacionWeb.Utilidades.AutoMapper
{
    public class AutoMapperProfile: Profile//Profile es de AutoMapper
    {
        public AutoMapperProfile()
        {
            CreateMap<Rol, VMRol>().ReverseMap();

            CreateMap<Usuario, VMUsuario>()
                .ForMember(d=>d.EsActivo,e=>e.MapFrom(f=>f.EsActivo == true ? 1 : 0))
                .ForMember(d=>d.NombreRol,e=>e.MapFrom(f=>f.IdRolNavigation.Descripcion));
            CreateMap<VMUsuario, Usuario>()
                .ForMember(d => d.EsActivo, e => e.MapFrom(f => f.EsActivo == 1 ? true : false))
                .ForMember(d => d.IdRolNavigation, e => e.Ignore());

            CreateMap<Negocio, VMNegocio>()
                .ForMember(d=>d.PorcentajeImpuesto,e=>e.MapFrom(f=>Convert.ToString(f.PorcentajeImpuesto.Value,
                new CultureInfo("es-MX"))));
            CreateMap<VMNegocio, Negocio>()
                .ForMember(d=>d.PorcentajeImpuesto,e=>e.MapFrom(f=>Convert.ToDecimal(f.PorcentajeImpuesto,
                new CultureInfo("es-MX"))));

            CreateMap<Categoria, VMCategoria>()
                .ForMember(d => d.esActivo, e => e.MapFrom(f => f.EsActivo == true ? 1 : 0));
            CreateMap<VMCategoria, Categoria>()
                .ForMember(d => d.EsActivo, e => e.MapFrom(f => f.esActivo == 1 ? true : false));

            CreateMap<Producto, VMProducto>()
                .ForMember(d => d.EsActivo, e => e.MapFrom(f => f.EsActivo == true ? 1 : 0))
                .ForMember(d=>d.NombreCategoria,e=>e.MapFrom(f=>f.IdCategoriaNavigation.Descripcion))
                .ForMember(d=>d.Precio,e=>e.MapFrom(f=>Convert.ToString(f.Precio.Value, new CultureInfo("es-MX"))));
            CreateMap<VMProducto, Producto>()
                .ForMember(d => d.EsActivo, e => e.MapFrom(f => f.EsActivo == 1 ? true : false))
                .ForMember(d => d.IdCategoriaNavigation, e => e.Ignore())
                .ForMember(d => d.Precio, e => e.MapFrom(f => Convert.ToDecimal(f.Precio, new CultureInfo("es-MX"))));

            CreateMap<TipoDocumentoVenta, VMTipoDocumentoVenta>().ReverseMap();

            CreateMap<Venta, VMVenta>()
                .ForMember(d=>d.TipoDocumentoVenta,e=>e.MapFrom(f=>f.IdTipoDocumentoVentaNavigation.Descripcion))
                .ForMember(d=>d.Usuario,e=>e.MapFrom(f=>f.IdUsuarioNavigation.Nombre))
                .ForMember(d=>d.SubTotal,e=>e.MapFrom(f=>Convert.ToString(f.SubTotal.Value, new CultureInfo("es-MX"))))
                .ForMember(d=>d.ImpuestoTotal,e=>e.MapFrom(f=>Convert.ToString(f.ImpuestoTotal.Value, new CultureInfo("es-MX"))))
                .ForMember(d=>d.Total,e=>e.MapFrom(f=>Convert.ToString(f.Total.Value, new CultureInfo("es-MX"))))
                .ForMember(d=>d.FechaRegistro,e=>e.MapFrom(f=>f.FechaRegistro.Value.ToString("dd/MM/yyyy")));
            CreateMap<VMVenta, Venta>()
                .ForMember(d => d.SubTotal, e => e.MapFrom(f => Convert.ToDecimal(f.SubTotal, new CultureInfo("es-MX"))))
                .ForMember(d => d.ImpuestoTotal, e => e.MapFrom(f => Convert.ToDecimal(f.ImpuestoTotal, new CultureInfo("es-MX"))))
                .ForMember(d => d.Total, e => e.MapFrom(f => Convert.ToDecimal(f.Total, new CultureInfo("es-MX"))));

            CreateMap<DetalleVenta, VMDetalleVenta>()
                .ForMember(d=>d.Precio,e=>e.MapFrom(f=>Convert.ToString(f.Precio.Value, new CultureInfo("es-MX"))))
                .ForMember(d=>d.Total,e=>e.MapFrom(f=>Convert.ToString(f.Total.Value, new CultureInfo("es-MX"))));
            CreateMap<VMDetalleVenta, DetalleVenta>()
                .ForMember(d => d.Precio, e => e.MapFrom(f => Convert.ToDecimal(f.Precio, new CultureInfo("es-MX"))))
                .ForMember(d => d.Total, e => e.MapFrom(f => Convert.ToDecimal(f.Total, new CultureInfo("es-MX"))));
            CreateMap<DetalleVenta, VMReporteVenta>()
                .ForMember(d => d.FechaRegistro, e => e.MapFrom(f => f.IdVentaNavigation.FechaRegistro.Value.ToString("dd/MM/yyyy")))
                .ForMember(d => d.NumeroVenta, e => e.MapFrom(f => f.IdVentaNavigation.NumeroVenta))
                .ForMember(d => d.TipoDocumento, e => e.MapFrom(f => f.IdVentaNavigation.IdTipoDocumentoVentaNavigation.Descripcion))
                .ForMember(d => d.DocumentoCliente, e => e.MapFrom(f => f.IdVentaNavigation.DocumentoCliente))
                .ForMember(d => d.NombreCliente, e => e.MapFrom(f => f.IdVentaNavigation.NombreCliente))
                .ForMember(d => d.SubTotalVenta, e => e.MapFrom(f => Convert.ToString(f.IdVentaNavigation.SubTotal.Value, new CultureInfo("es-MX"))))
                .ForMember(d => d.ImpuestoTotalVenta, e => e.MapFrom(f => Convert.ToString(f.IdVentaNavigation.ImpuestoTotal.Value, new CultureInfo("es-MX"))))
                .ForMember(d => d.TotalVenta, e => e.MapFrom(f => Convert.ToString(f.IdVentaNavigation.Total.Value, new CultureInfo("es-MX"))))
                .ForMember(d => d.Producto, e => e.MapFrom(f => f.DescripcionProducto))
                .ForMember(d => d.Precio, e => e.MapFrom(f => Convert.ToString(f.Precio.Value,new CultureInfo("es-MX"))))
                .ForMember(d => d.Total, e => e.MapFrom(f => Convert.ToString(f.Total.Value,new CultureInfo("es-MX"))));

            CreateMap<Menu, VMMenu>()
                .ForMember(d => d.SubMenus, e => e.MapFrom(f => f.InverseIdMenuPadreNavigation));

        }
    }
}
