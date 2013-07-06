using GalaSoft.MvvmLight;
using System.Runtime.CompilerServices;

namespace cipele46.ViewModels
{
    public class ViewModelBaseEx : ViewModelBase
    {
        /// <summary>
        /// Force using new C# 5.0 feature.
        /// </summary>
        /// <param name="propertyName"></param>
        protected bool Set<T>(ref T field, T newValue, [CallerMemberName] string propertyName = "")
        {
            return Set(propertyName, ref field, newValue);
        }

        /// <summary>
        /// Force using new C# 5.0 feature.
        /// </summary>
        /// <param name="propertyName"></param>
        protected new void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.RaisePropertyChanged(propertyName);
        }
    }
}
