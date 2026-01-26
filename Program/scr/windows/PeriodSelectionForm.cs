using Program.scr.core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Program.scr.windows
{
    public partial class PeriodSelectionForm : Form
    {
        // Свойства для получения выбранных дат извне
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        // Флаг для проверки, были ли даты успешно выбраны
        public bool DatesSelected { get; private set; } = false;

        public PeriodSelectionForm()
        {
            InitializeComponent();
            InitializeComponent_();
        }

        private void InitializeComponent_()
        {
            this.Text = "Выбор периода";
            this.Size = new System.Drawing.Size(350, 180);
            this.StartPosition = FormStartPosition.CenterParent; // Открывать по центру родителя
            this.FormBorderStyle = FormBorderStyle.FixedDialog; // Запретить изменение размера
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Метка "Дата начала"
            Label labelStart = new Label()
            {
                Text = "Дата начала:",
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(100, 20)
            };

            // DateTimePicker для даты начала
            DateTimePicker dtpStart = new DateTimePicker()
            {
                Name = "dtpStartDate",
                Format = DateTimePickerFormat.Short,
                Location = new System.Drawing.Point(130, 20),
                Value = DateTime.Today.AddDays(-7) // Установить по умолчанию неделю назад
            };

            // Метка "Дата окончания"
            Label labelEnd = new Label()
            {
                Text = "Дата окончания:",
                Location = new System.Drawing.Point(20, 50),
                Size = new System.Drawing.Size(100, 20)
            };

            // DateTimePicker для даты окончания
            DateTimePicker dtpEnd = new DateTimePicker()
            {
                Name = "dtpEndDate",
                Format = DateTimePickerFormat.Short,
                Location = new System.Drawing.Point(130, 50),
                Value = DateTime.Today // Установить по умолчанию сегодняшнюю дату
            };

            // Кнопка OK
            Button btnOk = new Button()
            {
                Text = "OK",
                DialogResult = DialogResult.OK, // Устанавливает результат формы при нажатии
                Location = new System.Drawing.Point(130, 90),
                Size = new System.Drawing.Size(75, 23)
            };
            btnOk.Click += (sender, e) => OnOkClick(dtpStart, dtpEnd); // Подписка на событие

            // Кнопка Отмена
            Button btnCancel = new Button()
            {
                Text = "Отмена",
                DialogResult = DialogResult.Cancel,
                Location = new System.Drawing.Point(210, 90),
                Size = new System.Drawing.Size(75, 23)
            };

            // Добавление элементов на форму
            this.Controls.AddRange(new Control[] { labelStart, dtpStart, labelEnd, dtpEnd, btnOk, btnCancel });
        }

        // Обработчик нажатия кнопки OK
        private void OnOkClick(DateTimePicker dtpStart, DateTimePicker dtpEnd)
        {
            // Проверяем, что дата начала не позже даты окончания
            if (dtpStart.Value.Date > dtpEnd.Value.Date)
            {
                MessageBox.Show(
                    "Дата начала не может быть позже даты окончания.",
                    "Ошибка ввода",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                // Не закрываем форму, если даты некорректны
                this.DialogResult = DialogResult.None;
                return;
            }

            // Сохраняем выбранные даты
            StartDate = dtpStart.Value.Date; // .Date, чтобы убрать время
            EndDate = dtpEnd.Value.Date;
            DatesSelected = true; // Устанавливаем флаг успешного выбора

            var dataService = new DataService();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "HTML (*.html)|*.html";
            string path = null;
            if (saveFileDialog.ShowDialog() != DialogResult.Cancel) path = saveFileDialog.FileName;


            new AnalyticViewForm(dataService.GenerateAnalyticsReport(StartDate, EndDate, path)).ShowDialog();


        }
    }
}
