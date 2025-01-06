
namespace TriviaGameApp.Services;
public class CategoryOption
{
    public string Value { get; set; } = "any";
    public string Text { get; set; } = "Any Category";
    public override string ToString() => Text; 
}

public class SimpleOption
{
    public string Value { get; set; } = "any";
    public string Text { get; set; } = "Any";
    public override string ToString() => Text; 
}
public static class CategoryHelper 
{
    public static readonly List<CategoryOption> AllCategories = new(){


    new() { Value = "any", Text = "Any Category" },
    new() { Value = "9",   Text = "General Knowledge" },
    new() { Value = "10",  Text = "Entertainment: Books" },
    new() { Value = "11",  Text = "Entertainment: Film" },
    new() { Value = "12",  Text = "Entertainment: Music" },
    new() { Value = "13",  Text = "Entertainment: Musicals & Theatres" },
    new() { Value = "14",  Text = "Entertainment: Television" },
    new() { Value = "15",  Text = "Entertainment: Video Games" },
    new() { Value = "16",  Text = "Entertainment: Board Games" },
    new() { Value = "17",  Text = "Science & Nature" },
    new() { Value = "18",  Text = "Science: Computers" },
    new() { Value = "19",  Text = "Science: Mathematics" },
    new() { Value = "20",  Text = "Mythology" },
    new() { Value = "21",  Text = "Sports" },
    new() { Value = "22",  Text = "Geography" },
    new() { Value = "23",  Text = "History" },
    new() { Value = "24",  Text = "Politics" },
    new() { Value = "25",  Text = "Art" },
    new() { Value = "26",  Text = "Celebrities" },
    new() { Value = "27",  Text = "Animals" },
    new() { Value = "28",  Text = "Vehicles" },
    new() { Value = "29",  Text = "Entertainment: Comics" },
    new() { Value = "30",  Text = "Science: Gadgets" },
    new() { Value = "31",  Text = "Entertainment: Japanese Anime & Manga" },
    new() { Value = "32",  Text = "Entertainment: Cartoon & Animations" }
    };

    public static string GetCategoryText(string value)
    {
        var category = AllCategories.Find(c => c.Value == value);
        return category?.Text ?? "Any Category";
    }
}
    

public static class DifficultyHelper{

    public static readonly List<SimpleOption> AllDifficulties = new()
    {
        new() { Value = "any",   Text = "Any Difficulty" },
        new() { Value = "easy",  Text = "Easy" },
        new() { Value = "medium",Text = "Medium" },
        new() { Value = "hard",  Text = "Hard" }
    };

}

public static class TypeHelper{

    public static readonly List<SimpleOption> AllTypes = new()
    {
        new() { Value = "any",      Text = "Any Type" },
        new() { Value = "multiple", Text = "Multiple Choice" },
        new() { Value = "boolean",  Text = "True / False" }
    };
}