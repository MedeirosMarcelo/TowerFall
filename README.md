# TowerFall 3D
Unity3D Towerfall 3D prototype

- Splitscreen: OK
- Controller: OK
- Flechas limitadas: OK
- Pegar flechas: OK
- Pegar flechas durante o dash
- Morrer
- Colisao da camera: OK
- Power Up
 - Spawn
 - Flechas bomba
 - Escudo
- Pular na cabeca do adversario: NetCode (tiago.bonetti)
- UI
 - flechas
 - Placar 
- Fluxo de jogo
 - Menus
 - Lobby
 - Start
 - End
- Castle: OK
- Player Model
 - Animations
- networking 0.1: OK
 - server dedicado
 - client
 - cenario fixo sem reflexao
 - clientes aparecem no server e outros clientes
 - sem flechas
- networking 0.2 : OK
 - clientes atiram flexas pelo server
- networking 0.3: OK
 - chat
- loop 0.2: (tiago.bonetti)
- relfections 0.2: (tiago.bonetti)

 Known Issues:
 - Flecha atirando para tras


## Dodge Stalling

A regular dodge lasts 20 frames, or 0.333 seconds, with a cooldown period of 25 frames, or 0.416
seconds (altogether 45 frames, 0.75 seconds). Holding a Dodge button will extend a dodge for an
extra 5 frames (for a total of 25 frames, 0.416 seconds), allowing the player to catch arrows that
would otherwise strike them during their dodge cooldown. 

	




