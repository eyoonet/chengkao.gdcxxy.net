using Stylet;
using System;
using System.Collections.Generic;
using System.Text;

namespace console
{
    public class LogItemViewModel : PropertyChangedBase
    {

        public LogItemViewModel(string content, string color = LogColor.Message, string weight = "Regular")
        {
            Time = DateTime.Now.ToString("MM'-'dd'  'HH':'mm':'ss-fff");
            this.Content = content;
            this.Color = color;
            this.Weight = weight;
        }
        private string _time;

        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        public string Time
        {
            get => _time;
            set => SetAndNotify(ref _time, value);
        }

        private string _content;

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        public string Content
        {
            get => string.Format("{0} {1}",Time,_content);
            set => SetAndNotify(ref _content, value);

        }

        private string _color;

        /// <summary>
        /// Gets or sets the font color.
        /// </summary>
        public string Color
        {
            get => _color;
            set => SetAndNotify(ref _color, value);
        }

        private string _weight;

        /// <summary>
        /// Gets or sets the font weight.
        /// </summary>
        public string Weight
        {
            get => _weight;
            set => SetAndNotify(ref _weight, value);
        }
    }
}
