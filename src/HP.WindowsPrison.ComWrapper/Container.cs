﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HP.WindowsPrison.ComWrapper
{
    [ComVisible(true)]
    public interface IContainer
    {
        [ComVisible(true)]
        string Id { get; }

        [ComVisible(true)]
        string HomePath { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1406:AvoidInt64ArgumentsForVB6Clients", Justification = "Compatibility with VB6 is not required"), 
        ComVisible(true)]
        long MemoryLimitBytes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1406:AvoidInt64ArgumentsForVB6Clients", Justification = "Compatibility with VB6 is not required"), 
        ComVisible(true)]
        long DiskLimitBytes { get; set; }

        [ComVisible(true)]
        int NetworkPort { get; set; }

        [ComVisible(true)]
        bool IsLockedDown();

        [ComVisible(true)]
        void Lockdown();

        [ComVisible(true)]
        IProcessTracker Run(IContainerRunInfo runInfo);

        [ComVisible(true)]
        void Stop();

        [ComVisible(true)]
        void Destroy();
    }

    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class Container : IContainer
    {

        // private  a;
        private Prison prison;


        public string Id { get; private set; }

        public string HomePath { get; set; }

        public long MemoryLimitBytes { get; set; }

        public long DiskLimitBytes { get; set; }

        public int NetworkPort { get; set; }

        public bool IsLockedDown()
        {
            return prison.IsLocked();
        }

        public Container()
        {
            this.prison = new Prison();
            this.prison.Tag = "uward";
            this.Id = prison.ID.ToString();
        }

        public Container(Prison p)
        {
            this.prison = p;
            this.Id = p.ID.ToString();
        }

        public void Lockdown()
        {
            PrisonRules prisonRules = new PrisonRules();
            prisonRules.CellType = RuleType.None;
            prisonRules.PrisonHomePath = this.HomePath;
            prisonRules.CellType |= RuleType.WindowStation;
            if (this.MemoryLimitBytes > 0)
            {
                prisonRules.CellType |= RuleType.Memory;
                prisonRules.TotalPrivateMemoryLimitBytes = this.MemoryLimitBytes;
            }

            if (this.DiskLimitBytes > 0)
            {
                prisonRules.CellType |= RuleType.Disk;
                prisonRules.DiskQuotaBytes = this.DiskLimitBytes;
            }
            if (this.NetworkPort > 0)
            {
                prisonRules.CellType |= RuleType.Httpsys;
                prisonRules.UrlPortAccess = this.NetworkPort;
            }

            prison.Lockdown(prisonRules);
        }

        public IProcessTracker Run(IContainerRunInfo runInfo)
        {
            var process = this.prison.Execute(runInfo.Filename, runInfo.Arguments, runInfo.CurrentDirectory, false, runInfo.ExtraEnvironmentVariables, runInfo.StdinPipe, runInfo.StdoutPipe, runInfo.StderrPipe);

            if (runInfo.StdinPipe != null) runInfo.StdinPipe.Dispose();
            if (runInfo.StdoutPipe != null) runInfo.StdoutPipe.Dispose();
            if (runInfo.StderrPipe != null) runInfo.StderrPipe.Dispose();

            return new ProcessTracker(process);
        }

        public void Stop()
        {
            this.prison.JobObject.TerminateProcesses(-1);
        }

        public void Destroy()
        {
            this.prison.Destroy();
        }
    }
}
