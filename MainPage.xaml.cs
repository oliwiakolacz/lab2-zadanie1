using System;
using System.IO;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;

namespace lab2_zadanie1;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private void OnDivideClicked(object sender, EventArgs e)
	{
		try
		{
			double dividend, divisor;
			if (!double.TryParse(DividendEntry.Text, out dividend))
				throw new FormatException("Nieprawidłowa wartość dzielnej.");
			if (!double.TryParse(DivisorEntry.Text, out divisor))
				throw new FormatException("Nieprawidłowa wartość dzielnika.");
			if (divisor == 0)
				throw new DivideByZeroException("Dzielenie przez zero jest niedozwolone.");

			double result = dividend / divisor;
			ResultEntry.Text = result.ToString();
		}
		catch (Exception ex)
		{
			ResultEntry.Text = "Błąd";
			LogErrorToFile(ex);
			LogToMacOSSystemLog(ex);
			DisplayAlert("Błąd", ex.Message, "OK");
		}
	}

	private void LogErrorToFile(Exception ex)
	{
		string logFilePath = Path.Combine(FileSystem.AppDataDirectory, "division_app_log.txt");
		string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {ex.GetType().Name}: {ex.Message}{Environment.NewLine}{ex.StackTrace}{Environment.NewLine}";
		File.AppendAllText(logFilePath, logEntry);

	}
	
	static void LogToMacOSSystemLog(Exception ex)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "logger",
            Arguments = $"\"[CSharpApp] ERROR: {ex.Message}\"",
            RedirectStandardOutput = false,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        Process.Start(psi);
    }
}

