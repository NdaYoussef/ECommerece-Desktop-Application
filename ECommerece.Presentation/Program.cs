using ECommerece.Application.Mappers;


namespace ECommerece.Presentation
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            #region Register mapping services using mapster
            Mapping.RegisterAllMapping(); 
            #endregion

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            System.Windows.Forms.Application.Run(new Form1());


        }
    }
}