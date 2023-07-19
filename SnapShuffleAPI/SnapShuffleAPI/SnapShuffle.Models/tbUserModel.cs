using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapShuffle.Models
{
    [Table("tbUser")]
    public class tbUserModel
    {
        public Guid UserGuid { get; set; } = Guid.NewGuid();
        public String UserName { get; set; }
        public String PasswordHash { get; set; }
        public String Email { get; set; }
    }

    public class tbUserRegisterModel
    {
        public String UserName { get; set; }
        public String Password { get; set; }
        public String Email { get; set; }
    }

    public class tbUserLoginModel
    {
        public String UserName { get; set; }
        public String Password { get; set; }
    }
}
