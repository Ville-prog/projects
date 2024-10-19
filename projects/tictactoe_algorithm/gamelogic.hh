#ifndef GAMELOGIC_HH
#define GAMELOGIC_HH

#include "ristinolla.hh"

char choose_human_symbol();
void start_game(Board& board);
void handle_human_move(Board& board, char human_symbol);
void handle_computer_move(Board& board, char comp_symbol);
bool ask_to_play_again();
void handle_current_move(Board& board);
bool check_win(Board& board);
bool check_draw(Board& board);
bool play_again(Board& board);

#endif // GAMELOGIC_HH
