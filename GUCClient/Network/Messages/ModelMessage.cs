using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Scripting;
using GUC.Models;

namespace GUC.Client.Network.Messages
{
    static class ModelMessage
    {
        public static void ReadCreateMessage(PacketReader stream)
        {
            Model model = ScriptManager.Interface.CreateModel();
            model.ReadStream(stream);
            model.ScriptObject.Create();
        }

        public static void ReadDeleteMessage(PacketReader stream)
        {
            Model model;
            if (Model.TryGet(stream.ReadUShort(), out model))
            {
                model.ScriptObject.Delete();
            }
        }
    }
}
