TetrisBetaWP
============

Tetris Beta for Windows Phone Project

Prototype for Tetris game for Windows Phone



TODO Items:

-Sound effect for droping/clearing

-Background Music

-Add Logo to Start Screen

-Add "Paused", "Game Over" boxes to Menu Screens

-Better Graphics/UI (Background, Touch Controls, HUD)

-Fix falling location of currentblock (currentblock should start @ (x,-1) & (x, 0))

-Adjust touch controls to allow holding down left/right/down to move current block at fixed rate

-Add 15 point to score per block droped

-Test balance of Score per level constant*

-Test lock delay



*Notes: Gravity is expressed in unit G, where 1G = 1 cell per frame, and 0.1G = 1 cell per 10 frames. Older games cannot move the tetromino down more than one cell per frame (60 cells per second). Newer games, especially those capable of a ghost piece, can do so; play at such speeds requires a lock delay.
