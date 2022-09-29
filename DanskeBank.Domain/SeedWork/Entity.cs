namespace DanskeBank.Domain.SeedWork;

public abstract class Entity
{
    public virtual Guid Id
    {
        get => _id;
        set => _id = value;
    }

    int? _requestedHashCode;
    Guid _id;

    public override bool Equals(object obj)
    {
        if (obj == null || obj is not Entity)
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        if (GetType() != obj.GetType())
            return false;

        var item = (Entity)obj;

        return item.Id == Id;
    }

    public override int GetHashCode()
    {
        if (!_requestedHashCode.HasValue)
            _requestedHashCode = Id.GetHashCode() ^ 31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)

        return _requestedHashCode.Value;
    }

    public static bool operator ==(Entity left, Entity right)
    {
        if (Equals(left, null))
            return Equals(right, null);
        else
            return left.Equals(right);
    }

    public static bool operator !=(Entity left, Entity right) =>
        !(left == right);
}
