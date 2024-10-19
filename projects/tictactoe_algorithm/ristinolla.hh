#ifndef RISTINOLLA_HH
#define RISTINOLLA_HH
#include <tuple>

class Board {

public:
    Board();

    // Game state checks
    bool check_draw();
    bool check_win(char symbol);
    bool is_legal_move(int row, int col);

    // Game operations
    void initialize_board();
    void print_board();
    void make_move(int row, int col, char symbol);
    std::tuple<int, int, int> minimax(int depth, char maxing_symbol, bool is_maxing);
    void switchTurn();

    // Getters and setters
    char getHumanSymbol() const;
    char getCompSymbol() const;
    int getCurrentPlayer() const;
    void setCompSymbol(char symbol);
    void setHumanSymbol(char symbol);
    void setCurrentPlayer(int player);

private:
    char grid_[3][3];
    char comp_symbol_;
    char human_symbol_;
    int current_player_; // 1 for human, 2 for computer
};

#endif // RISTINOLLA_HH
