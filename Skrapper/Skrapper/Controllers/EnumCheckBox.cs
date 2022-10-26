using System;
using System.Reflection;
using Xamarin.Forms;

namespace Skrapper
{
    class EnumCheckBox : CheckBox
    {
        public static readonly BindableProperty EnumTypeProperty =
            BindableProperty.Create(nameof(EnumType), typeof(Type), typeof(EnumCheckBox),
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    EnumCheckBox cb = (EnumCheckBox)bindable;
                    
                    if(newValue != null)
                    {
                        if (!((Type)newValue).GetTypeInfo().IsEnum)
                            throw new ArgumentException("EnumCheckBox: EnumType property must be enumeration type");
                    }
                });

        public Type EnumType
        {
            set => SetValue(EnumTypeProperty, value);
            get => (Type)GetValue(EnumTypeProperty);
        }
    }

}
