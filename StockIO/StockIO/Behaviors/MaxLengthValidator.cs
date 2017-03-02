using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace StockIO.Behaviors
{
    public class MaxLengthValidator : Behavior<Entry>
    {
        static readonly BindablePropertyKey IsValidPropertyKey = BindableProperty.CreateReadOnly("IsValid", typeof(bool), typeof(MaxLengthValidator), false);
        public static readonly BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;
        public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create("MaxLength", typeof(int), typeof(MaxLengthValidator), 0);

        public bool IsValid
        {
            get { return (bool)base.GetValue(IsValidProperty); }
            private set { base.SetValue(IsValidPropertyKey, value); }
        }

        public int MaxLength
        {
            get { return (int)GetValue(MaxLengthProperty); }
            set { SetValue(MaxLengthProperty, value); }
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += bindable_TextChanged;
        }

        private void bindable_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsValid = e.NewTextValue.Length > 0 && e.NewTextValue.Length <= MaxLength;
            if (e.NewTextValue.Length > MaxLength) ((Entry)sender).Text = "100";
            ((Entry)sender).TextColor = IsValid ? Color.Default : Color.Red;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= bindable_TextChanged;

        }
    }
}
