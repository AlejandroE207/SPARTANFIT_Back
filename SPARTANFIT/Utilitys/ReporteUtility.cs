using iTextSharp.text.pdf;
using iTextSharp.text;
using SPARTANFIT.Dto;

namespace SPARTANFIT.Utilitys
{
    public class ReporteUtility : PdfPageEventHelper
    {

        public override void OnEndPage(PdfWriter writer, Document document)
        {

            PdfPTable footerTable = new PdfPTable(1);
            footerTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            footerTable.HorizontalAlignment = Element.ALIGN_CENTER;

            PdfPCell cell = new PdfPCell(new Phrase(" Reporte Generado por SPARTANFIT", FontFactory.GetFont("Helvetica", 10, Font.ITALIC)));
            cell.Border = Rectangle.NO_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            footerTable.AddCell(cell);

            footerTable.WriteSelectedRows(0, -1, document.LeftMargin, document.BottomMargin - 10, writer.DirectContent);
        }

        public void CrearPdfDeEntrenadores(List<PersonaDto> entrenadores, string filePath)
        {
            Document doc = new Document(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
            writer.PageEvent = new ReporteUtility(); // Asignar el evento del pie de página
            doc.Open();

            BaseColor goldColor = new BaseColor(255, 215, 0);
            BaseColor negro = new BaseColor(0, 0, 0);
            BaseColor grisClaro = new BaseColor(240, 240, 240); // Color alternado para filas

            // Título del reporte
            Font titleFont = FontFactory.GetFont("Helvetica", 18, Font.BOLD, negro);
            Paragraph title = new Paragraph("Lista de Entrenadores", titleFont)
            {
                Alignment = Element.ALIGN_CENTER
            };
            doc.Add(title);
            doc.Add(new Paragraph(" "));

            // Tabla
            PdfPTable table = new PdfPTable(4); // Cambiar a 4 columnas ya que eliminamos "ID"
            table.WidthPercentage = 100;
            float[] columnWidths = { 2f, 2f, 4f, 2f }; // Ajustar anchos de columnas
            table.SetWidths(columnWidths);

            // Encabezados de tabla
            Font headerFont = FontFactory.GetFont("Helvetica", 12, Font.BOLD, goldColor);
            table.AddCell(new PdfPCell(new Phrase("Nombres", headerFont)) { BackgroundColor = negro });
            table.AddCell(new PdfPCell(new Phrase("Apellidos", headerFont)) { BackgroundColor = negro });
            table.AddCell(new PdfPCell(new Phrase("Correo", headerFont)) { BackgroundColor = negro });
            table.AddCell(new PdfPCell(new Phrase("Género", headerFont)) { BackgroundColor = negro });

            // Contenido de la tabla con colores alternados
            Font contentFont = FontFactory.GetFont("Helvetica", 12);
            bool isGray = false;

            foreach (var entrenador in entrenadores)
            {
                BaseColor backgroundColor = isGray ? grisClaro : BaseColor.WHITE;

                table.AddCell(new PdfPCell(new Phrase(entrenador.nombres, contentFont)) { BackgroundColor = backgroundColor });
                table.AddCell(new PdfPCell(new Phrase(entrenador.apellidos, contentFont)) { BackgroundColor = backgroundColor });
                table.AddCell(new PdfPCell(new Phrase(entrenador.correo, contentFont)) { BackgroundColor = backgroundColor });
                table.AddCell(new PdfPCell(new Phrase(entrenador.genero, contentFont)) { BackgroundColor = backgroundColor });

                isGray = !isGray; // Alternar el color
            }

            doc.Add(table);
            doc.Close();
        }

        public void CrearPdfUsuarios(List<UsuarioDto> usuarios, string filePath)
        {
            Document doc = new Document(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
            writer.PageEvent = new ReporteUtility(); // Asignar el evento del pie de página
            doc.Open();

            BaseColor goldColor = new BaseColor(255, 215, 0);
            BaseColor negro = new BaseColor(0, 0, 0);
            BaseColor grisClaro = new BaseColor(240, 240, 240); // Color alternado para filas

            // Título del reporte
            Font titleFont = FontFactory.GetFont("Helvetica", 18, Font.BOLD, negro);
            Paragraph title = new Paragraph("Lista de Usuarios", titleFont)
            {
                Alignment = Element.ALIGN_CENTER
            };
            doc.Add(title);
            doc.Add(new Paragraph(" "));

            // Tabla
            PdfPTable table = new PdfPTable(4); // Cambiar a 4 columnas ya que eliminamos "ID"
            table.WidthPercentage = 100;
            float[] columnWidths = { 2f, 2f, 4f, 2f }; // Ajustar anchos de columnas
            table.SetWidths(columnWidths);

            // Encabezados de tabla
            Font headerFont = FontFactory.GetFont("Helvetica", 12, Font.BOLD, goldColor);
            table.AddCell(new PdfPCell(new Phrase("Nombres", headerFont)) { BackgroundColor = negro });
            table.AddCell(new PdfPCell(new Phrase("Apellidos", headerFont)) { BackgroundColor = negro });
            table.AddCell(new PdfPCell(new Phrase("Correo", headerFont)) { BackgroundColor = negro });
            table.AddCell(new PdfPCell(new Phrase("Género", headerFont)) { BackgroundColor = negro });


            Font contentFont = FontFactory.GetFont("Helvetica", 12);
            bool isGray = false;

            foreach (var usuario in usuarios)
            {
                BaseColor backgroundColor = isGray ? grisClaro : BaseColor.WHITE;

                table.AddCell(new PdfPCell(new Phrase(usuario.persona.nombres, contentFont)) { BackgroundColor = backgroundColor });
                table.AddCell(new PdfPCell(new Phrase(usuario.persona.apellidos, contentFont)) { BackgroundColor = backgroundColor });
                table.AddCell(new PdfPCell(new Phrase(usuario.persona.correo, contentFont)) { BackgroundColor = backgroundColor });
                table.AddCell(new PdfPCell(new Phrase(usuario.persona.genero, contentFont)) { BackgroundColor = backgroundColor });

                isGray = !isGray;
            }

            doc.Add(table);
            doc.Close();
        }
    }
}