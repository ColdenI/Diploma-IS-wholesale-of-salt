namespace Program.scr.windows.customListBox
{
    public class CustomNumericListBox : Panel
    {
        private FlowLayoutPanel flowPanel;
        private List<LabeledNumericItem> items;
        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        public IReadOnlyList<LabeledNumericItem> Items => items.AsReadOnly();

        public CustomNumericListBox()
        {
            items = new List<LabeledNumericItem>();

            flowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };

            this.Controls.Add(flowPanel);
            this.BorderStyle = BorderStyle.FixedSingle;
        }

        // Добавление с ID
        public void Add(int id, string key, decimal value, decimal maxValue = 1000.0000m)
        {
            // Лямбда вызывает наше событие
            void OnValueChanged(int itemId, string itemKey, decimal newValue)
            {
                ValueChanged?.Invoke(this, new ValueChangedEventArgs(itemId, itemKey, newValue));
            }

            var item = new LabeledNumericItem(id, key, value, maxValue, OnValueChanged);
            items.Add(item);
            flowPanel.Controls.Add(item);
        }

        // Получение значения по ID
        public decimal? GetValueById(int id)
        {
            var item = items.FirstOrDefault(i => i.Id == id);
            return item?.Value;
        }

        // Опционально: получение элемента по ID
        public LabeledNumericItem GetItemById(int id)
        {
            return items.FirstOrDefault(i => i.Id == id);
        }

        public void Clear()
        {
            flowPanel.Controls.Clear();
            items.Clear();
        }

        public void RemoveAt(int index)
        {
            if (index >= 0 && index < items.Count)
            {
                flowPanel.Controls.Remove(items[index]);
                items.RemoveAt(index);
            }
        }
    }

    public class ValueChangedEventArgs : EventArgs
    {
        public int Id { get; }
        public string Key { get; }
        public decimal Value { get; }

        public ValueChangedEventArgs(int id, string key, decimal value)
        {
            Id = id;
            Key = key;
            Value = value;
        }
    }
}
