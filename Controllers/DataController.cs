// using System;
// using System.Linq;
// using Microsoft.AspNetCore.Mvc;
// using Newtonsoft.Json;
// using System.IO;
// using HotelsManager.Models; // Замените YourNamespace.Models на пространство имен вашей модели

// namespace YourNamespace.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     public class DataController : ControllerBase
//     {
//         private readonly Hotel _context; // Замените YourDbContext на ваш контекст базы данных

//         public DataController(Hotel context)
//         {
//             _context = context;
//         }

//         [HttpGet]
//         public IActionResult GetAndExportToJson()
//         {
//             var data = _context.Name.ToList(); // Замените YourTable на вашу таблицу

//             // Преобразование данных в формат JSON
//             string jsonData = JsonConvert.SerializeObject(data);

//             // Путь к файлу .json
//             string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "../Assets/hotels_profile.json");

//             // Запись данных в файл .json
//             System.IO.File.WriteAllText(jsonFilePath, jsonData);

//             return Ok(data);
//         }
//     }
// }
