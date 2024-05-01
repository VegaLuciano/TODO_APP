using System;
using System.Collections.Generic;

namespace API_TODO.Models;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Task> Tasks { get; } = new List<Task>();
}
