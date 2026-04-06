using System;
using System.Collections.Generic;

namespace college_events_admin_API.Models;

public partial class Group
{
    public int GroupId { get; set; }

    public string Name { get; set; } = null!;

    public int LoginId { get; set; }

    public virtual ICollection<EventGroup> EventGroups { get; set; } = new List<EventGroup>();

    public virtual AuthorizedUser Login { get; set; } = null!;
}
