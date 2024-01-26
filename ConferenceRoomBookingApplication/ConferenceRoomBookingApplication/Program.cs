namespace ConferenceRoomBookingApplication
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Use method below once to add som testdata to the database
            //TestData.AddToDatabase();

            Models.User currentUser = Navigation.ShowStartPage();
            Navigation.ToMenu(currentUser);
        }
    }
}