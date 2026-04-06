using System;
using System.Collections.Generic;

namespace college_events_admin_API.Models;

public partial class ActualAttendance
{
    public int Id { get; set; }

    public int EventGroupId { get; set; }

    public int ActualListenersCount { get; set; }

    public int ActualParticipantsCount { get; set; }

    public int ActualSuperParticipantsCount { get; set; }

    public virtual EventGroup EventGroup { get; set; } = null!;
}
