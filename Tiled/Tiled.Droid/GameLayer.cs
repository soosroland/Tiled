using CocosDenshion;
using CocosSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Tiled.Droid.Entities;

namespace Tiled.Shared
{
    public class GameLayer : CCLayer
    {
        CCTileMap tileMap;
        CCEventListenerTouchAllAtOnce touchListener;
        Button button_left;
        Button button_right;
        Button button_up;
        Button button_down;
        Button button_special;
        Button treasure_key;
        Button chest_key;
        Treasure treasure;
        Cannon cannon;
        Wall wall;
        Door door;
        DoorKey doorkey;
        TreasureKey treasurekey;
        FreezeDrink freezedrink;
        ImmortalityDrink immortalitydrink;
        Coin coin;
        Finish finish;
        CharacterModel character;
        CharacterModel character_enemy;
        List<Entities> entities = new List<Entities>();
        List<CharacterModel> charmodel_List = new List<CharacterModel>();
        List<CharacterModel> charmodel_enemy_List = new List<CharacterModel>();
        List<Treasure> treasure_List = new List<Treasure>();
        List<Cannon> shooter_List = new List<Cannon>();
        List<Bullet> bullet_List;
        List<Wall> wall_List = new List<Wall>();
        List<Wall> shootingWall_List = new List<Wall>();
        List<Door> door_List = new List<Door>();
        List<DoorKey> doorkey_List = new List<DoorKey>();
        List<TreasureKey> treasurekey_List = new List<TreasureKey>();
        List<FreezeDrink> freezedrink_List = new List<FreezeDrink>();
        List<ImmortalityDrink> immortalitydrink_List = new List<ImmortalityDrink>();
        List<Coin> coin_List = new List<Coin>();
        List<Finish> finish_List = new List<Finish>();
        CCPoint character_place;
        CCLabel time_label;
        CCLabel door_key_label;
        CCLabel treasure_key_label;
        CCLabel health_point_label;
        CCLabel coin_count_label;
        //CCPoint character_enemy_place;
        const float timeToTake = 0.5f; // in seconds
        int rrow;
        int ccolumn;
        bool[,] walls;
        bool[,] doors;
        bool[,] finishes;
        int[] char_pos = new int[2];
        int[] char_pos_enemy1 = new int[2];
        static CCPoint center = new CCPoint(384 / 2, 240 / 2);
        CCPoint char_diff;
        String _level;
        MainLayer _mainLayer;
        DateTime date_start;
        int numberOfColumns;
        int numberOfRows;
        int door_key_count = 0;
        int treasure_key_count = 0;
        DateTime refresh_time;
        bool refreshed = true;
        TimeSpan temp;
        int starting_health_point;
        int starting_coin_count;
        int health_point;
        int coin_count;
        int immortality_count;
        String _speed;

        public CCLabel Time_label { get => time_label; set => time_label = value; }

