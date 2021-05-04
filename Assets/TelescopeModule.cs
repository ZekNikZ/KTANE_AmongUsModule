using System;
using System.Linq;
using AmongUsModules;
using KModkit;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class TelescopeModule : MonoBehaviour {
    public KMSelectable leftButton;
    public KMSelectable rightButton;
    public KMSelectable upButton;
    public KMSelectable downButton;
    public KMSelectable submitButton;

    public KMBombModule bombModule;
    public KMBombInfo bombInfo;

    public GameObject recticle;

    public MaskedImage screen;

    public int speed = 500;
    public float flashTime = 0.3f;
    public double recticleThreshold = 100;

    private int _x;
    private int _y;
    private float _lastFlashDelay;
    private bool _flashState = true;

    private int _solution = -1;

    private static readonly string[] POINTS_OF_INTEREST_NAMES = {
        "BLACK_HOLE", "DYSON_SPHERE", "SPACESHIP", "GAS_GIANT", "GALAXY", "BROKEN_PLANET", "NEBULA"
    };

    private static readonly Vector2Int[] POINTS_OF_INTEREST_LOCATIONS = {
        new Vector2Int(1020, 1620), new Vector2Int(1470, 235), new Vector2Int(390, 990), new Vector2Int(1500, 1370),
        new Vector2Int(360, 1450), new Vector2Int(1010, 620), new Vector2Int(570, 250)
    };

    // TODO: submit button
    // TODO: look pretty
    // TODO: increase dimensions of image to better suit BLACK_HOLE and DYSON_SPHERE

    void Start() {
        // Button handlers
        this.leftButton.OnInteract += delegate {
            this._x = 1;
            return false;
        };
        this.rightButton.OnInteract += delegate {
            this._x = -1;
            return false;
        };
        this.upButton.OnInteract += delegate {
            this._y = -1;
            return false;
        };
        this.downButton.OnInteract += delegate {
            this._y = 1;
            return false;
        };
        this.leftButton.OnInteractEnded += delegate { this._x = 0; };
        this.rightButton.OnInteractEnded += delegate { this._x = 0; };
        this.upButton.OnInteractEnded += delegate { this._y = 0; };
        this.downButton.OnInteractEnded += delegate { this._y = 0; };

        this.submitButton.OnInteract += OnSubmit;

        this._solution = (this.bombInfo.GetBatteryCount() + this.bombInfo.GetPortCount()) %
            POINTS_OF_INTEREST_NAMES.Length;

        this.screen.setPosPx(Random.Range(-this.screen.getSizeX() / 2 + 200, this.screen.getSizeX() / 2 - 200),
            Random.Range(-this.screen.getSizeY() / 2 + 200, this.screen.getSizeY() / 2 - 200));
    }

    private bool OnSubmit() {
        if (this.getSelectedPOI() == this._solution) {
            this.bombModule.HandlePass();
        } else {
            this.bombModule.HandleStrike();
        }

        return false;
    }

    void Update() {
        // Compute animation offsets
        int xOffset = this._x * (int) (this.speed * Time.deltaTime);
        int yOffset = this._y * (int) (this.speed * Time.deltaTime);

        // Translate X
        if (!this.screen.translatePx(xOffset, 0)) {
            this._x = 0;
        }

        // Translate Y
        if (!this.screen.translatePx(0, yOffset)) {
            this._y = 0;
        }

        // Determine flashing state
        if (this._x == 0 && this._y == 0 && isInRange()) {
            this._lastFlashDelay += Time.deltaTime;

            if (this._lastFlashDelay > this.flashTime) {
                this._flashState = !this._flashState;
                this._lastFlashDelay -= this.flashTime;
            }
        } else {
            this._flashState = true;
            this._lastFlashDelay = 0;
        }

        // Flash the recticle
        this.recticle.SetActive(this._flashState);
    }

    private bool isInRange() {
        return POINTS_OF_INTEREST_LOCATIONS.Any(loc =>
            Math.Sqrt(Math.Pow(this.screen.getAbsPosX() - loc.x, 2) + Math.Pow(this.screen.getAbsPosY() - loc.y, 2)) <
            recticleThreshold);
    }

    private int getSelectedPOI() {
        for (int i = 0; i < POINTS_OF_INTEREST_NAMES.Length; i++) {
            var loc = POINTS_OF_INTEREST_LOCATIONS[i];
            if (Math.Sqrt(Math.Pow(this.screen.getAbsPosX() - loc.x, 2) +
                Math.Pow(this.screen.getAbsPosY() - loc.y, 2)) < recticleThreshold) {
                return i;
            }
        }

        return -1;
    }
}
