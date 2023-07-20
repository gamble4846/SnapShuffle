using SnapShuffle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapShuffle.DataAccess.Interface
{
    public interface ITbScreenShotDataAccess
    {
        tbScreenShotModel GetByPrintScreenId(string PrintScreenId);
        string Add(tbScreenShotModel model);
    }
}
