using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log.NetNotifiers
{
    public interface INetNotifiication
    {
        Task SendNotification(string message);
    }
}
