using Microsoft.AspNetCore.Mvc;
using apiLoadFile.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace apiLoadFile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class loadFileController : ControllerBase
    {

        // GET api/<loadFileController>/5
        [HttpGet("{fileName}")]
        public IActionResult Get(string fileName)
        {
            // Ruta base donde se almacenan los archivos
            var folderPath = "Files";

            // Construir la ruta completa del archivo
            var fullPath = Path.Combine(folderPath, fileName);

            // Verificar si el archivo existe
            if (!System.IO.File.Exists(fullPath))
            {
                return NotFound($"El archivo '{fileName}' no fue encontrado.");
            }

            // Leer el archivo y devolverlo como respuesta
            var fileBytes = System.IO.File.ReadAllBytes(fullPath);
            var contentType = "application/octet-stream"; // Tipo de contenido genérico
            return File(fileBytes, contentType, fileName);
        }


        // POST api/<loadFileController>
        [HttpPost]
        public async Task<string> Post([FromForm] FileModel file)
        {
            var route = String.Empty;

            if (file.Archive.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.Archive.FileName);
                route = $"Files/{fileName}";
                using (var stream = new FileStream(route, FileMode.Create))
                { 
                    await file.Archive.CopyToAsync(stream);
                }

            }
            return route;

        }


    }
}
