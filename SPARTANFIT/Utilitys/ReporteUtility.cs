using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using System.Collections.Generic;
using System;
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

            PdfPCell cell = new PdfPCell(new Phrase("Reporte Generado por SPARTANFIT", FontFactory.GetFont("Helvetica", 10, Font.ITALIC)));
            cell.Border = Rectangle.NO_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            footerTable.AddCell(cell);

            footerTable.WriteSelectedRows(0, -1, document.LeftMargin, document.BottomMargin - 10, writer.DirectContent);
        }

        public void CrearPdfDeEntrenadores(List<PersonaDto> entrenadores, string filePath)
        {
            Document doc = new Document(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
            writer.PageEvent = new ReporteUtility();
            doc.Open();

            BaseColor goldColor = new BaseColor(255, 215, 0);
            BaseColor negro = new BaseColor(0, 0, 0);
            BaseColor grisClaro = new BaseColor(240, 240, 240);

            string imageUrl = "https://cdn-icons-png.flaticon.com/256/5114/5114256.png";
            Image gymImage = Image.GetInstance(new Uri(imageUrl));
            gymImage.ScaleToFit(100f, 100f);
            gymImage.Alignment = Element.ALIGN_LEFT;
            doc.Add(gymImage);


            Font titleFont = FontFactory.GetFont("Helvetica", 18, Font.BOLD, negro);
            Paragraph title = new Paragraph("Lista de Entrenadores", titleFont)
            {
                Alignment = Element.ALIGN_CENTER
            };
            doc.Add(title);

            // Agregar la fecha actual
            Font dateFont = FontFactory.GetFont("Helvetica", 12, Font.ITALIC, negro);
            Paragraph date = new Paragraph("Fecha: " + DateTime.Now.ToString("dd/MM/yyyy"), dateFont)
            {
                Alignment = Element.ALIGN_RIGHT
            };
            doc.Add(date);

            doc.Add(new Paragraph(" "));


            PdfPTable table = new PdfPTable(4);
            table.WidthPercentage = 100;
            float[] columnWidths = { 2f, 2f, 4f, 2f };
            table.SetWidths(columnWidths);


            Font headerFont = FontFactory.GetFont("Helvetica", 12, Font.BOLD, goldColor);
            table.AddCell(new PdfPCell(new Phrase("Nombres", headerFont)) { BackgroundColor = negro });
            table.AddCell(new PdfPCell(new Phrase("Apellidos", headerFont)) { BackgroundColor = negro });
            table.AddCell(new PdfPCell(new Phrase("Correo", headerFont)) { BackgroundColor = negro });
            table.AddCell(new PdfPCell(new Phrase("Género", headerFont)) { BackgroundColor = negro });


            Font contentFont = FontFactory.GetFont("Helvetica", 12);
            bool isGray = false;

            foreach (var entrenador in entrenadores)
            {
                BaseColor backgroundColor = isGray ? grisClaro : BaseColor.WHITE;

                table.AddCell(new PdfPCell(new Phrase(entrenador.nombres, contentFont)) { BackgroundColor = backgroundColor });
                table.AddCell(new PdfPCell(new Phrase(entrenador.apellidos, contentFont)) { BackgroundColor = backgroundColor });
                table.AddCell(new PdfPCell(new Phrase(entrenador.correo, contentFont)) { BackgroundColor = backgroundColor });
                table.AddCell(new PdfPCell(new Phrase(entrenador.genero, contentFont)) { BackgroundColor = backgroundColor });

                isGray = !isGray;
            }

            doc.Add(table);
            doc.Close();
        }

        public void CrearPdfUsuarios(List<UsuarioDto> usuarios, string filePath)
        {
            Document doc = new Document(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
            writer.PageEvent = new ReporteUtility();
            doc.Open();

            BaseColor goldColor = new BaseColor(255, 215, 0);
            BaseColor negro = new BaseColor(0, 0, 0);
            BaseColor grisClaro = new BaseColor(240, 240, 240);


            string imageUrl = "https://cdn-icons-png.flaticon.com/256/5114/5114256.png";
            Image gymImage = Image.GetInstance(new Uri(imageUrl));
            gymImage.ScaleToFit(100f, 100f);
            gymImage.Alignment = Element.ALIGN_LEFT;
            doc.Add(gymImage);


            Font titleFont = FontFactory.GetFont("Helvetica", 18, Font.BOLD, negro);
            Paragraph title = new Paragraph("Lista de Usuarios", titleFont)
            {
                Alignment = Element.ALIGN_CENTER
            };
            doc.Add(title);

            Font dateFont = FontFactory.GetFont("Helvetica", 12, Font.ITALIC, negro);
            Paragraph date = new Paragraph("Fecha: " + DateTime.Now.ToString("dd/MM/yyyy"), dateFont)
            {
                Alignment = Element.ALIGN_RIGHT
            };
            doc.Add(date);

            doc.Add(new Paragraph(" "));


            PdfPTable table = new PdfPTable(4);
            table.WidthPercentage = 100;
            float[] columnWidths = { 2f, 2f, 4f, 2f };
            table.SetWidths(columnWidths);


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