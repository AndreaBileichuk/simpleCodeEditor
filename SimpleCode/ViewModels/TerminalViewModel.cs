using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Microsoft.Win32;
using SimpleCode.Commands;
using SimpleCode.Models;

namespace SimpleCode.ViewModels
{
    public class TerminalViewModel : ViewModelBase
    {
        private string _output;
        private string _inputCommand = "";
        private Process? _process;
        private StreamWriter _inputWriter;
        private string _currentDirectory;
        private readonly WorkSpaceViewModel _workSpaceViewModel;

        public string Output
        {
            get => _output;
            set
            {
                _output = value;
                OnPropertyChanged(nameof(Output));
            }
        }

        public string InputCommand
        {
            get => _inputCommand;
            set
            {
                _inputCommand = value;
                OnPropertyChanged(nameof(InputCommand));
            }
        }

        public ICommand ExecuteCommand { get; }

        public TerminalViewModel(EditorModel editorModel, WorkSpaceViewModel workSpaceViewModel)
        {
            _currentDirectory = editorModel.RootFolder.FullPath;
            _workSpaceViewModel = workSpaceViewModel;
            ExecuteCommand = new ExecuteCommand(Execute, this);
            StartTerminalProcess();
        }

        private async void StartTerminalProcess()
        {
            string keyName = @"SYSTEM\CurrentControlSet\Control\Session Manager\Environment\";
            string existingPathFolderVariable = (string)Registry.LocalMachine.OpenSubKey(keyName)
                .GetValue("PATH", "", RegistryValueOptions.DoNotExpandEnvironmentNames);

            _process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    StandardOutputEncoding = Encoding.UTF8,
                    WorkingDirectory = _currentDirectory,
                    EnvironmentVariables =
                    {
                        ["PATH"] = existingPathFolderVariable
                    }
                }
            };

            _process.OutputDataReceived += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                    Output += args.Data + "\n";
            };

            _process.ErrorDataReceived += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                    Output += "Error: " + args.Data + "\n";
            };

            _process.Start();
            _inputWriter = _process.StandardInput;
            _process.BeginOutputReadLine();
            _process.BeginErrorReadLine();

            await SetTimeout(2000, () => Output += GetPrompt());
        }

        async Task SetTimeout(int milliseconds, Action action)
        {
            await Task.Delay(milliseconds);
            action();
        }

        private string GetPrompt()
        {
            return $"\nPS {_currentDirectory}> ";
        }

        private async void Execute()
        {
            if (_process == null || _process.HasExited)
                StartTerminalProcess();

            if (!string.IsNullOrWhiteSpace(InputCommand))
            {
                string command = InputCommand;
                Output += command + "\n";
                _inputWriter.WriteLine(command);
                InputCommand = "";

                if (command.StartsWith("cd "))
                {
                    string newDirectory = command.Substring(3).Trim();
                    if (newDirectory == "..")
                    {
                        _currentDirectory = Directory.GetParent(_currentDirectory)?.FullName;
                    }
                    else
                    {
                        _currentDirectory = Path.Combine(_currentDirectory, newDirectory);
                    }

                    if (_process != null) _process.StartInfo.WorkingDirectory = _currentDirectory;
                }
                else if (command.StartsWith("gcc ") || command.StartsWith("g++ "))
                {
                    Output += $"Compiling with command: {command}\n";
                }

                await SetTimeout(1000, () => Output += GetPrompt());
            }
        }
    }
}