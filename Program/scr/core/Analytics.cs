using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program.scr.core
{
    // Класс для хранения результата анализа по одному клиенту
    public class ClientStats
    {
        public string ClientName { get; set; }
        public int OrderCount { get; set; }
        public int ServiceCount { get; set; }
        public decimal TotalSpent { get; set; }
    }

    // Класс для хранения результата анализа по одному сотруднику
    public class EmployeeStats
    {
        public string EmployeeName { get; set; }
        public int OrdersProcessed { get; set; }
        public int ServicesProvided { get; set; }
        public decimal RevenueGenerated { get; set; }
    }

    // Класс для хранения общих статистик
    public class SummaryStats
    {
        public int TotalOrders { get; set; }
        public int TotalServices { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalProfit { get; set; }
        public List<ClientStats> TopClients { get; set; } = new List<ClientStats>();
        public List<EmployeeStats> TopEmployees { get; set; } = new List<EmployeeStats>();
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
    }

    public class DataService
    {
        private readonly string _connectionString;

        public DataService()
        {
            _connectionString = SQL._sqlConnectStr; // Предполагается, что SQL._sqlConnectStr определён
        }

        /// <summary>
        /// Генерирует аналитический отчёт за указанный период и сохраняет его в HTML-файл.
        /// </summary>
        /// <param name="startDate">Начало периода (включительно).</param>
        /// <param name="endDate">Конец периода (включительно).</param>
        /// <param name="outputFilePath">Путь к файлу для сохранения HTML-отчёта.</param>
        public void GenerateAnalyticsReport(DateTime startDate, DateTime endDate, string outputFilePath)
        {
            var stats = GetAnalyticsData(startDate, endDate);

            string htmlReport = BuildHtmlReport(stats);

            File.WriteAllText(outputFilePath, htmlReport, Encoding.UTF8);

            Console.WriteLine($"Отчёт успешно сгенерирован и сохранён в {outputFilePath}");
        }

        /// <summary>
        /// Выполняет SQL-запросы для извлечения и анализа данных.
        /// </summary>
        /// <param name="startDate">Начало периода.</param>
        /// <param name="endDate">Конец периода.</param>
        /// <returns>Объект SummaryStats с рассчитанными показателями.</returns>
        private SummaryStats GetAnalyticsData(DateTime startDate, DateTime endDate)
        {
            var stats = new SummaryStats
            {
                PeriodStart = startDate,
                PeriodEnd = endDate
            };

            // Используем три отдельных запроса для получения общей статистики, статистики по клиентам и по сотрудникам
            // Это избегает сложностей с объединением и дублированием данных.

            // --- 1. Общая статистика ---
            string summaryQuery = @"
                SELECT
                    (SELECT COUNT(*) FROM Orders WHERE OrderDateTime >= @StartDate AND OrderDateTime <= @EndDate) AS TotalOrders,
                    (SELECT COUNT(*) FROM ProvidedServices WHERE ServiceDateTime >= @StartDate AND ServiceDateTime <= @EndDate) AS TotalServices,
                    (SELECT ISNULL(SUM(TotalAmount), 0) FROM Orders WHERE OrderDateTime >= @StartDate AND OrderDateTime <= @EndDate) +
                    (SELECT ISNULL(SUM(s.Cost), 0) FROM ProvidedServices ps JOIN Services s ON ps.ServiceID = s.ID WHERE ps.ServiceDateTime >= @StartDate AND ps.ServiceDateTime <= @EndDate) AS TotalRevenue;
            ";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(summaryQuery, connection))
                {
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", endDate);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            stats.TotalOrders = reader["TotalOrders"] == DBNull.Value ? 0 : Convert.ToInt32(reader["TotalOrders"]);
                            stats.TotalServices = reader["TotalServices"] == DBNull.Value ? 0 : Convert.ToInt32(reader["TotalServices"]);
                            stats.TotalRevenue = reader["TotalRevenue"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["TotalRevenue"]);
                            // Пока не реализуем расчёт прибыли, используем выручку как placeholder
                            stats.TotalProfit = stats.TotalRevenue * 0.1m; // Пример: 10% маржа
                        }
                    }
                }

                // --- 2. Статистика по клиентам ---
                string clientStatsQuery = @"
                    WITH ClientOrderTotals AS (
                        SELECT
                            o.ClientID,
                            COUNT(o.ID) AS OrderCount,
                            SUM(o.TotalAmount) AS TotalOrderValue
                        FROM Orders o
                        WHERE o.OrderDateTime >= @StartDate AND o.OrderDateTime <= @EndDate
                        GROUP BY o.ClientID
                    ),
                    ClientServiceTotals AS (
                        SELECT
                            ps.ClientID,
                            COUNT(ps.ID) AS ServiceCount,
                            SUM(s.Cost) AS TotalServiceValue
                        FROM ProvidedServices ps
                        JOIN Services s ON ps.ServiceID = s.ID
                        WHERE ps.ServiceDateTime >= @StartDate AND ps.ServiceDateTime <= @EndDate
                        GROUP BY ps.ClientID
                    ),
                    CombinedClientStats AS (
                        SELECT
                            c.ID,
                            c.FullName,
                            ISNULL(cot.OrderCount, 0) AS OrderCount,
                            ISNULL(cst.ServiceCount, 0) AS ServiceCount,
                            ISNULL(cot.TotalOrderValue, 0) + ISNULL(cst.TotalServiceValue, 0) AS TotalSpent
                        FROM Clients c
                        LEFT JOIN ClientOrderTotals cot ON c.ID = cot.ClientID
                        LEFT JOIN ClientServiceTotals cst ON c.ID = cst.ClientID
                    )
                    SELECT
                        FullName AS ClientName,
                        OrderCount,
                        ServiceCount,
                        TotalSpent
                    FROM CombinedClientStats
                    ORDER BY TotalSpent DESC;";

                using (var command = new SqlCommand(clientStatsQuery, connection))
                {
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", endDate);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if(Convert.ToDecimal(reader["TotalSpent"]) == 0) continue;
                            stats.TopClients.Add(new ClientStats
                            {
                                ClientName = reader["ClientName"].ToString(),
                                OrderCount = Convert.ToInt32(reader["OrderCount"]),
                                ServiceCount = Convert.ToInt32(reader["ServiceCount"]),
                                TotalSpent = Convert.ToDecimal(reader["TotalSpent"])
                            });
                        }
                    }
                }

                // --- 3. Статистика по сотрудникам ---
                string employeeStatsQuery = @"
                    WITH EmployeeOrderTotals AS (
                        SELECT
                            o.EmployeeID,
                            COUNT(o.ID) AS OrdersProcessed,
                            SUM(o.TotalAmount) AS RevenueFromOrders
                        FROM Orders o
                        WHERE o.OrderDateTime >= @StartDate AND o.OrderDateTime <= @EndDate
                        GROUP BY o.EmployeeID
                    ),
                    EmployeeServiceTotals AS (
                        SELECT
                            ps.EmployeeID,
                            COUNT(ps.ID) AS ServicesProvided,
                            SUM(s.Cost) AS RevenueFromServices
                        FROM ProvidedServices ps
                        JOIN Services s ON ps.ServiceID = s.ID
                        WHERE ps.ServiceDateTime >= @StartDate AND ps.ServiceDateTime <= @EndDate
                        GROUP BY ps.EmployeeID
                    ),
                    CombinedEmployeeStats AS (
                        SELECT
                            e.ID,
                            e.FullName,
                            ISNULL(eot.OrdersProcessed, 0) AS OrdersProcessed,
                            ISNULL(est.ServicesProvided, 0) AS ServicesProvided,
                            ISNULL(eot.RevenueFromOrders, 0) + ISNULL(est.RevenueFromServices, 0) AS RevenueGenerated
                        FROM Employees e
                        LEFT JOIN EmployeeOrderTotals eot ON e.ID = eot.EmployeeID
                        LEFT JOIN EmployeeServiceTotals est ON e.ID = est.EmployeeID
                    )
                    SELECT
                        FullName AS EmployeeName,
                        OrdersProcessed,
                        ServicesProvided,
                        RevenueGenerated
                    FROM CombinedEmployeeStats
                    ORDER BY RevenueGenerated DESC;";

                using (var command = new SqlCommand(employeeStatsQuery, connection))
                {
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", endDate);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            stats.TopEmployees.Add(new EmployeeStats
                            {
                                EmployeeName = reader["EmployeeName"].ToString(),
                                OrdersProcessed = Convert.ToInt32(reader["OrdersProcessed"]),
                                ServicesProvided = Convert.ToInt32(reader["ServicesProvided"]),
                                RevenueGenerated = Convert.ToDecimal(reader["RevenueGenerated"])
                            });
                        }
                    }
                }
            }

            return stats;
        }

        /// <summary>
        /// Формирует HTML-страницу на основе данных из SummaryStats.
        /// </summary>
        /// <param name="stats">Объект с данными для отчёта.</param>
        /// <returns>Строка с HTML-кодом.</returns>
        private string BuildHtmlReport(SummaryStats stats)
        {
            var html = new StringBuilder();
            html.AppendLine("<!DOCTYPE html>");
            html.AppendLine("<html lang='ru'>");
            html.AppendLine("<head>");
            html.AppendLine("    <meta charset='UTF-8'>");
            html.AppendLine("    <meta name='viewport' content='width=device-width, initial-scale=1.0'>");
            html.AppendLine("    <title>Аналитический отчёт - ООО 'Руссоль'</title>");
            html.AppendLine("    <style>");
            html.AppendLine("        body { font-family: Arial, sans-serif; margin: 20px; }");
            html.AppendLine("        h1, h2 { color: #333; }");
            html.AppendLine("        table { border-collapse: collapse; width: 100%; margin-bottom: 20px; }");
            html.AppendLine("        th, td { border: 1px solid #ddd; padding: 8px; text-align: left; }");
            html.AppendLine("        th { background-color: #f2f2f2; }");
            html.AppendLine("        .summary-box { background-color: #f9f9f9; padding: 10px; border-radius: 5px; margin-bottom: 20px; }");
            html.AppendLine("    </style>");
            html.AppendLine("</head>");
            html.AppendLine("<body>");

            html.AppendLine($"<h1>Аналитический отчёт за период с {stats.PeriodStart:yyyy-MM-dd} по {stats.PeriodEnd:yyyy-MM-dd}</h1>");

            html.AppendLine("<div class='summary-box'>");
            html.AppendLine($"<h2>Сводка</h2>");
            html.AppendLine($"<p><strong>Всего заказов:</strong> {stats.TotalOrders}</p>");
            html.AppendLine($"<p><strong>Всего услуг оказано:</strong> {stats.TotalServices}</p>");
            html.AppendLine($"<p><strong>Общая выручка:</strong> {stats.TotalRevenue:C}</p>");
            html.AppendLine("</div>");

            if (stats.TopClients.Count > 0)
            {
                html.AppendLine("<h2>Топ клиентов по сумме заказов и услуг</h2>");
                html.AppendLine("<table>");
                html.AppendLine("    <thead>");
                html.AppendLine("        <tr><th>Клиент</th><th>Заказов</th><th>Услуг</th><th>Потрачено</th></tr>");
                html.AppendLine("    </thead>");
                html.AppendLine("    <tbody>");
                foreach (var client in stats.TopClients)
                {
                    html.AppendLine($"        <tr><td>{client.ClientName}</td><td>{client.OrderCount}</td><td>{client.ServiceCount}</td><td>{client.TotalSpent:C}</td></tr>");
                }
                html.AppendLine("    </tbody>");
                html.AppendLine("</table>");
            }

            if (stats.TopEmployees.Count > 0)
            {
                html.AppendLine("<h2>Топ сотрудников по выручке</h2>");
                html.AppendLine("<table>");
                html.AppendLine("    <thead>");
                html.AppendLine("        <tr><th>Сотрудник</th><th>Обработано заказов</th><th>Оказано услуг</th><th>Сгенерировано выручки</th></tr>");
                html.AppendLine("    </thead>");
                html.AppendLine("    <tbody>");
                foreach (var emp in stats.TopEmployees)
                {
                    html.AppendLine($"        <tr><td>{emp.EmployeeName}</td><td>{emp.OrdersProcessed}</td><td>{emp.ServicesProvided}</td><td>{emp.RevenueGenerated:C}</td></tr>");
                }
                html.AppendLine("    </tbody>");
                html.AppendLine("</table>");
            }

            html.AppendLine("</body>");
            html.AppendLine("</html>");

            return html.ToString();
        }
    }
}
