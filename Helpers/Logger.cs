using System;
using System.IO;
using System.Windows.Forms;

namespace Enterprise.Helpers
{
    public static class Logger
    {
        private static string _logPath = Path.Combine(Application.StartupPath, "Logs");
        private static string _logFile = "";

        static Logger()
        {
            try
            {
                if (!Directory.Exists(_logPath))
                    Directory.CreateDirectory(_logPath);

                _logFile = Path.Combine(_logPath, $"log_{DateTime.Now:yyyy-MM-dd}.txt");

                // Escrever cabeçalho da sessão
                EscreverLog("INFO", $"=== SESSÃO INICIADA EM {DateTime.Now:dd/MM/yyyy HH:mm:ss} ===");
            }
            catch { }
        }

        public static void Info(string mensagem)
        {
            EscreverLog("INFO", mensagem);
        }

        public static void Debug(string mensagem)
        {
            EscreverLog("DEBUG", mensagem);
        }

        public static void Warning(string mensagem)
        {
            EscreverLog("WARN", mensagem);
        }

        public static void Error(string mensagem, Exception? ex = null)
        {
            string erro = mensagem;
            if (ex != null)
                erro += $" - {ex.Message}\nStackTrace: {ex.StackTrace}";
            EscreverLog("ERROR", erro);
        }

        private static void EscreverLog(string tipo, string mensagem)
        {
            try
            {
                string linha = $"{DateTime.Now:HH:mm:ss.fff} [{tipo}] {mensagem}";
                File.AppendAllText(_logFile, linha + Environment.NewLine);

                // Opcional: escrever também no console (útil para debugging)
                Console.WriteLine(linha);
            }
            catch { }
        }

        public static void LimparLogsAntigos(int diasManter = 30)
        {
            try
            {
                var arquivos = Directory.GetFiles(_logPath, "log_*.txt");
                var limite = DateTime.Now.AddDays(-diasManter);

                foreach (var arquivo in arquivos)
                {
                    if (File.GetCreationTime(arquivo) < limite)
                        File.Delete(arquivo);
                }
            }
            catch { }
        }
    }
}