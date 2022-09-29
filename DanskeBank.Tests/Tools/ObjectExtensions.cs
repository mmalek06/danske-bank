using System.Linq.Expressions;

namespace DanskeBank.Tests.Tools;

internal static class ObjectExtensions
{
    internal static TInstance SetPrivateProperty<TInstance, TValue>(
        this TInstance instance,
        Expression<Func<TInstance, TValue>> selector,
        TValue value)
    {
        if (selector.Body is not MemberExpression mBody)
        {
            var uBody = (UnaryExpression)selector.Body;

            mBody = (uBody.Operand as MemberExpression)!;
        }

        instance!.GetType().GetProperty(mBody.Member.Name)!.SetValue(instance, value);

        return instance;
    }
}
