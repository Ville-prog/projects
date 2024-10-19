#include "ristinolla.hh"
#include "gamelogic.hh"
#include <string>
#include <iostream>

/**
 * The main game loop. Alternates moves between
 * players until the game ends. When a game ends,
 * player can choose wether to restart or not.
 *
 * @param board The game board.
 */
void game_loop(Board& board) {
    while (true) {
        handle_current_move(board);

        if (check_win(board) || check_draw(board)) {
            if (!play_again(board)) {
                return;
            }
        }
        else {
            board.switchTurn();
        }
    }
}

int main() {
    Board board;
    start_game(board);
    game_loop(board);

    return 0;
}
