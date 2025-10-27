namespace Enums
{
    [Flags]
    public enum Permission
    {
        None = 0,
        CanView = 1,
        CanAdd = 2,
        CanEdit = 4,
        CanDelete = 8,
        All = CanView | CanAdd | CanEdit | CanDelete
    }
}
