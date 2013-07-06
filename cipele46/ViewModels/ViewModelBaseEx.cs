using GalaSoft.MvvmLight;
using System.Runtime.CompilerServices;

namespace cipele46.ViewModels
{
    public class ViewModelBaseEx : ViewModelBase
    {
        protected bool Set<T>(ref T field, T newValue, [CallerMemberName] string propertyName = "")
        {
            return Set(propertyName, ref field, newValue);
        }
    }
}
