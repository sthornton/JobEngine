using JobEngine.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JobEngine.Common;

namespace JobEngine.Client
{
    public class PowerShellJobExecutor
    {
        public PowerShellJobResult Execute(long jobExecutionQueueId, PowerShellJob powerShellJob)
        {
            PowerShellJobResult result = new PowerShellJobResult();
            result.Errors = new List<ErrorInfo>();
            using (Runspace runspace = RunspaceFactory.CreateRunspace())
            {
                //set the default runspace for this thread
                //otherwise you get exception: There is no Runspace available to run scripts in this thread.
                Runspace.DefaultRunspace = runspace;

                runspace.Open();
                using (Pipeline pipeline = runspace.CreatePipeline())
                {
                    pipeline.Commands.AddScript(powerShellJob.Script);

                    if(powerShellJob.PSResultType == PSResultType.String)
                        pipeline.Commands.Add("Out-String");

                    if (powerShellJob.Parameters != null && powerShellJob.Parameters.Count > 0)
                    {
                        foreach (var param in powerShellJob.Parameters)
                        {
                            switch (param.DataType)
                            {
                                case DataType.Int32:
                                    pipeline.Runspace.SessionStateProxy.SetVariable(param.Name, int.Parse(param.Value));
                                    break;
                                case DataType.Long:
                                    pipeline.Runspace.SessionStateProxy.SetVariable(param.Name, long.Parse(param.Value));
                                    break;
                                case DataType.String:
                                    pipeline.Runspace.SessionStateProxy.SetVariable(param.Name, param.Value);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    Collection<PSObject> PSOutput = pipeline.Invoke();

                    if (pipeline.Error.Count > 0)
                    {
                        var errors = pipeline.Error.Read() as Collection<ErrorRecord>;
                        foreach(var error in errors)
                        {
                            ErrorInfo errorInfo = new ErrorInfo();
                            errorInfo.Exception = error.Exception;
                            errorInfo.Message = error.ErrorDetails.Message;
                            errorInfo.RecommendedAction = error.ErrorDetails.RecommendedAction;
                            result.Errors.Add(errorInfo);
                        }
                    }

                    switch (powerShellJob.PSResultType)
                    {
                        case PSResultType.Table:
                            DataTable dataTableResults = GetDataTableResults(PSOutput);
                            result.Results = JsonConvert.SerializeObject(dataTableResults, new DataTableConverter());
                            break;
                        case PSResultType.String:
                            result.Results = GetStringResults(PSOutput);
                            break;
                        default:
                            break;
                    }                   
                }
            }
            return result;
        }

        private string GetStringResults(Collection<PSObject> PSOutput)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (PSObject obj in PSOutput)
            {
                stringBuilder.AppendLine(obj.ToString());
            }
            return stringBuilder.ToString();
        }

        private DataTable GetDataTableResults(Collection<PSObject> psOutput)
        {
            int i = 0;
            DataTable result = new DataTable();
            foreach (var item in psOutput)
            {
                try
                {
                    DataRow dr = result.NewRow();
                    foreach (var prop in item.Properties)
                    {
                        if (i == 0)
                        {
                            DataColumn newColumn = new DataColumn();
                            newColumn.ColumnName = prop.Name;
                            if (prop.Value != null)
                            {
                                if (System.Type.GetType(prop.TypeNameOfValue) != null)
                                {
                                    string g = prop.TypeNameOfValue;
                                    newColumn.DataType = System.Type.GetType(prop.TypeNameOfValue);
                                    result.Columns.Add(newColumn);
                                }
                            }
                        }
                        try
                        {
                            if (result.Columns[prop.Name] != null)
                                dr[prop.Name] = prop.Value.ToString();
                        }
                        catch { }
                    }
                    result.Rows.Add(dr);
                }
                catch { }
                i++;
            }
            return result;
        }
      
    }
}
