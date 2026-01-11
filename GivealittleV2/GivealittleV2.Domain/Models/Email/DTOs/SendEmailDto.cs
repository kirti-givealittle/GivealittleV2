using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GivealittleV2.Domain.Models.Email.DTOs
{
    public sealed class SendEmailDto
    {
        public string ToEmail { get; set; } = null!;
        public string TemplateKey { get; set; } = null!;
        public string? SenderProfileEmail { get; set; } = "DEFAULT";
        public Dictionary<string, object?> Model { get; set; } = new();
    }
}
