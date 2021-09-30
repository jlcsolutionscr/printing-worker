namespace JLCSolutionsCR
{
    public class TicketType
    {
        public int IdTiquete { get; set; }
        public int IdEmpresa { get; set; }
        public int IdSucursal { get; set; }
        public string Descripcion { get; set; }
        public string Impresora { get; set; }
        public LineaImpresion[] Lineas { get; set; }
        public bool Impreso { get; set; }
    }

    public class LineaImpresion
    {
        public int intSaltos { get; set; }
        public string strTexto { get; set; }
        public int intPosicionX { get; set; }
        public int intAncho { get; set; }
        public int intFuente { get; set; }
        public int intAlineado { get; set; }
        public bool bolBold { get; set; }
    }
}