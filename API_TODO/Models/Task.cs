using System;
using System.Collections.Generic;

namespace API_TODO.Models;

public partial class Task
{
    public Guid Id { get; set; }

    public int? Userid { get; set; }

    public DateOnly? Deadline { get; set; }

    public int? Typeid { get; set; }

    public string? Description { get; set; }

    public DateTime? Dateregister { get; set; }

    public virtual Typetask? Type { get; set; }

    public virtual User? User { get; set; }
}
