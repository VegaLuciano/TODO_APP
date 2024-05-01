using System;
using System.Collections.Generic;

namespace API_TODO.Models;

public partial class Typetask
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Task> Tasks { get; } = new List<Task>();
}
