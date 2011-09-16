using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Microsoft.Win32;

class Menu {
  bool root;
  string name;

  List<MenuItem> menuItems;

  public Menu(string name) {
    menuItems = new List<MenuItem>();
    if (name=="root") {
      root = true;
      menuItems.add(new MenuItem("games", "games.jpg", "gamesselected.jpg", menuItems.length, this));
      menuItems.add(new MenuItem("movies", "movies.jpg", "moviesselected.jpg", menuItems.length, this));
      menuItems.add(new MenuItem("music", "music.jpg", "musicselected.jpg", menuItems.length, this));
      menuItems.add(new MenuItem("apps", "apps.jpg", "appsselected.jpg", menuItems.length, this));
      menuItems.add(new MenuItem("settings", "settings.jpg", "settingsselected.jpg", menuItems.length, this));
    } else if (name == "games") {
      menuItems.add(new MenuItem("angrybirds", "angrybirds.jpg", "angrybirdsselected.jpg", menuItems.length, this));
      menuItems.add(new MenuItem("carnivalgames", "carnivalgames.jpg", "carnivalgamesselected.jpg", menuItems.length, this));
      menuItems.add(new MenuItem("dancecentral", "dancecentral.jpg", "dancecentralselected.jpg", menuItems.length, this));
      menuItems.add(new MenuItem("kinectimals", "kinectimals.jpg", "kinectimalsselected.jpg", menuItems.length, this));
      menuItems.add(new MenuItem("kinectsports", "kinectsports.jpg", "kinectsportsselected.jpg", menuItems.length, this));
      menuItems.add(new MenuItem("kungfupanda", "kungfupanda.jpg", "kungfupandaselected.jpg", menuItems.length, this));
    } else if (name == "apps") {
      menuItems.add(new MenuItem("aim", "aim.jpg", "aimselected.jpg", menuItems.length, this));
      menuItems.add(new MenuItem("facebook", "facebook.jpg", "facebookselected.jpg", menuItems.length, this));
      menuItems.add(new MenuItem("googleplus", "googleplus.jpg", "googleplusselected.jpg", menuItems.length, this));
      menuItems.add(new MenuItem("twitter", "twitter.jpg", "twitterselected.jpg", menuItems.length, this));
      menuItems.add(new MenuItem("yelp", "yelp.jpg", "yelpselected.jpg", menuItems.length, this));
    } else if (name == "movies") {
      menuItems.add(new MenuItem("anniehall", "anniehall.jpg", "anniehallselected.jpg", menuItems.length, this));
      menuItems.add(new MenuItem("emperorsnewgroove", "emperorsnewgroove.jpg", "emperorsnewgrooveselected.jpg", menuItems.length, this));
      menuItems.add(new MenuItem("hitchhikersguide", "hitchhikersguide.jpg", "hitchhikersguideselected.jpg", menuItems.length, this));
      menuItems.add(new MenuItem("oldyeller", "oldyeller.jpg", "oldyellerselected.jpg", menuItems.length, this));
      menuItems.add(new MenuItem("thematrix", "thematrix.jpg", "thematrixselected.jpg", menuItems.length, this));
    } else if (name == "music") {
      menuItems.add(new MenuItem("abbeyroad", "abbeyroad.jpg", "abbeyroadselected.jpg", menuItems.length, this));
      menuItems.add(new MenuItem("darksideofthemoon", "darksideofthemoon.jpg", "darksideofthemoonselected.jpg", menuItems.length, this));
      menuItems.add(new MenuItem("nevermind", "nevermind.jpg", "nevermindselected.jpg", menuItems.length, this));
      menuItems.add(new MenuItem("rollingstones", "rollingstones.jpg", "rollingstonesselected.jpg", menuItems.length, this));
    } else if (name == "abbeyroad") {
      menuItems.add(new MenuItem("Come Together", menuItems.length));
      menuItems.add(new MenuItem("Something", menuItems.length));
      menuItems.add(new MenuItem("Maxwells Silver Hammer", menuItems.length));
      menuItems.add(new MenuItem("Oh! Darling", menuItems.length));
      menuItems.add(new MenuItem("Octopus Garden", menuItems.length));
      menuItems.add(new MenuItem("I Want You", menuItems.length));
    } else if (name == "darksideofthemoon") {
      menuItems.add(new MenuItem("Speak to Me", menuItems.length));
      menuItems.add(new MenuItem("Breathe", menuItems.length));
      menuItems.add(new MenuItem("On the Run", menuItems.length));
      menuItems.add(new MenuItem("Money", menuItems.length));
      menuItems.add(new MenuItem("Any Colour You Like", menuItems.length));
      menuItems.add(new MenuItem("Brain Damage", menuItems.length));
      menuItems.add(new MenuItem("Eclipse", menuItems.length));
    } else if (name == "nevermind") {
      menuItems.add(new MenuItem("Smells Like Teen Spirit", menuItems.length));
      menuItems.add(new MenuItem("In Bloom", menuItems.length));
      menuItems.add(new MenuItem("Come as You Are", menuItems.length));
      menuItems.add(new MenuItem("Lithium", menuItems.length));
      menuItems.add(new MenuItem("On a Plain", menuItems.length));
      menuItems.add(new MenuItem("Endless, Nameless", menuItems.length));
    } else if (name == "rollingstones") {
      menuItems.add(new MenuItem("Rocks Off", menuItems.length));
      menuItems.add(new MenuItem("Shake Your Hips", menuItems.length));
      menuItems.add(new MenuItem("Casino Boogie", menuItems.length));
      menuItems.add(new MenuItem("Turd on the Run", menuItems.length));
      menuItems.add(new MenuItem("Let it Loose", menuItems.length));
      menuItems.add(new MenuItem("Soul Survivor", menuItems.length));
    } else {
      // huh?
    }

    if (!root) {
      backButton = new MenuItem("back", "back.jpg", "backselected.jpg", menuItems.length, this);
      menuItems.add(backButton);
    }
  }
}

