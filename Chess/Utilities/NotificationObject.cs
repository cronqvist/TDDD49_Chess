using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Utilities.Annotations;

namespace Utilities
{
    public abstract class NotificationObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged<T>(Expression<Func<T>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression != null)
            {
                OnPropertyChanged(memberExpression.Member.Name);
            }
        }
    }
}
