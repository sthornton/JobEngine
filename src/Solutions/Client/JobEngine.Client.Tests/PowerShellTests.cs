using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;
using JobEngine.Models;

namespace JobEngine.Client.Tests
{
    [TestFixture]
    public class PowerShellTests
    {
        private PowerShellJob powerShellJobNoParams;
        private PowerShellJob powerShellJobWithParams;

        [TestFixtureSetUp]
        public void Setup()
        {
            this.powerShellJobNoParams = new PowerShellJob
            {
                Script = "get-process -ComputerName DEV-STHORNTON"
            };

            this.powerShellJobWithParams = new PowerShellJob
            {
                Script = "get-process -ComputerName $computer", PSResultType = Models.PSResultType.Table,
                Parameters = new List<PowerShellJobParameter>(){
                    new PowerShellJobParameter{ Value = "DEV-STHORNTON", Name = "computer", DataType = DataType.String }
                }
            };
        }

        [Test]
        public void ExecuteWithNoParams()
        {
            PowerShellJobExecutor executor = new PowerShellJobExecutor();
            executor.Execute(-1, this.powerShellJobNoParams);
        }

        [Test]
        public void ExecuteWithWithParams()
        {
            PowerShellJobExecutor executor = new PowerShellJobExecutor();
            var result = executor.Execute(-1, this.powerShellJobWithParams);

        }
    }
}
