#include <iostream>
#include <ctime>

using namespace std;

int main()
{
    bool PlayGame(int Difficulty);
    srand(time(NULL)); //creates a new sequence depending on time of day

    int LevelDifficulty = 1;
    const int MaxDifficulty = 3;
    
    cout << "Want to play a game?\n";

    while (LevelDifficulty <= MaxDifficulty) //Loop game until complete
    {
        bool bLevelCompelete = PlayGame(LevelDifficulty);
        cin.clear(); //Clears errors
        cin.ignore(); //Discards the Buffer

        if (bLevelCompelete)
        {
            ++LevelDifficulty;
        }
    }

    cout << "Congratulations, you've completed the game!\n\n";
    
    return 0;
}

bool PlayGame(int Difficulty)
{

    const int FirstNum = rand() % Difficulty + Difficulty;
    const int SecondNum = rand() % Difficulty + Difficulty;
    const int ThirdNum = rand() % Difficulty + Difficulty;

    const int GameSum = FirstNum + SecondNum + ThirdNum;
    const int GameProduct = FirstNum * SecondNum * ThirdNum;

    cout << "\nLevel " << Difficulty;
    cout << "\nI'm thinking of 3 numbers...";
    cout << "\nThey add up to: " << GameSum;
    cout << "\nAnd their product is: " << GameProduct;
    cout << "\nWhat are the numbers? ";

    int FirstAnswer, SecondAnswer, ThirdAnswer;
    cin >> FirstAnswer >> SecondAnswer >> ThirdAnswer;
    cout << "You answered: " << FirstAnswer << SecondAnswer << ThirdAnswer
        << "\n\n";


    int AnswerSum = FirstAnswer + SecondAnswer + ThirdAnswer;
    int AnswerProduct = FirstAnswer * SecondAnswer * ThirdAnswer;

    //Check player answers
    if (GameSum == AnswerSum && GameProduct == AnswerProduct)
    {
        cout << "You are correct!\n";
        return true;
    }
    else
    {
        cout << "That is incorrect.\nTry again.\n\n";
        return false;
    }
}

