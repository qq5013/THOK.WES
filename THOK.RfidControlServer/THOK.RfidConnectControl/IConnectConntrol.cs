using System;
using System.Collections.Generic;

using System.Text;

namespace THOK.RfidConnectControl
{
    public enum StateType
    {
        ReadErr,
        Reading,
        ReadRuning,
        ReadClose
    }

    public interface IConnectConntrol
    {
        void SetSize();
        void Start();
        void Stop();
        void SetRunState(StateType stateType);
    }
}
