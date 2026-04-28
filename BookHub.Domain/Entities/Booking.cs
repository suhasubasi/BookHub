using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookHub.Domain.Enums;

namespace BookHub.Domain.Entities;

public class Booking
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public Guid ServiceId { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public BookingStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public Company Company { get; set; } = null!;
    public Service Service { get; set; } = null!;
    public User Customer { get; set; } = null!;
    public Payment? Payment { get; set; }
}