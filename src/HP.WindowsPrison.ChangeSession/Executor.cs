﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.ServiceModel;
using System.Text;
using HP.WindowsPrison.ExecutorService;

namespace HP.WindowsPrison.ChangeSession
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ExecuteProcess" in both code and config file together.
    public class Executor : IExecutor
    {
        [PrincipalPermission(SecurityAction.Demand, Role = "BUILTIN\\Administrators")]
        public int ExecuteProcess(Prison prison, string filename, string arguments, string curDir, Dictionary<string, string> extraEnvironmentVariables, PipeStream stdinPipeName, PipeStream stdoutPipeName, PipeStream stderrPipeName)
        {
            // To debug the service uncomment the following line:
            // Debugger.Launch();

            prison.Reattach();
            var p = prison.InitializeProcess(filename, arguments, curDir, false, extraEnvironmentVariables, stdinPipeName, stdoutPipeName, stderrPipeName);

            return p.Id;
        }
    }
}
