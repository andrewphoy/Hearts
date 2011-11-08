using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace HeartsInterfaces {
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
    public class HeartsPlayerAttribute : ExportAttribute {
        public HeartsPlayerAttribute(string playerName) : base(typeof(IHeartsPlayer)) {
            this.PlayerName = playerName;
        }
        public string PlayerName { get; set; }
    }
}
