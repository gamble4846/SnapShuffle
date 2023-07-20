using SnapShuffle.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapShuffle.Managers.Interface
{
    public interface IScreenshotManager
    {
        Task<APIResponse> GetRandom();
    }
}
