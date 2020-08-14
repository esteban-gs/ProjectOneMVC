namespace ProjectOneMVC.Data.Data.Initialization
{
    public static class SeedData
    {
        public static string ClassJson
        {
            get => @"
[
	{
		'Name' : 'C#',
		'Description' : 'Learn C#',
		'Price' : 200.0000
	},
	{
		'Name' : 'ASP.NET MVC',
		'Description' : 'Learn how to create websites',
		'Price' : 250.0000
	},
	{
		'Name' : 'Android',
		'Description' : 'Learn how to write Android applications',
		'Price' : 500.0000
	},
	{
		'Name' : 'Design Patterns',
		'Description' : 'Learn how to write code better',
		'Price' : 300.0000
	}
]
";
        }
    }
}
