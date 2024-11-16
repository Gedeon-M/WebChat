using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebChat.Entities;

namespace WebChat.Controllers
{
    [Route("api/messages")]
    [ApiController]
    //[Authorize(Roles = "Admin, User")]
    public class MessagesController : ControllerBase
    {
        private readonly ChatDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public MessagesController(ChatDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromForm] int senderId, [FromForm] int receiverId, [FromForm] string text, [FromForm] IFormFile? file)
        {
            try
            {
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                //if (userRole != "Admin" && userRole != "User")
                //{
                //    return Forbid();
                //}

                if (senderId == 0 || receiverId == 0 || string.IsNullOrEmpty(text))
                {
                    return BadRequest("Les paramètres senderId, receiverId ou text sont manquants ou invalides.");
                }

                var message = new Message
                {
                    SenderId = senderId,
                    ReceiverId = receiverId,
                    Text = text,
                    SentAt = DateTime.UtcNow,
                    IsRead = false,
                    FilePath = string.Empty
                };

                // If there's a file attached, save it
                //if (file != null)
                //{
                //    var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                //    if (!Directory.Exists(uploadsFolder))
                //    {
                //        Directory.CreateDirectory(uploadsFolder);
                //    }

                //    var filePath = Path.Combine(uploadsFolder, file.FileName);
                //    using (var fileStream = new FileStream(filePath, FileMode.Create))
                //    {
                //        await file.CopyToAsync(fileStream);
                //    }

                //    message.FilePath = $"/uploads/{file.FileName}";
                //}

                _context.Messages.Add(message);
                await _context.SaveChangesAsync();

                return Ok(message);
            }
            catch (Exception ex)
            {
                // Log and return detailed error information
                return StatusCode(500, $"Erreur interne : {ex.Message} - StackTrace: {ex.StackTrace}");
            }
        }

        [HttpGet("receive")]
        public async Task<IActionResult> ReceiveMessages([FromForm] int receiverId)
        {
            try
            {
                var userRole = await _context.Users
                    .Where(user => user.Id == receiverId)
                    .Select(user => user.Role.Name)
                    .FirstOrDefaultAsync();

                var messages = userRole == "User" ? await _context.Messages
                    .Where(message => message.ReceiverId == receiverId)
                    .ToListAsync() : await _context.Messages
                    .ToListAsync();

                return Ok(messages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"messages can't be loads with the id {receiverId}");
            }
        }
    }
}