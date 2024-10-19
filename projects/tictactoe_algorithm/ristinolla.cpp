#include "ristinolla.hh"
#include <iostream>

/**
 * Constructor for the class.
 * Initializes a 3x3 board for the game
 */
Board::Board() {
    initialize_board();
}

void Board::initialize_board() {
    for (int i = 0; i < 3; ++i) {
        for (int j = 0; j < 3; ++j) {
            grid_[i][j] = ' ';
        }
    }
}

char Board::getCompSymbol() const {
    return comp_symbol_;
}

char Board::getHumanSymbol() const {
    return human_symbol_;
}

void Board::setCompSymbol(char symbol) {
    comp_symbol_ = symbol;
}

void Board::setHumanSymbol(char symbol) {
    human_symbol_ = symbol;
}

int Board::getCurrentPlayer() const {
    return current_player_;
}

void Board::setCurrentPlayer(int player) {
    current_player_ = player;
}

/**
 * Switch the turn between players
 */
void Board::switchTurn() {
    current_player_ = (current_player_ == 1) ? 2 : 1;
}

/**
 * Prints the current state of the game board.
 */
void Board::print_board() {
    for (int i = 0; i < 3; i++) {
        std::cout << "-------------" << std::endl;
        std::cout << "|";
        for (int j = 0; j < 3; j++) {
            std::cout << " " << grid_[i][j] << " |";
        }
        std::cout << std::endl;
    }
    std::cout << "-------------" << std::endl;
}

/**
 * Checks the legality of a move.
 *
 * @param row The row index of the move).
 * @param col The column index of the move.
 * @return true if the move is legal, false otherwise.
 */
bool Board::is_legal_move(int row, int col) {
    if (row < 0 || row > 2 || col < 0 || col > 2 || grid_[row][col] != ' ')
        return false;
    return true;
}

/**
 * Puts a symbol on the board.
 *
 * @param row The row of the move
 * @param col The column of the move
 * @param symbol The symbol to be placed.
 */
void Board::make_move(int row, int col, char symbol) {
    grid_[row][col] = symbol;
}

/**
 * Checks the board for a draw.
 *
 * @return true if draw, false otherwise.
 */
bool Board::check_draw() {
    for (int i = 0; i < 3; ++i)
        for (int j = 0; j < 3; ++j)
            if (grid_[i][j] == ' ')
                return false;
    return true;
}

/**
 * Checks if the specified symbol has won the game.
 *
 * @param symbol The symbol to check for a win.
 * @return true if the specified symbol has a winning combination, false otherwise.
 */
bool Board::check_win(char symbol) {
    for (int i = 0; i < 3; ++i) {
        if (grid_[i][0] == symbol && grid_[i][1] == symbol && grid_[i][2] == symbol)
            return true;
        if (grid_[0][i] == symbol && grid_[1][i] == symbol && grid_[2][i] == symbol)
            return true;
    }
    if (grid_[0][0] == symbol && grid_[1][1] == symbol && grid_[2][2] == symbol)
        return true;
    if (grid_[0][2] == symbol && grid_[1][1] == symbol && grid_[2][0] == symbol)
        return true;
    return false;
}

/**
 * Recursively evaluates all possible moves and returns the score,
 * row, and column of the best move.
 *
 * @param depth The depth of the minimax algorithm. 
 *              (0 being the current board state)
 * 
 * @param maxing_symbol The symbol of the player maximizing
 *                      the score ('X' or 'O').
 * 
 * @param is_maxing Indicates whether the current player
 *                  is maximizing or minimizing the score.
 * 
 * @return Tuple containing the best score, row, and 
 *         column of the move.
 */
std::tuple<int, int, int> Board::minimax(
    int depth, char maxing_symbol, bool is_maxing
) {
    // Check if the maximizing player has won
    if (check_win(maxing_symbol)) {
        return std::make_tuple(10 - depth, 0, 0);
    }

    // Check if the minimizing player has won (opposite of maximizing player)
    if (check_win(is_maxing ? human_symbol_ : comp_symbol_)) {
        return std::make_tuple(-10 + depth, 0, 0);
    }

    if (check_draw()) {
        return std::make_tuple(0, 0, 0);
    }
    int best_score = is_maxing ? -1000 : 1000;
    int best_row = -1;
    int best_col = -1;

    for (int i = 0; i < 3; i++) {
        for (int j = 0; j < 3; j++) {
            if (is_legal_move(i, j)) {

                // maximize computer and minimize human player
                grid_[i][j] = is_maxing ? comp_symbol_ : human_symbol_;

                // Recursing with the other player, increasing depth
                int score = std::get<0>(minimax(depth + 1, maxing_symbol, !is_maxing));

                // Undo move to clear simulated moves
                grid_[i][j] = ' ';
                
                if (is_maxing) {
                    if (score > best_score) {
                        best_score = score;
                        best_row = i;
                        best_col = j;                   
                    }
                } 
                else {
                    if (score < best_score) {
                        best_score = score;
                        best_row = i;
                        best_col = j;
                    }
                }
            }
        }
    }
    return std::make_tuple(best_score, best_row, best_col);
}
