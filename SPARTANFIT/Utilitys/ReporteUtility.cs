

using iTextSharp.text.pdf;
using iTextSharp.text;
using SPARTANFIT.Dto;

namespace SPARTANFIT.Utilitys
{
 
    public class ReporteUtility
    {
        public void CrearPdfDeEntrenadores(List<PersonaDto> entrenadores, string filePath)
        {
            Document doc = new Document(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));

            doc.Open();


            BaseColor goldColor = new BaseColor(255, 215, 0);


            BaseColor negro = new BaseColor(0, 0, 0);


            Font titleFont = FontFactory.GetFont("Helvetica", 18, Font.BOLD, negro);


            Paragraph title = new Paragraph("Lista de Entrenadores", titleFont)
            {
                Alignment = Element.ALIGN_CENTER
            };
            doc.Add(title);

            doc.Add(new Paragraph(" "));


            PdfPTable table = new PdfPTable(5);


            var headerFont = FontFactory.GetFont("Helvetica", 12, Font.BOLD, goldColor);
            table.AddCell(new PdfPCell(new Phrase("ID", headerFont)) { BackgroundColor = negro });
            table.AddCell(new PdfPCell(new Phrase("Nombres", headerFont)) { BackgroundColor = negro });
            table.AddCell(new PdfPCell(new Phrase("Apellidos", headerFont)) { BackgroundColor = negro });
            table.AddCell(new PdfPCell(new Phrase("Correo", headerFont)) { BackgroundColor = negro });
            table.AddCell(new PdfPCell(new Phrase("Género", headerFont)) { BackgroundColor = negro });



            foreach (var entrenador in entrenadores)
            {
                table.AddCell(entrenador.id_usuario.ToString());
                table.AddCell(entrenador.nombres);
                table.AddCell(entrenador.apellidos);
                table.AddCell(entrenador.correo);
                table.AddCell(entrenador.genero);

            }

            doc.Add(table);

            doc.Close();
        }
        public void CrearPdfUsuarios(List<UsuarioDto> usuarios, string filePath)
        {
            Document doc = new Document(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));

            doc.Open();


            BaseColor goldColor = new BaseColor(255, 215, 0);

            Font titleFont = FontFactory.GetFont("Helvetica", 18, Font.BOLD, BaseColor.BLACK);


            Paragraph title = new Paragraph("Lista de Usuarios", titleFont)
            {
                Alignment = Element.ALIGN_CENTER
            };
            doc.Add(title);

            doc.Add(new Paragraph(" "));


            BaseColor negro = new BaseColor(0, 0, 0);

            PdfPTable table = new PdfPTable(5);

            Font headerFont = FontFactory.GetFont("Helvetica", 12, Font.BOLD, goldColor);


            table.AddCell(new PdfPCell(new Phrase("Id", headerFont)) { BackgroundColor = negro });
            table.AddCell(new PdfPCell(new Phrase("Nombres", headerFont)) { BackgroundColor = negro });
            table.AddCell(new PdfPCell(new Phrase("Apellidos", headerFont)) { BackgroundColor = negro });
            table.AddCell(new PdfPCell(new Phrase("Correo", headerFont)) { BackgroundColor = negro });
            table.AddCell(new PdfPCell(new Phrase("Género", headerFont)) { BackgroundColor = negro });



            Font contentFont = FontFactory.GetFont("Helvetica", 12);


            foreach (var usuario in usuarios)
            {
                table.AddCell(new PdfPCell(new Phrase(usuario.persona.id_usuario.ToString(), contentFont)));
                table.AddCell(new PdfPCell(new Phrase(usuario.persona.nombres, contentFont)));
                table.AddCell(new PdfPCell(new Phrase(usuario.persona.apellidos, contentFont)));
                table.AddCell(new PdfPCell(new Phrase(usuario.persona.correo, contentFont)));
                table.AddCell(new PdfPCell(new Phrase(usuario.persona.genero, contentFont)));

            }

            doc.Add(table);

            doc.Close();
        }
    }
}

