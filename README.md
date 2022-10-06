# Module1-Projects
 ApplePicker, Mission Demolition and Prototype 1
 
To play this game use the arrow keys.  Get the fruits to grow your snake, avoid the bombs.  Don't touch the walls, bombs, or the snake!
 
I created a snake game.  I thought it would be cool to recreate this classic game.  I had troubles making the camera and sometimes the edges of the play area cut out I think it is an issue with the "far" inspector within Unity. I tried to make this game using physics like we did for roll a ball but could not figure out how to do so. Instead I put all of the game objects into a list and move them to the previous position of the parent "node" of the snake.  This will move all of the nodes into the correct position for the snake to grow.  

I used the MDA here and wanted the player to experience frustration.  I played around with the spawn timings of the fruits and bombs and made them to where they were just fast enough that the player would need to quickly decide which to pick but slow enough where they would not get too upset about them despawning to fast.  I tried to add obstacles to the middle of the play area by adding in random walls but ended up not liking the direction the game was heading so removed them.
