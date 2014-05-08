using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpDox.Build.Messenger
{
    public static class BuildEvents
    {
        public static BuildMessenger BuildMessenger { get; set; }
        public static SDMessengerStrings MessengerStrings { get; set; }

        public static void RaiseStartBuild()
        {
            BuildMessenger.ExecuteOnBuildMessage(MessengerStrings.StartingBuild);
            BuildMessenger.ExecuteOnBuildProgress(0);
        }

        public static void RaiseBuildSuccessful()
        {
            BuildMessenger.ExecuteOnBuildProgress(100);
            BuildMessenger.ExecuteOnStepMessage(string.Empty);
            BuildMessenger.ExecuteOnBuildMessage(MessengerStrings.BuildSuccess);
            BuildMessenger.ExecuteOnBuildCompleted();
        }
    }
}
