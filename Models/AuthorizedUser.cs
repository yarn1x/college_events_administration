using System;
using System.Collections.Generic;

namespace college_events_admin_API.Models;

public partial class AuthorizedUser
{
    public int LoginId { get; set; }

    public string Login { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public int UsernameId { get; set; }

    public string? MobilePhone { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    public virtual ICollection<UserPhoto> UserPhotos { get; set; } = new List<UserPhoto>();

    public virtual ICollection<UserUsertype> UserUsertypes { get; set; } = new List<UserUsertype>();

    public virtual User Username { get; set; } = null!;
}
