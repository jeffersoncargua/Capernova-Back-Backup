using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelectPdf;
using System.ComponentModel;
using System.Net;
using User.Managment.Data.Data;
using User.Managment.Data.Models;

namespace CapernovaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        protected ApiResponse _response;
        public StudentController(ApplicationDbContext db)
        {
            _db=db;
            this._response = new();
        }

        [HttpGet("getCertificate/{id:int}")]
        public async Task<ActionResult<ApiResponse>> GetCertificate(int id)
        {
            var course = await _db.CourseTbl.FirstOrDefaultAsync(u => u.Id ==id);
            if (course == null)
            {
                _response.isSuccess = false;
                _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _response.Message = "Ha ocurrido un problema al intentar descargar el certificado";
                return BadRequest(_response);
            }

            HtmlToPdf converter = new HtmlToPdf();

            //string htmlContent = @"<html>";
            //htmlContent += "<body>";
            string htmlContent = "<div style='background-image: url(\"https://i.postimg.cc/SK55dgbd/certificado-Capernova.jpg\"); max-width: 1024px;'>";
            htmlContent += "<div style='display: flex;flex-direction: column;justify-content: center; align-items: center;'>";
            htmlContent += "<h1 style='margin-top: 160px; margin-buttom : 0px ;padding: 0;font-size: 48px;font-weight: 600;font-family:'Lato';'>CERTIFICADO DE CAPACITACIÓN</h1>";
            htmlContent += "<h2 style='margin-top: 1px;margin-rigth: 0px;margin-left: 0px;margin-buttom: 0px;padding: 0px;font-size: 30px;font-weight: 400;'>POR APROBACIÓN</h2>";
            htmlContent += "<h3 style='margin-top: 1px;margin-rigth: 0px;margin-left: 0px;margin-buttom: 0px;padding: 0px;font-size: 20px;font-weight: 400'>ESTE CERTIFICADO SE OTORGA A:</h3>";
            htmlContent += "<h1 style='margin-top: 1px;margin-rigth: 0px;margin-left: 0px;margin-buttom: 0px; padding: 0px; font-size: 48px;font-weight: 500;'>César Almachi</h1>";
            htmlContent += $"<p style='margin-top: 1px;margin-rigth: 0px;margin-left: 0px;margin-buttom: 0px;padding: 0px;font-size: 18px;font-weight: 400;text-align: justify;max-width: 800px;line-height: 1.5;'>Por haber cursado todos los niveles de manera satisfactoria y con los más altos estándares de educación brindados por el Centro de Capacitación para Profesionales, Emprendedores e Innovación \"Capernova\", el curso de Experto en {course.Titulo} con Técnica Franceso con 120 horas de estudio.</p>";
            htmlContent += "<br><br><br><br><br><br><br><br>";
            htmlContent += "</div>";
            htmlContent += "</div>";
            //htmlContent += "</body>";
            //htmlContent += "</html>";


            //Margenes del documento
            converter.Options.MarginTop = 5;
            converter.Options.MarginLeft = 35;
            converter.Options.MarginTop = 5;
            converter.Options.MarginTop = 5;

            //doc.Margins.Top = 5;
            //doc.Margins.Right = 10;
            //doc.Margins.Bottom = 10;
            //doc.Margins.Right = 35;

            //ancho del documento
            converter.Options.WebPageHeight = 1024;

            //orientacion del documento
            converter.Options.PdfPageOrientation = PdfPageOrientation.Landscape;

            //convertir el documento html a pdf
            PdfDocument doc = converter.ConvertHtmlString(htmlContent);

            byte[] pdfFile = doc.Save();

            doc.Close();

            FileResult fileResult = new FileContentResult(pdfFile, "application/pdf");
            fileResult.FileDownloadName = "Certificado.pdf";

            _response.isSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.Message = "Se ha enviado el certificado";
            _response.Result = fileResult;
            return Ok(_response);

        }
    }
}
