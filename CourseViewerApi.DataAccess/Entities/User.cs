using CourseViewerApi.Common.Enums;
using Microsoft.AspNetCore.Identity;

namespace CourseViewerApi.DataAccess.Entities
{
    public class User : IdentityUser
    {
        public UserType Type { get; set; }
        public string? Name { get; set; }
    }
}
