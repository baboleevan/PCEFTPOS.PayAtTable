﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PayAtTable.TestPos.ViewModels
{
    public enum LogType { INFO, WARNING, ERROR }

    public class LogData
    {
        public LogData(string rawData, string data, LogType type = LogType.INFO)
        {
            RawData = rawData;
            Data = data;
            Type = type;
        }

        string _rawData = string.Empty;
        public string RawData
        {
            get { return _rawData; }
            set
            {
                _rawData = value;

                try
                {
                    var temp = new POSAPIMsg();
                    temp.ParseFromString(value);

                    var prefix = temp.Header.ToString().Substring(0, 6) + Environment.NewLine;

                    var tempHeader = JsonConvert.DeserializeObject(temp.Header.ToString().Remove(0, 6));
                    var header = JsonConvert.SerializeObject(tempHeader, Formatting.Indented);

                    var tempContent = JsonConvert.DeserializeObject(temp.Content);
                    var content = JsonConvert.SerializeObject(tempContent, Formatting.Indented);

                    _rawData = prefix + header + Environment.NewLine + content;
                }
                catch (Exception)
                {
                    _rawData = value;
                }
            }
        }
        public string Data { get; set; }
        public LogType Type { get; set; }
    }

    public class ApiDataViewModel
    {
        public ObservableCollection<LogData> Logs { get; set; } = new ObservableCollection<LogData>();

        public LogData SelectedData { get; set; } = null;

        public Dictionary<string, string> TxnTypes { get; set; } = new Dictionary<string, string>();
        public Server.Data.SettingsOptions Options { get; set; } = new Server.Data.SettingsOptions();

        public ApiDataViewModel()
        {
            TxnTypes.Add("P", "Purchase Cash");
            TxnTypes.Add("R", "Refund");
        }

        public void Log(string message, LogType type = LogType.INFO)
        {
            var msg = $"{DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt")}: [{type}] {message}";
            var x = new LogData(message, msg, type);
            Logs.Add(x);
        }

        public void Log(LogData data)
        {
            data.Data = $"{DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt")}: [{data.Type}] {data.Data}";
            Logs.Add(data);
        }
    }
}
