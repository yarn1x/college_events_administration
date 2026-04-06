using System;
using System.Collections.Generic;

namespace college_events_admin_API.Models;

public partial class UserUsertype
{
    public int Id { get; set; }

    public int TypeId { get; set; }

    public int LoginId { get; set; }

    public virtual AuthorizedUser Login { get; set; } = null!;

    public virtual UserType Type { get; set; } = null!;
}
