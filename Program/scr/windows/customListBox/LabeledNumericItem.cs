namespace Program.scr.windows.customListBox
{
    public class LabeledNumericItem : UserControl
    {
        public int Id { get; private set; }
        public string Key { get; private set; }
        public decimal Value => numericUpDown.Value;

        public NumericUpDown numericUpDown;

        public LabeledNumericItem(int id, string key, decimal value, decimal maximum, Action<int, string, decimal> onValueChanged)
        {
            Id = id;
            Key = key;

            this.Height = 30;
            this.Width = 1000;
            this.AutoSize = false;

            var label = new Label
            {
                Text = key,
                Width = 390,
                Height = 25,
                Dock = DockStyle.Left,
                AutoEllipsis = true,
                //AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft,
                Margin = new Padding(3),
                Anchor = AnchorStyles.Left | AnchorStyles.Top
            };

            numericUpDown = new NumericUpDown
            {
                DecimalPlaces = 4,
                Minimum = 0,
                Maximum = Math.Max(0, maximum), // Ограничиваем минимумом, чтобы не было <= 0
                Increment = 1.0000m,
                Value = Math.Min(Math.Max(0, value), maximum), // Значение в пределах [min, max]
                Width = 120,
                Height = 25,
                Location = new Point(400, 0),
                Dock = DockStyle.Right,
                Margin = new Padding(3),
                
                Anchor = AnchorStyles.Right | AnchorStyles.Top                
            };
            numericUpDown.ValueChanged += (object? sender, EventArgs e) => 
            {
                numericUpDown.BackColor = numericUpDown.Maximum == 0 ? Color.IndianRed : Color.White;
                numericUpDown.BackColor = numericUpDown.Value == 0 ? Color.White : Color.LightGreen;            
                onValueChanged?.Invoke(Id, Key, numericUpDown.Value);
            };

            this.Controls.Add(numericUpDown);
            this.Controls.Add(label);

            numericUpDown.BackColor = numericUpDown.Value == 0 ? Color.White : Color.LightGreen;
        }


        public void SetValue(decimal value)
        {
            if (value < 0) value = 0;
            if (value > numericUpDown.Maximum) value = numericUpDown.Maximum;
            numericUpDown.Value = Math.Round(value, 4);
        }

        public void SetMaximum(decimal maximum)
        {
            if (maximum <= 0) maximum = 1.0000m;
            numericUpDown.Maximum = Math.Round(maximum, 4);
            if (numericUpDown.Value > numericUpDown.Maximum)
                numericUpDown.Value = numericUpDown.Maximum;
        }
    }
}
