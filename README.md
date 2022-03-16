# Arr DDA (Dyanmic Difficulty Adjustment) — The Smart Difficulty Adjustor

## __This plugin are created for educational purpose only__

Arr DDA (Dynamic Difficulty Adjustment) are a tool for developers and game designers to help them create a game with smart AI that can adjust the game difficulty

The purpose of this tool are :

* Helping Developer and Game Designers to create an AI that can adjust difficulty to the player behaviour easily
* Keeping the game fun by making all parameters used in the game all shown and can be controlled using graphical editor

## Current Features

* Easy way to manage multiple difficulties
* Graph Illustration to help game designer shows what will happen and what's happening inside the game
* Graphical Editor to help designers tweak the difficulties without touching and changing a single line of code

## Setup

* Download the necessary package from designated download
* Import the package into your Unity projects
* You can now start using the DDA

## Basic Usage Example

In this example, you can use DDA on a tile based game. We can use tile size and score as a base for the DDA. Let's start by right clicking in the project folder and create 3 metrics, score metric, target tiles metric, and tile size metric

![Right clicking on project folder](/img/right_click.png)

<p align="center">
  <img src="https://github.com/arrakh/ArrDDA/img/metric_score.png?raw=true" alt="Create Score metric"/>
</p>

![Create target tiles metric](/img/metric_targetsize.png)
![Create tile size metric](/img/metric_tilesize.png)

After that, we can create channels for both the target tiles and tile size. Start by right click on project folder and choose Channel

![Create channel target tile](/img/channel_targettile.png)
![Create channel tile size](/img/channel_tilesize.png)

Create empty gameobjects and attach 'GameManager.cs' inside the gameobjects

```cs
using System;
using System.Collections.Generic;
using Arr.DDA;
using Arr.DDA.Script;
using Arr.DDA.Script.Evaluators;
using UnityEngine;
using UnityEngine.UI;e
using Random = UnityEngine.Random;

namespace MockupGame
{
    public class GameManager : MonoBehaviour
    {
        public MetricObject Score;
        public MetricObject TargetTile;
        public MetricObject TileSize;
        public ChannelObject TargetChannel;
        public ChannelObject SizeChannel;

        public AdaptParameter Parameter = new AdaptParameter();
        
        public float timeLeft;

        [SerializeField] private GameObject tilePrefab;
        [SerializeField] private GridLayout gridLayout;
        [SerializeField] private Image timerFill;

        private List<Tile> tiles = new List<Tile>();
        private bool isPlaying = false;
        private float maxTime = 20f;
        private int currentTile = 0;

        private void Start()
        {
            TargetChannel.Initialize();
            SizeChannel.Initialize();

            GenerateGrid((int)TileSize.Value);
            timeLeft = maxTime;
            isPlaying = true;
        }

        private void Update()
        {
            TimerTick();
        }

        private void TimerTick()
        {
            if (!isPlaying) return;

            timeLeft -= Time.deltaTime;
            timerFill.fillAmount = timeLeft / maxTime;

            if (timeLeft < 0) OnRoundOver(false);
        }

        private void OnRoundOver(bool hasSucceded)
        {
            Score.Add(1);
            Parameter.isSuccess = hasSucceded;
            TargetChannel.Evaluate(Parameter);
            SizeChannel.Evaluate(Parameter);
            
            currentTile = 0;
            
            timeLeft = maxTime;
            GenerateGrid((int)TileSize.Value);
        }

        public void GenerateGrid(int size)
        {
            ClearTiles();

            for (int i = 0; i < size * size; i++)
            {
                var tile = Instantiate(tilePrefab, gridLayout.transform).GetComponent<Tile>();
                tile.OnClicked += OnTileClicked;
                tiles.Add(tile);
            }

            for (int i = 0; i < (int)TargetTile.Value; i++)
            {
                Tile randTile;

                do
                {
                    randTile = tiles[Random.Range(0, tiles.Count)];
                } while (randTile.IsOn);

                randTile.SetOn(true);
            }
        }

        private void ClearTiles()
        {
            foreach (var tile in tiles)
            {
                tile.OnClicked -= OnTileClicked;
                Destroy(tile.gameObject);
            }
        
            tiles.Clear();
        }
        
        private void OnTileClicked(Tile tile)
        {
            if (tile.IsOn)
            {
                tile.SetOn(false);
                currentTile++;
                if (currentTile >= (int)TargetTile.Value)
                    OnRoundOver(true);

            }
            else OnRoundOver(false);

        }
    }
}
```

Create simple clickable tiles with 'Tile.cs' attached

```cs
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MockupGame
{
    public class Tile : MonoBehaviour, IPointerClickHandler
    {
        public Action<Tile> OnClicked;
        public bool IsOn { get; private set; } = false;
        public Color onColor, offColor;
        public Image image;

        private int scaleTween = 0;
        public void OnPointerClick(PointerEventData eventData)
        {
            OnClicked?.Invoke(this);
            
            if(scaleTween != 0) LeanTween.cancel(scaleTween);
            transform.localScale = Vector3.one * 0.8f; 
            scaleTween = transform.LeanScale(Vector3.one, 0.25f)
                .setEase(LeanTweenType.easeOutBounce).uniqueId;
        }

        public void SetOn(bool on)
        {
            IsOn = on;
            image.color = on ? onColor : offColor;
        }
    }
}
```

Check on Channel scriptable objects to see how the game performs DDA live on your editor

![Output Target Tile](/img/target_tile_output.png)
![Output Tile Size](/img/tile_size_output.png)
![Output Overall](/img/output.png)

## Contribution & License

Want to improve ArrDDA? Feel free to email or message me if you want to use this for commercial purposes. This content is released under the '(<http://opensource.org/licenses/MIT>)' MIT License.
