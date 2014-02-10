using System;
using System.Collections.Generic;
using System.Text;

namespace GMP_Server.Scripting.Listener
{
    public interface ITextBoxListener
    {
        void OnMessageReceived(TextBox tb, Player pl, String message);
    }
}
