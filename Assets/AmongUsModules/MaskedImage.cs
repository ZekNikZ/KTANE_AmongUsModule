using System;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace AmongUsModules {
    public class MaskedImage : MonoBehaviour {
        public Transform spriteTransform;
        public SpriteRenderer spriteRenderer;
        public SpriteMask spriteMask;

        private Sprite _sprite;
        private Sprite _mask;

        private float _spriteWidth;
        private float _spriteHeight;
        private float _maskWidth;
        private float _maskHeight;

        // Use this for initialization
        private void Awake() {
            // Setup
            this._sprite = this.spriteRenderer.sprite;
            this._mask = this.spriteMask.sprite;

            this._spriteWidth = this._sprite.bounds.size.x;
            this._spriteHeight = this._sprite.bounds.size.y;

            this._maskWidth = this._mask.bounds.size.x;
            this._maskHeight = this._mask.bounds.size.y;
        }

        public bool translatePx(int x, int y) {
            var sprite = this.spriteRenderer.sprite;
            return this.translate(x / sprite.pixelsPerUnit, y / sprite.pixelsPerUnit);
            ;
        }

        public bool translate(float x, float y) {
            var currentPos = this.spriteTransform.localPosition;
            return this.setPos(currentPos.x + x, currentPos.y + y);
        }

        public bool setPosPx(int x, int y) {
            var sprite = this.spriteRenderer.sprite;
            return this.setPos(x / sprite.pixelsPerUnit, y / sprite.pixelsPerUnit);
        }

        public bool setPos(float x, float y) {
            this.spriteTransform.localPosition = new Vector3(Util.constrain(-this._maskWidth, x, this._maskWidth),
                Util.constrain(-this._maskHeight, y, this._maskHeight), 0);
            return Math.Abs(x - this.spriteTransform.localPosition.x) < 0.01 && Math.Abs(y - this.spriteTransform.localPosition.y) < 0.01;
        }

        public int getSizeX() {
            return (int) (this._sprite.pixelsPerUnit * this._sprite.bounds.size.x);
        }

        public int getSizeY() {
            return (int) (this._sprite.pixelsPerUnit * this._sprite.bounds.size.y);
        }

        public int getAbsPosX() {
            return (int) ((-this.spriteTransform.localPosition.x + this._spriteWidth / 2) * this._sprite.pixelsPerUnit);
        }

        public int getAbsPosY() {
            return (int) ((this.spriteTransform.localPosition.y + this._spriteHeight / 2) * this._sprite.pixelsPerUnit);
        }
    }
}
