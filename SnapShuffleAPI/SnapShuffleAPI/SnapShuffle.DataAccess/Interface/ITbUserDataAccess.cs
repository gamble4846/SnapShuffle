using SnapShuffle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapShuffle.DataAccess.Interface
{
    public interface ITbUserDataAccess
    {
        string Register(tbUserModel model);
        tbUserModel GetByEmail(string email);
        tbUserModel GetByUsername(string username);
    }
}
