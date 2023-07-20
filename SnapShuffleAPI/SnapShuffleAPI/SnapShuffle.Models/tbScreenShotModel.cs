using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapShuffle.Models
{
    [Table("tbScreenShot")]
    public class tbScreenShotModel
    {
        public Guid ScreenshotGUID { get; set; } = Guid.NewGuid();
        public String OldImageLink { get; set; }
        public String NewImgurLink { get; set; }
        public String AppName { get; set; }
    }
}
