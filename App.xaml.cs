protected override void OnStartup(StartupEventArgs e)
{
    try
    {
        using var context = new DbContext();
        context.Database.EnsureCreated();
        
        // Optional: Add database seeding here if needed
        // SeedDatabase(context);
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Database initialization failed: {ex.Message}", 
                       "Database Error", 
                       MessageBoxButton.OK, 
                       MessageBoxImage.Error);
        Shutdown(1);
        return;
    }
    
    base.OnStartup(e);
}