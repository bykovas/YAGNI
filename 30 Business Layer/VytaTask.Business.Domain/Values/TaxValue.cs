using VytaTask.CrossCutting.ValueConverters;

namespace VytaTask.Business.Domain.Values
{
    public class TaxValue
    {
        private decimal _value { get; set; }

        public TaxValue(decimal value = 0.0M)
        {
            _value = value;
        }

        public decimal Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (_value == value)
                    return;

                _value = value;
            }
        }

        public override string ToString()
        {
            return TaxConverter.DecimalToQuantityString(this.Value);
        }


        #region Implicit Conversion

        public static implicit operator TaxValue(decimal value)
        {
            return new TaxValue(value);
        }

        public static implicit operator decimal(TaxValue money)
        {
            return money.Value;
        }

        #endregion
    }
}
