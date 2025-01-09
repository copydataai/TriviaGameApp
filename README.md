# TriviaGameApp

TriviaGameApp is a cross-platform trivia game application built using .NET MAUI. It supports Android, iOS, MacCatalyst, and Windows platforms.

## Features

- Cross-platform support (Android, iOS, MacCatalyst, Windows)
- Interactive trivia questions
- Leaderboard to track high scores
- Integration with Open Trivia Database (OpenTDB)

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- Visual Studio 2022 or later with .NET MAUI workload installed

### Installation

1. Clone the repository:

    ```sh
    git clone https://github.com/copydataai/TriviaGameApp.git
    ```

2. Navigate to the project directory:

    ```sh
    cd TriviaGameApp
    ```

3. Restore the project dependencies:

    ```sh
    dotnet restore
    ```

### Running the App

To run the app on a specific platform, use the following commands:

- Android:

    ```sh
    dotnet build -t:Run -f net8.0-android
    ```

- iOS:

    ```sh
    dotnet build -t:Run -f net8.0-ios
    ```

- MacCatalyst:

    ```sh
    dotnet build -t:Run -f net8.0-maccatalyst
    ```

- Windows:

    ```sh
    dotnet build -t:Run -f net8.0-windows10.0.19041.0
    ```

## License

This project is licensed under the MIT License - see the [LICENSE.txt](http://_vscodecontentref_/1) file for details.