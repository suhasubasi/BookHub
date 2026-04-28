using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookHub.Domain.Entities;

public class Document
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string BlobUrl { get; set; } = string.Empty;
    public bool IsProcessed { get; set; }
    public DateTime UploadedAt { get; set; }
    public Company Company { get; set; } = null!;
}
