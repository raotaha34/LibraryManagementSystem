using System;
using System.Collections.Generic;

namespace Library_Management_System.Models;

public partial class RoleMaster
{
    public int Id { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<Member> Members { get; set; } = new List<Member>();
}
