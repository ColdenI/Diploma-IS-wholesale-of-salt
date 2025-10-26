using Program.scr.core.dbt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program.scr.core
{
    public class Core
    {
        public static DBT_Employees ThisEmployee;
        public static int AccessLevel = -1;

        public static string[] arrAccess = { "Нет доступа", "Админ", "Менеджер по заказам", "Менеджер по услугам" };
    }
}
