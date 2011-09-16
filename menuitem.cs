
using System.Collections.Generic;
using System.Windows;

class Menu {
  bool root;
  string name;

  List<MenuItem> menuItems;

  public Menu(string name) {
    menuItems = new List<MenuItem>();
    if (name=="root") {
      root = true;
      menuItems.Add(new MenuItem("games", "games.jpg", "gamesselected.jpg", menuItems.Count, this));
      menuItems.Add(new MenuItem("movies", "movies.jpg", "moviesselected.jpg", menuItems.Count, this));
      menuItems.Add(new MenuItem("music", "music.jpg", "musicselected.jpg", menuItems.Count, this));
      menuItems.Add(new MenuItem("apps", "apps.jpg", "appsselected.jpg", menuItems.Count, this));
      menuItems.Add(new MenuItem("settings", "settings.jpg", "settingsselected.jpg", menuItems.Count, this));
    } else if (name == "games") {
      menuItems.Add(new MenuItem("angrybirds", "angrybirds.jpg", "angrybirdsselected.jpg", menuItems.Count, this));
      menuItems.Add(new MenuItem("carnivalgames", "carnivalgames.jpg", "carnivalgamesselected.jpg", menuItems.Count, this));
      menuItems.Add(new MenuItem("dancecentral", "dancecentral.jpg", "dancecentralselected.jpg", menuItems.Count, this));
      menuItems.Add(new MenuItem("kinectimals", "kinectimals.jpg", "kinectimalsselected.jpg", menuItems.Count, this));
      menuItems.Add(new MenuItem("kinectsports", "kinectsports.jpg", "kinectsportsselected.jpg", menuItems.Count, this));
      menuItems.Add(new MenuItem("kungfupanda", "kungfupanda.jpg", "kungfupandaselected.jpg", menuItems.Count, this));
    } else if (name == "apps") {
      menuItems.Add(new MenuItem("aim", "aim.jpg", "aimselected.jpg", menuItems.Count, this));
      menuItems.Add(new MenuItem("facebook", "facebook.jpg", "facebookselected.jpg", menuItems.Count, this));
      menuItems.Add(new MenuItem("googleplus", "googleplus.jpg", "googleplusselected.jpg", menuItems.Count, this));
      menuItems.Add(new MenuItem("twitter", "twitter.jpg", "twitterselected.jpg", menuItems.Count, this));
      menuItems.Add(new MenuItem("yelp", "yelp.jpg", "yelpselected.jpg", menuItems.Count, this));
    } else if (name == "movies") {
      menuItems.Add(new MenuItem("anniehall", "anniehall.jpg", "anniehallselected.jpg", menuItems.Count, this));
      menuItems.Add(new MenuItem("emperorsnewgroove", "emperorsnewgroove.jpg", "emperorsnewgrooveselected.jpg", menuItems.Count, this));
      menuItems.Add(new MenuItem("hitchhikersguide", "hitchhikersguide.jpg", "hitchhikersguideselected.jpg", menuItems.Count, this));
      menuItems.Add(new MenuItem("oldyeller", "oldyeller.jpg", "oldyellerselected.jpg", menuItems.Count, this));
      menuItems.Add(new MenuItem("thematrix", "thematrix.jpg", "thematrixselected.jpg", menuItems.Count, this));
    } else if (name == "music") {
      menuItems.Add(new MenuItem("abbeyroad", "abbeyroad.jpg", "abbeyroadselected.jpg", menuItems.Count, this));
      menuItems.Add(new MenuItem("darksideofthemoon", "darksideofthemoon.jpg", "darksideofthemoonselected.jpg", menuItems.Count, this));
      menuItems.Add(new MenuItem("nevermind", "nevermind.jpg", "nevermindselected.jpg", menuItems.Count, this));
      menuItems.Add(new MenuItem("rollingstones", "rollingstones.jpg", "rollingstonesselected.jpg", menuItems.Count, this));
    } else if (name == "abbeyroad") {
      menuItems.Add(new MenuItem("Come Together", menuItems.Count));
      menuItems.Add(new MenuItem("Something", menuItems.Count));
      menuItems.Add(new MenuItem("Maxwells Silver Hammer", menuItems.Count));
      menuItems.Add(new MenuItem("Oh! Darling", menuItems.Count));
      menuItems.Add(new MenuItem("Octopus Garden", menuItems.Count));
      menuItems.Add(new MenuItem("I Want You", menuItems.Count));
    } else if (name == "darksideofthemoon") {
      menuItems.Add(new MenuItem("Speak to Me", menuItems.Count));
      menuItems.Add(new MenuItem("Breathe", menuItems.Count));
      menuItems.Add(new MenuItem("On the Run", menuItems.Count));
      menuItems.Add(new MenuItem("Money", menuItems.Count));
      menuItems.Add(new MenuItem("Any Colour You Like", menuItems.Count));
      menuItems.Add(new MenuItem("Brain Damage", menuItems.Count));
      menuItems.Add(new MenuItem("Eclipse", menuItems.Count));
    } else if (name == "nevermind") {
      menuItems.Add(new MenuItem("Smells Like Teen Spirit", menuItems.Count));
      menuItems.Add(new MenuItem("In Bloom", menuItems.Count));
      menuItems.Add(new MenuItem("Come as You Are", menuItems.Count));
      menuItems.Add(new MenuItem("Lithium", menuItems.Count));
      menuItems.Add(new MenuItem("On a Plain", menuItems.Count));
      menuItems.Add(new MenuItem("Endless, Nameless", menuItems.Count));
    } else if (name == "rollingstones") {
      menuItems.Add(new MenuItem("Rocks Off", menuItems.Count));
      menuItems.Add(new MenuItem("Shake Your Hips", menuItems.Count));
      menuItems.Add(new MenuItem("Casino Boogie", menuItems.Count));
      menuItems.Add(new MenuItem("Turd on the Run", menuItems.Count));
      menuItems.Add(new MenuItem("Let it Loose", menuItems.Count));
      menuItems.Add(new MenuItem("Soul Survivor", menuItems.Count));
    } else {
      // huh?
    }

    if (!root) {
      MenuItem backButton = new MenuItem("back", "back.jpg", "backselected.jpg", menuItems.Count, this);
      menuItems.Add(backButton);
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
    chooseDrawLocation();
  }

  public MenuItem(string name, string icon, string selectedIcon, int order, Menu previousMenu) {
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

  public void setSelected(bool selected) {
    this.selected = selected;
  }

  /*public void draw(Graphics g) {
    if (this.selected && this.selectedIcon) {
      icon = new Bitmap(this.selectedIcon);
    } else if (this.icon) {
      icon = new Bitmap(this.icon);
    } else {
      //do something with text
    }
    g.DrawImage(icon, new RectangleF(upperLeft.X, upperLeft.Y, WIDTH, HEIGHT));
  }*/
}
