﻿using System;
using System.Collections.Generic;
using System.Text;
using MathLibrary;

namespace MathForGames
{
    class Player : Actor
    {
        private float _speed;
        private Vector2 _velocity;
        int _gold;
        item[] _inventory;

        public int Gold()
        {
            return _gold;
        }

        public bool Buy(item item, int playerInventory)
        {
            if (_gold >= item.cost)
            {
                _gold -= item.cost;

                _inventory[playerInventory] = item;

                return true;
            }
            return false;
        }
        public item[] GetInventory()
        {
            return _inventory;
        }

        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public Vector2 Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }

        public Player(char icon, float x, float y, float speed, string name = "Default", ConsoleColor color = ConsoleColor.White) 
            : base(icon, x, y, name, color)
        {
            _speed = speed;
        }

        public override void Update()
        {
            Vector2 moveDirection = new Vector2();

            ConsoleKey keyPressed = Engine.GetNextKey();

            if (keyPressed == ConsoleKey.A)
                moveDirection = new Vector2 { x = -1 };
            if (keyPressed == ConsoleKey.D)
                moveDirection = new Vector2 { x = 1 };
            if (keyPressed == ConsoleKey.W)
                moveDirection = new Vector2 { y = -1 };
            if (keyPressed == ConsoleKey.S)
                moveDirection = new Vector2 { y = 1 };

            Vector2 newPosition = new Vector2();

            newPosition.x += Position.x + Velocity.x;
            newPosition.y += Position.y + Velocity.y;


            Velocity = moveDirection * Speed;

            Position += Velocity; 
        }


        public virtual void OnCollision(Actor actor)
        {
            Engine.CloseApplication();
        }
    }
}