        public GameLayer(MainLayer mainLayer, String level, int hp, String speed)
        {
            _level = level;
            _mainLayer = mainLayer;
            starting_health_point = hp;
            _speed = speed;
            Schedule(CollisionCheck, interval: 0.1f); //0.042f);
            Schedule(ShowTimer, interval: 1.0f);

            //CCSimpleAudioEngine.SharedEngine.PlayBackgroundMusic("sounds/backgroundmusic", true);
            //CCSimpleAudioEngine.SharedEngine.BackgroundMusicVolume = 0.3f;
        }
        protected override void AddedToScene()
        {
            base.AddedToScene();

            tileMap = new CCTileMap("tilemaps/dungeon_"+_level+".tmx");
            
            tileMap.Antialiased = false;

            this.AddChild(tileMap);

            HandleCustomTileProperties(tileMap);

            foreach (var shooter in shooter_List)
            {
                int actual_c = shooter.column;
                int actual_r = shooter.row;
                bool go = true;

                while (go)
                {
                    if ((actual_c >= 0) && (actual_r >= 0) && (actual_c < numberOfColumns) && (actual_r < numberOfRows))
                    {
                        if (walls[actual_c, actual_r] == true)
                        {
                            wall = new Wall("shooter_wall");
                            wall.PositionX = 16 * actual_c + 8;
                            wall.PositionY = 16 * actual_r + 8;
                            shootingWall_List.Add(wall);
                            this.AddChild(wall);
                            go = false;
                        }
                        if (shooter._rotation == 0)
                        {
                            actual_r++;
                        }
                        else if (shooter._rotation == 90)
                        {
                            actual_c++;
                        }
                        else if (shooter._rotation == 180)
                        {
                            actual_r--;
                        }
                        else if (shooter._rotation == 270)
                        {
                            actual_c--;
                        }
                    }
                    else
                    {
                        go = false;
                    }
                }
            }

            // Összes elem helyére tolása

            char_diff = character_place - center;
            tileMap.TileLayersContainer.Position -= new CCPoint(char_diff);

            foreach (var ce in charmodel_enemy_List)
            {
                ce.Position -= new CCPoint(char_diff);
            }

            foreach (var treasure in treasure_List)
            {
                treasure.Position -= new CCPoint(char_diff);
            }

            foreach (var shooter in shooter_List)
            {
                shooter.Position -= new CCPoint(char_diff);
            }

            foreach (var wall in shootingWall_List)
            {
                wall.Position -= new CCPoint(char_diff);
            }

            foreach (var door in door_List)
            {
                door.Position -= new CCPoint(char_diff);
            }

            foreach (var doorkey in doorkey_List)
            {
                doorkey.Position -= new CCPoint(char_diff);
            }

            foreach (var treasurekey in treasurekey_List)
            {
                treasurekey.Position -= new CCPoint(char_diff);
            }

            foreach (var freezedrink in freezedrink_List)
            {
                freezedrink.Position -= new CCPoint(char_diff);
            }

            foreach (var immortalitydrink in immortalitydrink_List)
            {
                immortalitydrink.Position -= new CCPoint(char_diff);
            }

            foreach (var coin in coin_List)
            {
                coin.Position -= new CCPoint(char_diff);
            }

            foreach (var finish in finish_List)
            {
                finish.Position -= new CCPoint(char_diff);
            }

            // Tolás vége

            // Register for touch events
            touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesBegan = HandleInput;
            AddEventListener(touchListener, this);

            Schedule(
               (dt) =>
               {
                   if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                   {
                       _mainLayer.BackToMenu();
                    }
               }

           );

            //Schedule();

            bullet_List = new List<Bullet>();
            BulletFactory.Self.BulletCreated += HandleBulletCreated;

            time_label = new CCLabel("0:0", "fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);
            time_label.Scale = 0.5f;
            time_label.Position = new CCPoint(15, 225);
            this.AddChild(time_label);

            date_start = DateTime.Now;
            refresh_time = DateTime.Now;

            door_key_label = new CCLabel(door_key_count.ToString(), "fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);
            door_key_label.Scale = 0.5f;
            door_key_label.Position = new CCPoint(378, 185);
            this.AddChild(door_key_label);

            treasure_key_label = new CCLabel(treasure_key_count.ToString(), "fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);
            treasure_key_label.Scale = 0.5f;
            treasure_key_label.Position = new CCPoint(378, 200);
            this.AddChild(treasure_key_label);

            treasure_key = new Button(365, 202, "treasure_key_open");
            this.AddChild(treasure_key);

            chest_key = new Button(365, 187, "door_key");
            this.AddChild(chest_key);


            button_left = new Button(20, 20, "arrow_left");

            this.AddChild(button_left);

            button_right = new Button(60, 20, "arrow_right");

            this.AddChild(button_right);

            button_up = new Button(360, 50, "arrow_up");

            this.AddChild(button_up);

            button_down = new Button(360, 20, "arrow_down");

            this.AddChild(button_down);

            button_special = new Button(300, 35, "special_button");
            this.AddChild(button_special);

            health_point = starting_health_point;
            health_point_label = new CCLabel("lives: "+health_point.ToString(), "fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);
            health_point_label.Scale = 0.5f;
            health_point_label.Position = new CCPoint(360, 230);
            this.AddChild(health_point_label);

            coin_count = 0;
            coin_count_label = new CCLabel("score: "+(coin_count).ToString(), "fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);
            coin_count_label.Scale = 0.5f;
            coin_count_label.Position = new CCPoint(360, 215);
            this.AddChild(coin_count_label);

            immortality_count = 0;
        }
        
        void ShowTimer(float unusedValue)
        {
            TimeSpan interval = DateTime.Now - date_start;
            TimeSpan slowed_time = DateTime.Now - refresh_time;
            if (temp == null)
            {
                temp = interval;
            }

            if (interval == slowed_time)
            {
                temp = slowed_time;
            }
            else if ((interval > slowed_time) && ( refreshed == false ))
            {
                temp = interval;
                refreshed = true;
            }
            else if (slowed_time > temp)
            {
                date_start = refresh_time;
                temp = slowed_time;
            }

            time_label.Text = temp.Minutes.ToString() + ":" + temp.Seconds.ToString();
            
        }
        void CollisionCheck(float unusedValue)
        {
            if (health_point <= 0)
            {
                _mainLayer.Died();
            }
            List<Coin> tempcoin = new List<Coin>();
            foreach (var coin in coin_List)
            {
                tempcoin.Add(coin);
            }
            foreach (var coin in tempcoin)
            {
                foreach (var characterModel in charmodel_List)
                {
                    if (coin.BoundingBoxTransformedToWorld.IntersectsRect(characterModel.BoundingBoxTransformedToParent))
                    {
                        //CCSimpleAudioEngine.SharedEngine.PlayEffect("sounds/coineffect");
                        RemoveChild(coin);
                        coin_List.Remove(coin);
                        coin_count++;
                        coin_count_label.Text = "score: " + (coin_count).ToString();
                    }
                }
            }
            List<Bullet> temp_List = new List<Bullet>();
            foreach (var bullet in bullet_List)
            {
                temp_List.Add(bullet);
            }
            foreach (var bullet in temp_List)
            {
                foreach (var characterModel in charmodel_List)
                {
                    if (bullet.sprite.BoundingBoxTransformedToWorld.IntersectsRect(characterModel.BoundingBoxTransformedToParent))
                    {
                        RemoveChild(bullet);
                        bullet_List.Remove(bullet);
                        if (immortality_count == 0)
                        {
                            health_point--;
                            health_point_label.Text = "lives: " + health_point.ToString();
                        }
                        else if(immortality_count == 1)
                        {
                            foreach (var charmodel in charmodel_List)
                            {
                                charmodel.UnShield();
                                charmodel.ChangeSprite();
                            }
                            immortality_count--;
                        }
                        else
                        {
                            immortality_count--;
                        }
                    }
                }
                foreach (var wall in shootingWall_List)
                {
                    if (bullet.sprite.BoundingBoxTransformedToWorld.IntersectsRect(wall.BoundingBoxTransformedToWorld))
                    {
                        RemoveChild(bullet);
                        bullet_List.Remove(bullet);
                    }
                }
            }
            List<TreasureKey> temptreasure = new List<TreasureKey>();
            foreach (var treasurekey in treasurekey_List)
            {
                temptreasure.Add(treasurekey);
            }
            foreach (var treasurekey in temptreasure)
            {
                foreach (var characterModel in charmodel_List)
                {
                    if (treasurekey.BoundingBoxTransformedToWorld.IntersectsRect(characterModel.BoundingBoxTransformedToParent))
                    {
                        RemoveChild(treasurekey);
                        treasurekey_List.Remove(treasurekey);
                        treasure_key_count++;
                        treasure_key_label.Text = treasure_key_count.ToString();
                    }
                }
            }
            List<DoorKey> tempdoor = new List<DoorKey>();
            foreach (var doorkey in doorkey_List)
            {
                tempdoor.Add(doorkey);
            }
            foreach (var doorkey in tempdoor)
            {
                foreach (var characterModel in charmodel_List)
                {
                    if (doorkey.BoundingBoxTransformedToWorld.IntersectsRect(characterModel.BoundingBoxTransformedToParent))
                    {
                        RemoveChild(doorkey);
                        doorkey_List.Remove(doorkey);
                        door_key_count++;
                        door_key_label.Text = door_key_count.ToString();
                    }
                }
            }

            List<FreezeDrink> tempfreezedrink = new List<FreezeDrink>();
            foreach (var freezedrink in freezedrink_List)
            {
                tempfreezedrink.Add(freezedrink);
            }
            foreach (var freezedrink in tempfreezedrink)
            {
                foreach (var characterModel in charmodel_List)
                {
                    if (freezedrink.BoundingBoxTransformedToWorld.IntersectsRect(characterModel.BoundingBoxTransformedToParent))
                    {
                        RemoveChild(freezedrink);
                        freezedrink_List.Remove(freezedrink);
                        // Stop time
                        refresh_time = date_start.AddSeconds(10.0);
                        refreshed = false;
                    }
                }
            }

            List<ImmortalityDrink> tempimmortalitydrink = new List<ImmortalityDrink>();
            foreach (var immortalitydrink in immortalitydrink_List)
            {
                tempimmortalitydrink.Add(immortalitydrink);
            }
            foreach (var immortalitydrink in tempimmortalitydrink)
            {
                foreach (var characterModel in charmodel_List)
                {
                    if (immortalitydrink.BoundingBoxTransformedToWorld.IntersectsRect(characterModel.BoundingBoxTransformedToParent))
                    {
                        RemoveChild(immortalitydrink);
                        immortalitydrink_List.Remove(immortalitydrink);
                        // No damage for X collision
                        foreach (var charmodel in charmodel_List)
                        {
                            charmodel.Shield();
                            charmodel.ChangeSprite();
                        }
                        immortality_count += 5;
                    }
                }
            }
        }
        
        void HandleCustomTileProperties(CCTileMap tileMap)
        {
            // Width and Height are equal so we can use either
            int tileDimension = (int)tileMap.TileTexelSize.Width;

            // Find out how many rows and columns are in our tile map
            numberOfColumns = (int)tileMap.MapDimensions.Size.Width;
            numberOfRows = (int)tileMap.MapDimensions.Size.Height;

            walls = new bool[numberOfColumns, numberOfRows];
            doors = new bool[numberOfColumns, numberOfRows];
            finishes = new bool[numberOfColumns, numberOfRows];
            for (int i = 0; i < numberOfColumns; i++)
            {
                for (int j = 0; j < numberOfRows; j++)
                {
                    walls[i, j] = false;
                    doors[i, j] = false;
                    finishes[i, j] = false;
                }
            }

            // Tile maps can have multiple layers, so let's loop through all of them:
            foreach (CCTileMapLayer layer in tileMap.TileLayersContainer.Children)
            {
                // Loop through the columns and rows to find all tiles
                for (int column = 0; column < numberOfColumns; column++)
                {
                    int worldX = tileDimension * column + tileDimension / 2;
                    for (int row = 0; row < numberOfRows; row++)
                    {
                        // See above on why we add tileDimension / 2
                        int worldY = tileDimension * row + tileDimension / 2;
                        ccolumn = column;
                        rrow = row;
                        HandleCustomTilePropertyAt(worldX, worldY, layer);
                    }
                }
            }
        }

        void HandleCustomTilePropertyAt(int worldX, int worldY, CCTileMapLayer layer)
        {
            CCTileMapCoordinates tileAtXy = layer.ClosestTileCoordAtNodePosition(new CCPoint(worldX, worldY));

            CCTileGidAndFlags info = layer.TileGIDAndFlags(tileAtXy.Column, tileAtXy.Row);

            if (info != null)
            {
                Dictionary<string, string> properties = null;

                try
                {
                    properties = tileMap.TilePropertiesForGID(info.Gid);
                }
                catch
                {
                    // CocosSharp 
                }

                if (properties != null && properties.ContainsKey("IsTreasure") && properties["IsTreasure"] == "true")
                {
                    layer.RemoveTile(tileAtXy);

                    // Create a Treasure Chest Entity
                    treasure = new Treasure("treasure");
                    treasure.PositionX = worldX;
                    treasure.PositionY = worldY;

                    this.AddChild(treasure);
                    treasure_List.Add(treasure);

                }
                else if (properties != null && properties.ContainsKey("isFinish") && properties["isFinish"] == "true")
                {
                    // Create Finish Entity

                    finishes[ccolumn, rrow] = true;

                    layer.RemoveTile(tileAtXy);
                    finish = new Finish();
                    finish.PositionX = worldX;
                    finish.PositionY = worldY;
                    finish_List.Add(finish);

                    this.AddChild(finish);
                }
                else if (properties != null && properties.ContainsKey("isCharacter") && properties["isCharacter"] == "true" && properties.ContainsKey("isEnemy") && properties["isEnemy"] == "false")
                {
                    layer.RemoveTile(tileAtXy);

                    // Create a Character Entity
                    character = new CharacterModel("pplayer_1_", properties["part"]);
                    if ((properties["part"] == "1") || (properties["part"] == "3"))
                    {
                        character.PositionX = 184;
                    }
                    else
                    {
                        character.PositionX = 200;
                    }
                    if ((properties["part"] == "1") || (properties["part"] == "2"))
                    {
                        character.PositionY = 128;
                    }
                    else
                    {
                        character.PositionY = 112;
                    }
                    this.AddChild(character);
                    charmodel_List.Add(character);
                    if (properties["part"] == "1")
                    {
                        character_place = new CCPoint(worldX + 8, worldY - 8);
                        char_pos[0] = ccolumn;
                        char_pos[1] = rrow - 1;
                    }
                }
                else if (properties != null && properties.ContainsKey("isWall") && properties["isWall"] == "true")
                {
                    // Create Wall
                    walls[ccolumn, rrow] = true;

                    // Create Wall Entity

                    wall = new Wall();

                    wall.PositionX = worldX;
                    wall.PositionY = worldY;
                    wall_List.Add(wall);

                    this.AddChild(wall);
                }
                else if (properties != null && properties.ContainsKey("isCharacter") && properties["isCharacter"] == "true" && properties.ContainsKey("isEnemy") && properties["isEnemy"] == "true")
                {
                    // Create Enemy Entity

                    layer.RemoveTile(tileAtXy);

                    character_enemy = new CharacterModel("pplayer_2_", properties["part"]);

                    character_enemy.PositionX = worldX;
                    character_enemy.PositionY = worldY;

                    this.AddChild(character_enemy);
                    charmodel_enemy_List.Add(character_enemy);
                }
                else if (properties != null && properties.ContainsKey("isShooting") && properties["isShooting"] == "true")
                {
                    // Create Shooting Entity

                    layer.RemoveTile(tileAtXy);
                    int x = 0;
                    int y = 0;
                    if (properties["dir"] == "up")
                    {
                        x = 0;
                        y = 20;
                    }
                    else if (properties["dir"] == "right")
                    {
                        x = 20;
                        y = 0;
                    }
                    else if (properties["dir"] == "down")
                    {
                        x = 0;
                        y = -20;
                    }
                    else if (properties["dir"] == "left")
                    {
                        x = -20;
                        y = 0;
                    }

                    cannon = new Cannon(x, y, properties["dir"], _speed);

                    cannon.PositionX = worldX;
                    cannon.PositionY = worldY;

                    cannon.row = rrow;
                    cannon.column = ccolumn;

                    this.AddChild(cannon);
                    shooter_List.Add(cannon);
                }
                else if (properties != null && properties.ContainsKey("isDoor") && properties["isDoor"] == "true")
                {
                    layer.RemoveTile(tileAtXy);

                    // Create Door
                    doors[ccolumn, rrow] = true;

                    // Create Door Entity

                    String s = "door_";
                    if (properties["dir"] == "up")
                    {
                        s += "up";
                    }
                    else if (properties["dir"] == "right")
                    {
                        s += "right";
                    }
                    else if (properties["dir"] == "down")
                    {
                        s += "down";
                    }
                    else if (properties["dir"] == "left")
                    {
                        s += "left";
                    }
                    if (properties["isOpen"] == "false")
                    {
                        walls[ccolumn, rrow] = true;
                    }
                    door = new Door(s, properties["isOpen"], properties["part"]);

                    door.PositionX = worldX;
                    door.PositionY = worldY;

                    this.AddChild(door);
                    door_List.Add(door);
                }
                else if (properties != null && properties.ContainsKey("isKey") && properties["isKey"] == "true")
                {
                    // Create Key Entity
                    if(properties["type"] == "door")
                    {
                        layer.RemoveTile(tileAtXy);
                        doorkey = new DoorKey();
                        doorkey.PositionX = worldX;
                        doorkey.PositionY = worldY;
                        doorkey_List.Add(doorkey);

                        this.AddChild(doorkey);
                    }
                    else if (properties["type"] == "treasure")
                    {
                        layer.RemoveTile(tileAtXy);
                        treasurekey = new TreasureKey();
                        treasurekey.PositionX = worldX;
                        treasurekey.PositionY = worldY;
                        treasurekey_List.Add(treasurekey);

                        this.AddChild(treasurekey);
                    }
                }
                else if (properties != null && properties.ContainsKey("isDrink") && properties["isDrink"] == "true")
                {
                    // Create Freeze Drink Entity
                    if (properties["type"] == "freeze")
                    {
                        layer.RemoveTile(tileAtXy);
                        freezedrink = new FreezeDrink();
                        freezedrink.PositionX = worldX;
                        freezedrink.PositionY = worldY;
                        freezedrink_List.Add(freezedrink);

                        this.AddChild(freezedrink);
                    }
                    else if (properties["type"] == "immortality")
                    {
                        layer.RemoveTile(tileAtXy);
                        immortalitydrink = new ImmortalityDrink();
                        immortalitydrink.PositionX = worldX;
                        immortalitydrink.PositionY = worldY;
                        immortalitydrink_List.Add(immortalitydrink);

                        this.AddChild(immortalitydrink);
                    }
                }
                else if (properties != null && properties.ContainsKey("isCoin") && properties["isCoin"] == "true")
                {
                    // Create Coin Entity

                    layer.RemoveTile(tileAtXy);
                    coin = new Coin();
                    coin.PositionX = worldX;
                    coin.PositionY = worldY;
                    coin_List.Add(coin);
                    starting_coin_count++;

                    this.AddChild(coin);
                }
            }
        }

        private void HandleInput(System.Collections.Generic.List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count > 0)
            {
                foreach (var touch in touches)
                {
                    if (button_right.sprite.BoundingBoxTransformedToWorld.ContainsPoint(touch.Location))
                    {
                        if (walls[char_pos[0] + 2, char_pos[1]] == false)
                        {
                            if(finishes[char_pos[0] + 2, char_pos[1]] == true)
                            {
                                _mainLayer.Victory(time_label.Text, _level, starting_coin_count-coin_count, starting_health_point-health_point);
                            }
                            int move_unit;
                            if (doors[char_pos[0] + 2, char_pos[1]] == true)
                            {
                                move_unit = -64;
                                char_pos[0] = char_pos[0] + 4;
                            }
                            else
                            {
                                move_unit = -16;
                                char_pos[0] = char_pos[0] + 1;
                            }
                            tileMap.TileLayersContainer.Position += new CCPoint(move_unit, 0);

                            foreach (var cme in charmodel_enemy_List)
                            {
                                cme.MoveX(move_unit);
                            }

                            foreach (var treasure in treasure_List)
                            {
                                treasure.MoveX(move_unit);
                            }

                            foreach (var bullet in bullet_List)
                            {
                                bullet.MoveX(move_unit);
                            }
                            foreach (var shooter in shooter_List)
                            {
                                shooter.MoveX(move_unit);
                            }
                            foreach (var wall in shootingWall_List)
                            {
                                wall.MoveX(move_unit);    
                            }
                            foreach (var door in door_List)
                            {
                                door.MoveX(move_unit);
                            }
                            foreach (var doorkey in doorkey_List)
                            {
                                doorkey.MoveX(move_unit);
                            }

                            foreach (var treasurekey in treasurekey_List)
                            {
                                treasurekey.MoveX(move_unit);
                            }

                            foreach (var freezedrink in freezedrink_List)
                            {
                                freezedrink.MoveX(move_unit);
                            }

                            foreach (var immortalitydrink in immortalitydrink_List)
                            {
                                immortalitydrink.MoveX(move_unit);
                            }

                            foreach (var coin in coin_List)
                            {
                                coin.MoveX(move_unit);
                            }
                            foreach (var finish in finish_List)
                            {
                                finish.MoveX(move_unit);
                            }
                        }
                    }

                    else if (button_left.sprite.BoundingBoxTransformedToWorld.ContainsPoint(touch.Location))
                    {
                        if (walls[char_pos[0] - 1, char_pos[1]] == false)
                        {
                            if (finishes[char_pos[0] - 1, char_pos[1]] == true)
                            {
                                _mainLayer.Victory(time_label.Text, _level, starting_coin_count - coin_count, starting_health_point - health_point);
                            }
                            int move_unit;
                            if (doors[char_pos[0] - 1, char_pos[1]] == true)
                            {
                                move_unit = 64;
                                char_pos[0] = char_pos[0] - 4;
                            }
                            else
                            {
                                move_unit = 16;
                                char_pos[0] = char_pos[0] - 1;
                            }
                            tileMap.TileLayersContainer.Position += new CCPoint(move_unit, 0);

                            foreach (var cme in charmodel_enemy_List)
                            {
                                cme.MoveX(move_unit);
                            }

                            foreach (var treasure in treasure_List)
                            {
                                treasure.MoveX(move_unit);
                            }

                            foreach (var bullet in bullet_List)
                            {
                                bullet.MoveX(move_unit);
                            }

                            foreach (var shooter in shooter_List)
                            {
                                shooter.MoveX(move_unit);
                            }
                            foreach (var wall in shootingWall_List)
                            {
                                wall.MoveX(move_unit);
                            }
                            foreach (var door in door_List)
                            {
                                door.MoveX(move_unit);
                            }
                            foreach (var doorkey in doorkey_List)
                            {
                                doorkey.MoveX(move_unit);
                            }

                            foreach (var treasurekey in treasurekey_List)
                            {
                                treasurekey.MoveX(move_unit);
                            }

                            foreach (var freezedrink in freezedrink_List)
                            {
                                freezedrink.MoveX(move_unit);
                            }

                            foreach (var immortalitydrink in immortalitydrink_List)
                            {
                                immortalitydrink.MoveX(move_unit);
                            }

                            foreach (var coin in coin_List)
                            {
                                coin.MoveX(move_unit);
                            }
                            foreach (var finish in finish_List)
                            {
                                finish.MoveX(move_unit);
                            }
                        }
                    }

                    else if (button_up.sprite.BoundingBoxTransformedToWorld.ContainsPoint(touch.Location))
                    {
                        if ((walls[char_pos[0], char_pos[1] + 1] == false) && (walls[char_pos[0] + 1, char_pos[1] + 1] == false))
                        {
                            if (finishes[char_pos[0], char_pos[1] + 1] == true)
                            {
                                _mainLayer.Victory(time_label.Text, _level, starting_coin_count - coin_count, starting_health_point - health_point);
                            }
                            int move_unit;
                            if ((doors[char_pos[0], char_pos[1] + 1] == true) && (doors[char_pos[0] + 1, char_pos[1] + 1] == true))
                            {
                                move_unit = -64;
                                char_pos[1] = char_pos[1] + 4;
                            }
                            else
                            {
                                move_unit = -16;
                                char_pos[1] = char_pos[1] + 1;
                            }
                            tileMap.TileLayersContainer.Position += new CCPoint(0, move_unit);
                            
                            foreach (var cme in charmodel_enemy_List)
                            {
                                cme.MoveY(move_unit);
                            }

                            foreach (var treasure in treasure_List)
                            {
                                treasure.MoveY(move_unit);
                            }

                            foreach (var bullet in bullet_List)
                            {
                                bullet.MoveY(move_unit);
                            }

                            foreach (var shooter in shooter_List)
                            {
                                shooter.MoveY(move_unit);
                            }
                            foreach (var wall in shootingWall_List)
                            {
                                wall.MoveY(move_unit);
                            }
                            foreach (var door in door_List)
                            {
                                door.MoveY(move_unit);
                            }
                            foreach (var doorkey in doorkey_List)
                            {
                                doorkey.MoveY(move_unit);
                            }

                            foreach (var treasurekey in treasurekey_List)
                            {
                                treasurekey.MoveY(move_unit);
                            }
                            foreach (var freezedrink in freezedrink_List)
                            {
                                freezedrink.MoveY(move_unit);
                            }

                            foreach (var immortalitydrink in immortalitydrink_List)
                            {
                                immortalitydrink.MoveY(move_unit);
                            }

                            foreach (var coin in coin_List)
                            {
                                coin.MoveY(move_unit);
                            }
                            foreach (var finish in finish_List)
                            {
                                finish.MoveY(move_unit);
                            }
                        }
                    }

                    else if (button_down.sprite.BoundingBoxTransformedToWorld.ContainsPoint(touch.Location))
                    {
                        if (walls[char_pos[0], char_pos[1] - 1] == false)
                        {
                            if (finishes[char_pos[0], char_pos[1] - 1] == true)
                            {
                                _mainLayer.Victory(time_label.Text, _level, starting_coin_count - coin_count, starting_health_point - health_point);
                            }
                            int move_unit;
                            if (doors[char_pos[0], char_pos[1] - 1] == true)
                            {
                                move_unit = 64;
                                char_pos[1] = char_pos[1] - 4;
                            }
                            else
                            {
                                move_unit = 16;
                                char_pos[1] = char_pos[1] - 1;
                            }
                            tileMap.TileLayersContainer.Position += new CCPoint(0, move_unit);
                            
                            foreach (var cme in charmodel_enemy_List)
                            {
                                cme.MoveY(move_unit);
                            }

                            foreach (var treasure in treasure_List)
                            {
                                treasure.MoveY(move_unit);
                            }
                            foreach (var bullet in bullet_List)
                            {
                                bullet.MoveY(move_unit);
                            }

                            foreach (var shooter in shooter_List)
                            {
                                shooter.MoveY(move_unit);
                            }
                            foreach (var wall in shootingWall_List)
                            {
                                wall.MoveY(move_unit);
                            }
                            foreach (var door in door_List)
                            {
                                door.MoveY(move_unit);
                            }
                            foreach (var doorkey in doorkey_List)
                            {
                                doorkey.MoveY(move_unit);
                            }

                            foreach (var treasurekey in treasurekey_List)
                            {
                                treasurekey.MoveY(move_unit);
                            }
                            foreach (var freezedrink in freezedrink_List)
                            {
                                freezedrink.MoveY(move_unit);
                            }

                            foreach (var immortalitydrink in immortalitydrink_List)
                            {
                                immortalitydrink.MoveY(move_unit);
                            }

                            foreach (var coin in coin_List)
                            {
                                coin.MoveY(move_unit);
                            }
                            foreach (var finish in finish_List)
                            {
                                finish.MoveY(move_unit);
                            }
                        }
                    }

                    else if (button_special.sprite.BoundingBoxTransformedToWorld.ContainsPoint(touch.Location))
                    {
                        //bal alsó negyed
                        bool opened = false;
                        CCPoint location = new CCPoint(charmodel_List[0].PositionX - 16, charmodel_List[0].PositionY);
                        foreach (var treasure in treasure_List)
                        {
                            if (treasure.BoundingBoxTransformedToWorld.ContainsPoint(location))
                            {
                                if (treasure_key_count > 0)
                                {
                                    treasure_key_count--;
                                    treasure_key_label.Text = treasure_key_count.ToString();
                                    treasure.Interact();
                                }
                            }
                        }
                        foreach (var door in door_List)
                        {
                            if (door.BoundingBoxTransformedToWorld.ContainsPoint(location))
                            {
                                if(door_key_count > 0)
                                {
                                    door.Open();
                                    opened = true;
                                    walls[char_pos[0] - 1, char_pos[1]] = false;
                                }
                            }
                        }
                        location = new CCPoint(charmodel_List[0].PositionX, charmodel_List[0].PositionY - 16);
                        foreach (var treasure in treasure_List)
                        {
                            if (treasure.BoundingBoxTransformedToWorld.ContainsPoint(location))
                            {
                                if (treasure_key_count > 0)
                                {
                                    treasure_key_count--;
                                    treasure_key_label.Text = treasure_key_count.ToString();
                                    treasure.Interact();
                                }
                            }
                        }
                        foreach (var door in door_List)
                        {
                            if (door.BoundingBoxTransformedToWorld.ContainsPoint(location))
                            {
                                if (door_key_count > 0)
                                {
                                    door.Open();
                                    opened = true;
                                    walls[char_pos[0], char_pos[1] - 1] = false;
                                }
                            }
                        }

                        //bal felső negyed
                        location = new CCPoint(charmodel_List[1].PositionX - 16, charmodel_List[1].PositionY);
                        foreach (var treasure in treasure_List)
                        {
                            if (treasure.BoundingBoxTransformedToWorld.ContainsPoint(location))
                            {
                                if (treasure_key_count > 0)
                                {
                                    treasure_key_count--;
                                    treasure_key_label.Text = treasure_key_count.ToString();
                                    treasure.Interact();
                                }
                            }
                        }
                        foreach (var door in door_List)
                        {
                            if (door.BoundingBoxTransformedToWorld.ContainsPoint(location))
                            {
                                if (door_key_count > 0)
                                {
                                    door.Open();
                                    opened = true;
                                    walls[char_pos[0] - 1, char_pos[1]] = false;
                                }
                            }
                        }
                        location = new CCPoint(charmodel_List[1].PositionX, charmodel_List[1].PositionY);
                        foreach (var treasure in treasure_List)
                        {
                            if (treasure.BoundingBoxTransformedToWorld.ContainsPoint(location))
                            {
                                if (treasure_key_count > 0)
                                {
                                    treasure_key_count--;
                                    treasure_key_label.Text = treasure_key_count.ToString();
                                    treasure.Interact();
                                }
                            }
                        }
                        foreach (var door in door_List)
                        {
                            if (door.BoundingBoxTransformedToWorld.ContainsPoint(location))
                            {
                                if (door_key_count > 0)
                                {
                                    door.Open();
                                    opened = true;
                                    walls[char_pos[0], char_pos[1] + 1] = false;
                                }
                            }
                        }
                        location = new CCPoint(charmodel_List[1].PositionX, charmodel_List[1].PositionY + 16);
                        foreach (var door in door_List)
                        {
                            if (door.BoundingBoxTransformedToWorld.ContainsPoint(location))
                            {
                                if (door_key_count > 0)
                                {
                                    door.Open();
                                    opened = true;
                                    walls[char_pos[0], char_pos[1] + 2] = false;
                                }
                            }
                        }

                        //jobb alsó negyed
                        location = new CCPoint(charmodel_List[2].PositionX, charmodel_List[2].PositionY - 16);
                        foreach (var treasure in treasure_List)
                        {
                            if (treasure.BoundingBoxTransformedToWorld.ContainsPoint(location))
                            {
                                if (treasure_key_count > 0)
                                {
                                    treasure_key_count--;
                                    treasure_key_label.Text = treasure_key_count.ToString();
                                    treasure.Interact();
                                }
                            }
                        }
                        foreach (var door in door_List)
                        {
                            if (door.BoundingBoxTransformedToWorld.ContainsPoint(location))
                            {
                                if (door_key_count > 0)
                                {
                                    door.Open();
                                    opened = true;
                                    walls[char_pos[0], char_pos[1] - 1] = false;
                                }
                            }
                        }
                        location = new CCPoint(charmodel_List[2].PositionX + 16, charmodel_List[2].PositionY);
                        foreach (var treasure in treasure_List)
                        {
                            if (treasure.BoundingBoxTransformedToWorld.ContainsPoint(location))
                            {
                                if (treasure_key_count > 0)
                                {
                                    treasure_key_count--;
                                    treasure_key_label.Text = treasure_key_count.ToString();
                                    treasure.Interact();
                                }
                            }
                        }
                        foreach (var door in door_List)
                        {
                            if (door.BoundingBoxTransformedToWorld.ContainsPoint(location))
                            {
                                if (door_key_count > 0)
                                {
                                    door.Open();
                                    opened = true;
                                    walls[char_pos[0] + 2, char_pos[1]] = false;
                                }
                            }
                        }

                        //jobb felső negyed
                        location = new CCPoint(charmodel_List[3].PositionX + 16, charmodel_List[3].PositionY);
                        foreach (var treasure in treasure_List)
                        {
                            if (treasure.BoundingBoxTransformedToWorld.ContainsPoint(location))
                            {
                                if (treasure_key_count > 0)
                                {
                                    treasure_key_count--;
                                    treasure_key_label.Text = treasure_key_count.ToString();
                                    treasure.Interact();
                                }
                            }
                        }
                        foreach (var door in door_List)
                        {
                            if (door.BoundingBoxTransformedToWorld.ContainsPoint(location))
                            {
                                if (door_key_count > 0)
                                {
                                    door.Open();
                                    opened = true;
                                    walls[char_pos[0] + 2, char_pos[1]] = false;
                                }
                            }
                        }
                        location = new CCPoint(charmodel_List[3].PositionX, charmodel_List[3].PositionY);
                        foreach (var treasure in treasure_List)
                        {
                            if (treasure.BoundingBoxTransformedToWorld.ContainsPoint(location))
                            {
                                if (treasure_key_count > 0)
                                {
                                    treasure_key_count--;
                                    treasure_key_label.Text = treasure_key_count.ToString();
                                    treasure.Interact();
                                }
                            }
                        }
                        foreach (var door in door_List)
                        {
                            if (door.BoundingBoxTransformedToWorld.ContainsPoint(location))
                            {
                                if (door_key_count > 0)
                                {
                                    door.Open();
                                    opened = true;
                                    walls[char_pos[0] + 1, char_pos[1] + 1] = false;
                                }
                            }
                        }
                        location = new CCPoint(charmodel_List[3].PositionX, charmodel_List[3].PositionY + 16);
                        foreach (var door in door_List)
                        {
                            if (door.BoundingBoxTransformedToWorld.ContainsPoint(location))
                            {
                                if (door_key_count > 0)
                                {
                                    door.Open();
                                    opened = true;
                                    walls[char_pos[0] + 1, char_pos[1] + 2] = false;
                                }
                            }
                        }
                        if (opened)
                        {
                            door_key_count--;
                            door_key_label.Text = door_key_count.ToString();
                        }
                    }
                }
            }
        }
        void HandleBulletCreated(Bullet newBullet)
        {
            AddChild(newBullet);
            bullet_List.Add(newBullet);
        }
    }
}