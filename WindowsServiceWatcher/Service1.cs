using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceWatcher
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                var largs = args.ToList();
                switch (largs.Count)
                {
                    case 1:
                        fileSystemWatcher1.EnableRaisingEvents = true;
                        fileSystemWatcher1.Path = largs[0];
                        fileSystemWatcher1.IncludeSubdirectories = false;
                        break;
                    case 2:
                        fileSystemWatcher1.EnableRaisingEvents = true;
                        fileSystemWatcher1.Path = largs[0];
                        fileSystemWatcher1.IncludeSubdirectories = true;
                        break;
                }
            }
            catch
            {
            }
        }

        protected override void OnStop()
        {
            try
            {
                fileSystemWatcher1.EnableRaisingEvents = false;
            }
            catch
            {
            }
        }

        private void fileSystemWatcher1_Changed(object sender, System.IO.FileSystemEventArgs e)
        {
            try
            {
                // Sending post to web api
                using (var httpClient = new HttpClient())
                {
                    var watchRecord = new WatchRecord();
                    watchRecord.Path = e.FullPath;

                    // get the file attributes for file or directory
                    FileAttributes attr = File.GetAttributes(e.FullPath);
                    if (attr.HasFlag(FileAttributes.Directory))
                    {
                        //MessageBox.Show("Its a directory");
                        watchRecord.IsFile = false;
                    }
                    else
                    {
                        //MessageBox.Show("Its a file");
                        watchRecord.IsFile = true;
                    }

                    watchRecord.Event = e.ChangeType.ToString();
                    watchRecord.Date = DateTime.Now;

                    var result = JsonConvert.SerializeObject(watchRecord);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    
                    var response = Task.Run(() => httpClient.PostAsync("http://vysoft.somee.com/api/WatchRecords", new StringContent(result, Encoding.UTF8, "application/json")));

                    //var responseString = response.Result.RequestMessage;
                    //Console.WriteLine(responseString);
                }
            }
            catch
            {
            }
        }

        private void fileSystemWatcher1_Renamed(object sender, System.IO.RenamedEventArgs e)
        {
            try
            {
                // Sending post to web api
                using (var httpClient = new HttpClient())
                {
                    var watchRecord = new WatchRecord();
                    watchRecord.Path = e.FullPath;
                    

                    // get the file attributes for file or directory
                    FileAttributes attr = File.GetAttributes(e.FullPath);
                    if (attr.HasFlag(FileAttributes.Directory))
                    {
                        //MessageBox.Show("Its a directory");
                        watchRecord.IsFile = false;
                    }
                    else
                    {
                        //MessageBox.Show("Its a file");
                        watchRecord.IsFile = true;
                    }

                    watchRecord.Event = e.ChangeType.ToString();
                    watchRecord.Date = DateTime.Now;

                    var result = JsonConvert.SerializeObject(watchRecord);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    
                    var response = Task.Run(() => httpClient.PostAsync("http://vysoft.somee.com/api/WatchRecords", new StringContent(result, Encoding.UTF8, "application/json")));

                    //var responseString = response.Result.RequestMessage;
                    //Console.WriteLine(responseString);
                }
            }
            catch
            {
            }
        }
    }
}
