using Infracciones.App.ViewModels;

namespace Infracciones.Helpers
{
    public class Field<T> : BaseViewModel where T : class
    {
        public T Value { get; set; } = default;
        public bool IsNotValid { get; set; } = false;
        public string NotValidMessageError { get; set; } = "";
    }
}
