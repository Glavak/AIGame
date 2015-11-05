﻿using GameServer.Abstract;
using GameServer.DataEntities;
using Jurassic;
using Jurassic.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameServer.GameLogic.JSClasses
{
    public class CellEngine : Cell
    {
        public override CellTypesEnum type
        {
            get { return CellTypesEnum.Engine; }
        }

        [JSFunction(Name = "jump")]
        public int Jump()
        {
            if (status.stepsToReady == 0)
            {
                this.parent.owner.Events.Add(
                    new Event("jump", new string[] { }));

                // Clear connection with previous enemy
                if (parent.owner.enemyShip != null)
                {
                    parent.owner.enemyShip.owner.enemyShip = null;
                    parent.owner.enemyShip = null;
                }

                // Found another enemy, with some chance
                User nextEnemy;
                Random r = new Random();
                if (r.NextDouble() <= 0.3)
                {
                    nextEnemy = repository.Users.ToList()[r.Next(repository.Users.Count())];
                    if (nextEnemy.enemyShip == null && nextEnemy != parent.owner)
                    {
                        nextEnemy.enemyShip = parent;
                        parent.owner.enemyShip = nextEnemy.ship;
                    }
                }

                status.stepsToReady = 15;
            }

            return 0;
        }

        [JSFunction(Name = "power")]
        public override int Power(int energy)
        {
            return base.Power(energy);
        }

        public IUserRepository repository;

        public CellEngine(Ship parent, int roomId)
            : base(parent, roomId) { }
    }
}