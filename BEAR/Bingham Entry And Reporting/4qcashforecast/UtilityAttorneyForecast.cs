
namespace BEAR._4qcashforecast
{
    public class UtilityAttorneyForecast
    {
        protected bool[] rowChanged;

        public void SetRowChanged(int totalRows)
        {
            this.rowChanged = new bool[totalRows];
        }
        public bool[] GetRowChanged()
        {
            return this.rowChanged;
        }

    }
}
