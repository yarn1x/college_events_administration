using System;
using System.Collections.Generic;

namespace college_events_admin_API.Models;

public partial class UserPhoto
{
    public int UserPhotoId { get; set; }

    public int LoginId { get; set; }

    public string PhotoUrl { get; set; } = null!;

    public virtual AuthorizedUser Login { get; set; } = null!;
}
