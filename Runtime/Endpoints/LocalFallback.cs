using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Newtonsoft.Json.Linq;
using TinCan;
using UnityEngine;

namespace OmiLAXR.xAPI.Endpoints
{
    public class LocalFallback : MonoBehaviour
    {
        // [Tooltip("Enable if you want to log statements in Debug log.")]
        // public bool enableLogs = true;
        //
        // private string _filePath = "";
        // private string _unsentFilePath = "";
        // private string _unsentFolderPath = "";
        // private string _notAllowedFolderPath = "";
        //
        // private static string DefaultLocalStoragePath => Path.Combine(Path.GetTempPath(), "OmiLAXR.xAPI", "Statements");
        //
        // private readonly Queue<Statement> _queuedStatements = new Queue<Statement>();
        // private BackgroundWorker _worker;
        //
        // [Tooltip("Path to file, where statements will be saved locally. Keep empty to use the default folder.")]
        // public string localStoragePath = "";
        //
        // public int saveIterationMilliseconds = 500;
        //
        // private LearningRecordStore _lrs;
        //
        // private void Awake()
        // {
        //     _lrs = GetComponent<LearningRecordStore>();
        //     
        //     // initialize worker
        //     _worker = new BackgroundWorker();
        //     _worker.DoWork += SaveStatements;
        //     _worker.WorkerSupportsCancellation = true;
        //     
        //     // set local storage path
        //     if (string.Empty == localStoragePath)
        //         localStoragePath = DefaultLocalStoragePath;
        //     EnsurePath(localStoragePath);
        //     
        //     // define filename for local storage
        //     var now = DateTime.Now;
        //     var filename = $"{now.Year}{now.Month:00}{now.Day:00}{now.Hour:00}{now.Minute:00}{now.Second:00}";
        //     _filePath = Path.Combine(localStoragePath, filename + ".txt");
        //     
        //     // define filename of unsent statements buffer
        //     _unsentFolderPath = Path.Combine(localStoragePath, "unsent");
        //     EnsurePath(_unsentFolderPath);
        //     
        //     _unsentFilePath = Path.Combine(_unsentFolderPath, filename + ".json");
        //     
        //     if (!File.Exists(_unsentFilePath))
        //         File.Create(_unsentFilePath);
        //
        //     _notAllowedFolderPath = Path.Combine(localStoragePath, "failed");
        //     EnsurePath(_notAllowedFolderPath);
        //
        //     _lrs.onStartedSending += _ => SendUnsentStatements();
        //
        //     StartStoring();
        // }
        //
        // private void StartStoring() =>  _worker?.RunWorkerAsync();
        //
        // private void StopStoring() => _worker?.CancelAsync();
        //
        // private void OnDestroy()
        // {
        //     StopStoring();
        //     _worker?.Dispose();
        //     _worker = null;
        // }
        //
        // private void SaveStatements(object sender, DoWorkEventArgs e)
        // {
        //     while (!_worker.CancellationPending)
        //     {
        //         lock (_queuedStatements)
        //         {
        //             if (_queuedStatements.Count > 0)
        //             {
        //                 var statements = _queuedStatements.ToArray();
        //                 _queuedStatements.Clear();
        //                 SaveStatementsLocally(statements, _filePath, "[LocalFallback] Stored {0} statements in local storage.");    
        //             }
        //         }
        //         
        //         Thread.Sleep(saveIterationMilliseconds);
        //     }
        // }
        //
        // private void SaveStatementsLocally(IReadOnlyCollection<Statement> statements, string filePath, string message = null)
        // {
        //     var statementsString = StatementsToString(statements);
        //
        //     using (var streamWriter = new StreamWriter(filePath, append: true))
        //     {
        //         streamWriter.WriteLine("[" + statementsString + "]");   
        //     }
        //     
        //     if (enableLogs && message != null)
        //         Debug.Log(string.Format(message, statements.Count));
        // }
        //
        // private static string StatementsToString(IEnumerable<Statement> statements)
        // {
        //     var count = 0;
        //
        //     var sb = new StringBuilder();
        //     
        //     var jsons = statements.Select(s => s.ToJSON());
        //     foreach (var json in jsons)
        //     {
        //         if (count > 0)
        //             sb.Append(",");
        //         sb.Append(json);
        //         count++;
        //     }
        //
        //     return sb.ToString();
        // }
        //
        // /// <summary>
        // /// Create all parts of path if not exists.
        // /// </summary>
        // /// <param name="path"></param>
        // private static void EnsurePath(string path)
        // {
        //     if (Directory.Exists(path))
        //         return;
        //     
        //     path = path.Replace('\\', '/');
        //     var splitPath = path.Split('/');
        //     if (SystemInfo.operatingSystem.Contains("Win"))
        //         splitPath[0] += '\\';
        //     else
        //         splitPath[0] = "/";
        //     
        //     var checkedPath = "";
        //     // Go deep through whole path
        //     foreach (var t in splitPath)
        //     {
        //         checkedPath = t != string.Empty ? System.IO.Path.Combine(checkedPath, t) : checkedPath;
        //         Debug.Log(checkedPath);
        //         if (checkedPath == string.Empty)
        //             continue;
        //         // Create folder if not exists
        //         if (!Directory.Exists(checkedPath))
        //             Directory.CreateDirectory(checkedPath);
        //     }
        // }
        //
        // public void StoreStatement(Statement statement)
        // {
        //     lock (_queuedStatements)
        //     {
        //         _queuedStatements.Enqueue(statement);
        //     }
        // }
        //
        // private void StoreNotAllowedStatements(Statement[] statements, string filename)
        // {
        //     var notAllowedFile = Path.Combine(_notAllowedFolderPath, filename);
        //
        //     var stmts = StatementsToString(statements);
        //
        //     using (var writer = new StreamWriter(notAllowedFile, true))
        //     {
        //         writer.WriteLine(stmts);
        //     }
        //     
        //     Debug.LogWarning($"[LocalFallback] Caught {statements.Length} failed statements in '{notAllowedFile}'.");
        // }
        //
        // public void StoreUnsentStatements(IEnumerable<Statement> statements) 
        // => SaveStatementsLocally(statements.ToArray(), _unsentFilePath, "[LocalFallback] Caught {0} statements in local storage.");
        //
        // private void SendUnsentStatements()
        // {
        //     var files = Directory.GetFiles(_unsentFolderPath);
        //
        //     foreach (var file in files)
        //     {
        //         JArray statementsJArray;
        //         using (var streamReader = new StreamReader(file))
        //         {
        //             // skip if on end
        //             if (streamReader.EndOfStream)
        //                 continue;
        //             var text = streamReader.ReadToEnd();
        //             if (text.Trim() == string.Empty) // exit if empty file
        //                 continue;
        //             statementsJArray = JArray.Parse(text);
        //         }
        //
        //         if (statementsJArray.Count < 1)
        //             continue;
        //         
        //         var statements = statementsJArray.Children<JObject>().Select(obj => new Statement(obj)).ToArray();
        //
        //         var allowedStatements = statements.Where(s => s.actor.name != "").ToArray();
        //         var notAllowedStatements = statements.Where(s => s.actor.name == "").ToArray();
        //         
        //         var success = _lrs.SendStatements(allowedStatements);
        //
        //         if (success == TransferCode.Success)
        //         {
        //             Debug.Log("[LocalFallback] Sent " + allowedStatements.Length + " statements from local storage to LRS.");
        //             File.Delete(file);
        //
        //             if (notAllowedStatements.Length > 0)
        //             {
        //                 StoreNotAllowedStatements(notAllowedStatements, System.IO.Path.GetFileName(file));
        //             }
        //             
        //             return;
        //         }
        //         Debug.LogError("[LocalFallback] Cannot send " + statements.Length + $" statements from local storage to LRS from file '{file}'");
        //     }
        // }
    }
}