class MenuItem
{
  static int WIDTH = 226;
  static int HEIGHT = 160;

  static int ROWHEIGHT = 200;
  static int COLWIDTH = 266;

  static int MARGIN = 20;

  string name;
  string icon;
  string selectedIcon;
  int order;
  Point upperLeft;
  bool selected;
  Menu previousMenu;

  public MenuItem(string name, int order) {
    this.name = name;
    this.order = order;
    this.selected = false;
    chooseDrawLocation;
  }

  public MenuItem(string name, string icon, string selectedIcon, int order, Menu previousMenu) {}
    this.name = name;
    this.icon = icon;
    this.selectedIcon = selectedIcon;
    this.order = order;
    this.previousMenu = previousMenu;
    this.selected = false;
    chooseDrawLocation();
  }

  private void chooseDrawLocation() {
    // select a reasonable location to draw
    // based on the order in the menu list
    this.upperLeft = new Point();
    upperLeft.Y = ((int)(order/3))*ROWHEIGHT + MARGIN;
    upperLeft.X = (order % 3)*COLWIDTH + MARGIN;
  }

  public Menu getPreviousMenu() {
    return this.previousMenu;
  }

  public string getIconLocation() {
    return this.icon;
  }

  public string getSelectedIconLocation() {
    return this.selectedIcon;
  }

  public int getOrder() {
    return this.order;
  }

  public bool setSelected(bool selected) {
    this.selected = selected;
  }

  public void draw(Graphics g) {
    if (this.selected and this.selectedIcon) {
      icon = new Bitmap(this.selectedIcon);
    } else if (this.icon) {
      icon = new Bitmap(this.icon);
    } else {
      //do something with text
    }
    g.DrawImage(icon, new RectangleF(upperLeft.X, upperLeft.Y, WIDTH, HEIGHT));
  }
}
