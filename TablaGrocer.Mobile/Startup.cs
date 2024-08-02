using Android.Runtime;
using TableGrocer.EFCore;

namespace TablaGrocerMobile;
[Application]
public class Startup : Application
{
    public Startup(IntPtr handle, JniHandleOwnership transer)
        : base(handle, transer)
    {
    }
    public override void OnCreate()
    {
        base.OnCreate();
        // Initialize EF Core
        InitializeDatabase();
    }
    private void InitializeDatabase()
    {
        using (var context = new AppDbContext())
        {
            context.Database.EnsureCreated();
        }
    }
}