using Android.Runtime;
using TableGrocer.EFCore;
using Xamarin.Essentials;

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
        var path = "";
        using (var context = new AppDbContext())
        {
            context.Database.EnsureCreated();
            path = context.DbPath;
        }
        

        string backupfile = Path.Combine(FileSystem.AppDataDirectory, "backup.db3");
        File.Copy(path, backupfile, true);

    }
}