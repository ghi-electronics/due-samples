using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DUE.Graphics;

namespace DUE.Game {
    public class GameObject {
        private List<GameObject> gameObjects = new();
        private List<GameObject> toRemove = new();

        protected virtual void Update(Canvas canvas, double dt) {
            this.toRemove.Clear();
            foreach (var o in this.gameObjects) {
                o.Update(canvas, dt);
            }
            foreach(var o in this.toRemove) {
                this.gameObjects.Remove(o);
            }
        }

        protected void Add(GameObject o) => this.gameObjects.Add(o);

        protected void Remove(GameObject o) => this.toRemove.Add(o);
    }
}
