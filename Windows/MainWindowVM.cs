using MouseMover.Util;
using System.Windows.Input;

namespace MouseMover.Windows;

public class MainWindowVM
{
    public MainWindowVM()
    {
        CreateCommands();
    }

    public bool TimeBetweenChangesChecked { get; set; }

    public int? TimeBetweenChangesSeconds { get; set; }

    public bool ShutdownPCAfter { get; set; }

    public int? ShutdownPCAfterMinutes { get; set; }

    public bool SimulateKeyPress { get; set; }

    public string Timer { get; set; }

    public ICommand Enable { get; private set; }

    private void CreateCommands()
    {
        this.Enable = new RelayCommand(
            obj =>
            {
                Start();
            });
    }

    private void Start()
    {
        CoreApplication.Start(this.TimeBetweenChangesSeconds, this.SimulateKeyPress, this.ShutdownPCAfter, this.ShutdownPCAfterMinutes);
    }
}
