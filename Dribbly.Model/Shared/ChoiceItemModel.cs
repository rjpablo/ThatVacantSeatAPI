using Dribbly.Model.Courts;
using Dribbly.Service.Enums;

namespace Dribbly.Model.Shared
{
    public class ChoiceItemModel<TValue>
    {
        public string Text { get; set; }

        public TValue Value { get; set; }

        public string IconUrl { get; set; }

        public EntityTypeEnum  Type { get; set; }

        public ChoiceItemModel() { }

        public ChoiceItemModel(string text, TValue value, string iconUrl, EntityTypeEnum type)
        {
            Text = text;
            Value = value;
            IconUrl = iconUrl;
            Type = type;
        }
    }
}
