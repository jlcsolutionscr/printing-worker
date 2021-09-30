using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Drawing;
using System.Drawing.Printing;

namespace JLCSolutionsCR
{
    public class DomainService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private int _charCount;
        private LineaImpresion[] _lineas;
        private const string APP_URL = "https://dev-webservices.jlcsolutionscr.com/facturacion-v2/PuntoventaWCF.svc";

        public DomainService(HttpClient httpClient) => _httpClient = httpClient;

        public async void ExecutePendingTickets(int companyId, int branchId, int charCount, string strFilter, string strPrinterName)
        {
            _charCount = charCount;
            try
            {
                PrintDocument pd = new PrintDocument();
                pd.PrinterSettings.PrinterName = strPrinterName;
                if (pd.PrinterSettings.IsValid) {
                    string strData = await _httpClient.GetFromJsonAsync<string>(APP_URL + "/obtenerlistadotiqueteordenserviciopendiente?idempresa=" + companyId + "&idsucursal=" + branchId, _options);
                    if (strData != "")
                    {
                        TicketType[] ticketList = JsonSerializer.Deserialize<TicketType[]>(strData);
                        foreach (TicketType ticket in ticketList)
                        {
                            _lineas = ticket.Lineas;
                            if (ticket.Impresora == strFilter)
                            {
                                Console.Out.WriteLine("Imprimiendo tiquete en impresora: " + strPrinterName);
                                PrintTicket(pd);
                                Console.Out.WriteLine("Cambiando el estado del tiquete"); 
                                var request = new HttpRequestMessage(HttpMethod.Get, APP_URL + "/cambiarestadoaimpresotiqueteordenservicio?idtiquete=" + ticket.IdTiquete);
                                await _httpClient.SendAsync(request);
                                Console.Out.WriteLine("Tiquete procesado satisfactoriamente"); 
                            }
                        }
                    }
                }
                else
                {
                    Console.Out.WriteLine("Printer " + strPrinterName + " not found!"); 
                }
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("Error processing the pending tickets: " + ex.Message + ex.StackTrace);
            }
        }

        private void PrintTicket(PrintDocument doc)
        {
            doc.PrintPage += new PrintPageEventHandler(this.ProvideContent);
            doc.Print();
        }

        private void ProvideContent(object sender, PrintPageEventArgs e)
        {
            float paperWith = (float)3.5375 * _charCount;
            Graphics graphics = e.Graphics;
            int positionY = 0;
            StringFormat sf = new StringFormat();
            int i = 0;
            while (i < _lineas.Length)
            {
                LineaImpresion linea  = _lineas[i];
                FontStyle fontStyle = linea.bolBold ? FontStyle.Bold : FontStyle.Regular;
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = (System.Drawing.StringAlignment)linea.intAlineado;
                RectangleF rec = new RectangleF();
                rec.Width = paperWith * linea.intAncho / 100;
                rec.Height = 20;
                rec.X = paperWith * linea.intPosicionX / 100;
                rec.Y = positionY;
                float fltFontSize = linea.intFuente;
                try
                {
                    fltFontSize = (float)Math.Round(linea.intFuente / (decimal)80 * _charCount, 0);
                }
                catch (Exception ex)
                {
                    Console.Out.WriteLine(ex.Message);
                }
                Font drawFont = new Font("drawFont", fltFontSize, fontStyle);
                graphics.DrawString(linea.strTexto, drawFont, new SolidBrush(Color.Black), rec, sf);
                positionY += (20 * linea.intSaltos);
                i++;
            }
        }
    }
}