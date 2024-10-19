#include "gamelogic.hh"
#include "ristinolla.hh"
#include <string>
#include <iostream>

/**
 * Prompts the human player to choose their symbol (X or O).
 *
 * @return char The chosen symbol ('X' or 'O').
 */
char choose_human_symbol() {
    std::string player_choice;
    std::cout << "\nDo you wish to go 1st (X) or 2nd? (O)" << std::endl;
    std::cout << "Please enter X or O: ";

    while (true) {
        std::cin >> player_choice;
        char choice = std::toupper(player_choice[0]);

        if (player_choice.length() != 1 || (choice != 'X' && choice != 'O')) {
            std::cout << "Invalid input. Please enter X or O: ";
        }
        else {
            return choice;
        }
    }
}

/**
 * Initializes the game by setting up the board and player symbols.
 *
 * @param board Reference to the game board.
 */
void start_game(Board& board) {
    board.initialize_board();
    char human_symbol = choose_human_symbol();
    board.setHumanSymbol(human_symbol);

    char comp_symbol = (human_symbol == 'X') ? 'O' : 'X';
    board.setCompSymbol(comp_symbol);
    board.setCurrentPlayer((human_symbol == 'X') ? 1 : 2);

    board.print_board();
}

/**
 * Handles the human player's move. Checks move legality.
 *
 * @param human_symbol The symbol used by the human player.
 * @param board Reference to the game board.
 */
void handle_human_move(Board& board, char human_symbol) {
    while (true) {
        int row, col;
        std::cout << "Choose a row then a column. Valid range (1-3)" << std::endl;
        std::cout << "Enter coordinates seperated by whitespace (e.g. 1 3): ";
        std::cin >> row >> col;

        // Actual gameboard indexing starts at 0
        row --;
        col --;

        if (!board.is_legal_move(row, col)) {
            std::cout << "\nIllegal move!\n" << std::endl;
        } else {
            board.make_move(row, col, human_symbol);
            return;
        }
    }
}

/**
 * Handles the computer's move.
 *
 * @param comp_symbol The symbol used by the computer.
 * @param board Reference to the game board.
 */
void handle_computer_move(Board& board, char comp_symbol) {
    int score, row, col;
    std::tie(score, row, col) = board.minimax(0, comp_symbol, true);
    board.make_move(row, col, comp_symbol);
}

/**
 * Handles the current player's move and prints the updated game board.
 *
 * @param board Reference to the game board.
 */
void handle_current_move(Board& board) {
    char human_symbol = board.getHumanSymbol();
    char comp_symbol = board.getCompSymbol();
    char current_player = board.getCurrentPlayer();

    if (current_player == 1) {
        handle_human_move(board, human_symbol);
    } else {
        handle_computer_move(board, comp_symbol);
    }
    board.print_board();
}

/**
 * Checks if the current player has won the game and prints a
 * message if they have.
 *
 * @param board Reference to the game board.
 * @return true if the current player has won, false otherwise.
 */
bool check_win(Board& board) {
    char current_player = board.getCurrentPlayer();
    char symbol = (current_player == 1) ? board.getHumanSymbol() : board.getCompSymbol();

    if (board.check_win(symbol)) {
        std::string winner = (current_player == 1) ? "Player" : "Computer";
        std::cout << winner << " wins!" << std::endl;
        return true;
    }
    return false;
}

/**
 * Checks if the game is a draw and prints a message if it is.
 *
 * @param board Reference to the game board.
 * @return true if the game is a draw, false otherwise.
 */
bool check_draw(Board& board) {
    if (board.check_draw()) {
        std::cout << "Draw!" << std::endl;
        return true;
    }
    return false;
}

/**
 * Prompts the user to play again or quit the game.
 * If the user chooses to play again, the game is restarted.
 *
 * @param board Reference to the game board.
 * @return true if the user chooses to play again, false if the user chooses to quit.
 */
bool play_again(Board& board) {
    std::cout << "Game over. Play again (P) or quit (Q)" << std::endl;
    std::string ask_to_play_again_input;

    while (true) {
        std::cin >> ask_to_play_again_input;

        if (toupper(ask_to_play_again_input[0]) == 'Q') {
            return false;
        }
        if (toupper(ask_to_play_again_input[0]) == 'P') {
            start_game(board);
            return true;
        } else {
            std::cout << "Incorrect input. Play again (P) or quit (Q)" << std::endl;
        }
    }
}
