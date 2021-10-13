using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using MathLibrary;

namespace MathForGames
{
    struct item
    {
        public string name;
        public int cost;
    }

    class Engine
    {
        private static bool _shouldApplicationClose = false;
        private Scene[] _scenes = new Scene[0];
        private static int _currentSceneIndex;
        private static Icon[,] _buffer;
        int playerIndex = -1;
        private static Player _player;
        private Shop _shop;
        private int _currentScene;
        public static item[] _shopStock;

        private item _blueBerry;
        private item _animeFigure;
        private item _panacea;

        /// <summary>
        /// Called to bein the application.
        /// </summary>
        public void Run()
        {
            //Call start for the entire application.
            Start();

            //Loop until the application is told to close.
                while (!_shouldApplicationClose)
                {
                    Update();
                    Draw();
                    
                    Thread.Sleep(150);
                }

            // call end for the entire application.
            End();  
        }

        /// <summary>
        /// Called when the application starts
        /// </summary>
        private void Start()
        {
            Scene scene = new Scene();
            Actor actor = new Actor('$', new MathLibrary.Vector2 { x = 0, y = 0 },  "Shop", ConsoleColor.Green);
            Actor actor2 = new Actor('W', new MathLibrary.Vector2 { x = 10, y = 10 }, "Actor2", ConsoleColor.Blue);
            Player player = new Player('@', 5, 5, 1, "Player", ConsoleColor.Yellow, 5000);

            scene.AddActor(actor);
            scene.AddActor(actor2);
            scene.AddActor(player);

            _currentSceneIndex = AddScene(scene);

            _scenes[_currentSceneIndex].Start();

            Console.CursorVisible = false;

            _shopStock = new item[] { _blueBerry, _animeFigure, _panacea };
            _shop = new Shop(_shopStock);
        }


        /// <summary>
        /// Called everytime the game loops
        /// </summary>
        private void Update()
        {
            _scenes[_currentSceneIndex].Update();

            while (Console.KeyAvailable)
                Console.ReadKey(true);
        }

        /// <summary>
        /// Called everytime the game loops to the update visuals
        /// </summary>
        private void Draw()
        {
            //Clear the stuff that was on the screen in the last frame
            _buffer = new Icon[Console.WindowWidth, Console.WindowHeight - 1];

            //Reset the curse posistion to the top of the previous scren is drawn over 
            Console.SetCursorPosition(0, 0);

            //Adds all actor icons to buffer
            _scenes[_currentSceneIndex].Draw();

            //Iterate through buffer
            for (int y = 0; y < _buffer.GetLength(1); y++)
            {
                for (int x = 0; x < _buffer.GetLength(0); x++)
                {
                    if (_buffer[x, y].Symbol == '\0')
                        _buffer[x, y].Symbol = ' ';

                    //Set console text color to be the color of the item at buffer
                    Console.ForegroundColor = _buffer[x, y].color;
                    //print the symbol of the item in the buffer
                    Console.Write(_buffer[x, y].Symbol);
                }
                //skip a line once at the end of a row
                Console.WriteLine();
            }
            
        }
        /// <summary>
        /// Called when the application exits
        /// </summary>
        private void End()
        {
            _scenes[_currentSceneIndex].End();
        }

        /// <summary>
        /// Addsa a scene to thje engine's scene array
        /// </summary>
        /// <param name="scene">the scene that will be added to the scene array</param>
        /// <returns>the index where the new scene is located</returns>
        public int AddScene(Scene scene)
        {
            //Creats a new temporary array
            Scene[] tempArray = new Scene[_scenes.Length + 1];
            //copies all values from the old array into the new array
            for (int i = 0; i < _scenes.Length; i ++)
            {
                tempArray[i] = _scenes[i];
            }
            //Set the last index to be the new scene
            tempArray[_scenes.Length] = scene;

            //set the old array to be the new array
            _scenes = tempArray;
            //return the last array
            return _scenes.Length - 1;
        }
        /// <summary>
        /// Gets the next key in the input stream
        /// </summary>
        /// <returns>The key that was pressed</returns>
        public static ConsoleKey GetNextKey()
        {
            //If there is no key being pressed....
            if (!Console.KeyAvailable)
                //...return
                return 0;

            //Return the current key being pressed4
            return Console.ReadKey(true).Key;
        }


        /// <summary>
        /// Adds the icon to the buffer to print to the screen in the next draw call.
        /// Prints the icon at the given position in the bugger.
        /// </summary>
        /// <param name="icon">The icon to draw</param>
        /// <param name="position">The position of the icon in the buggers</param>
        /// <returns>False if the posistion is outside the bounds of the buffer</returns>
        public static bool Render(Icon icon, Vector2 position)
        {
            //if the posistion is out of bounds....
            if (position.x < 0 || position.x >= _buffer.GetLength(0) || position.y < 0 ||  position.y >= _buffer.GetLength(1))
                //return false
                return false;

            //Set the bugger at the index of the given posistion to be the icon.
            _buffer[(int)position.x, (int)position.y] = icon;
            return true;
        }

        /// <summary>
        /// Ends the game and closes the window. 
        /// </summary>
        public static void CloseApplication()
        {
            _shouldApplicationClose = true;
        }

        public static void PrintInventory(item[] inventory)
        {
            for (int i = 0; i < inventory.Length; i++)
            {
                Console.WriteLine((i + 1) + ". " + inventory[i].name + "$" + inventory[i].cost);
            }
        }

        public static void ShoppingMenu()
        {
            Console.WriteLine("Welcome! Please select an item.");
            //Shows the player the amount of money they have left
            Console.WriteLine("\nYou have: $" + Player.Gold());
            //shows the player what the shop has
            PrintInventory(_shopStock);
            char input = Console.ReadKey().KeyChar;
            int itemIndex = -1;
            switch (input)
            {
                //each case is an item to buy
                case '1':
                    {
                        itemIndex = 0;
                        break;
                    }
                case '2':
                    {
                        itemIndex = 1;
                        break;
                    }
                case '3':
                    {
                        itemIndex = 2;
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Invalid Input");
                        Console.ReadKey(true);
                        Console.Clear();
                        return;
                    }
            }

            if (Player.Gold() < _shopStock[itemIndex].cost)
            {
                Console.WriteLine("You cant afford this.");
                return;
            }

            //allows the player to place the item they want in the slot of their choice
            Console.WriteLine("Choose a slot to replace.");
            //shows the player inventory 
            PrintInventory(_player.GetInventory());
            input = Console.ReadKey().KeyChar;

            int playerIndex = -1;
            //each case is a slot the player can put an item into
            switch (input)
            {
                case '1':
                    {
                        playerIndex = 0;
                        break;
                    }
                case '2':
                    {
                        playerIndex = 1;
                        break;
                    }
                case '3':
                    {
                        playerIndex = 2;
                        break;
                    }
            }
            Shop.Sell(_player, itemIndex, playerIndex);
        }
    }
}
