using System.Collections.Generic;
using System.Windows;

// Menu will build a level of the menu tree
// given the name of the sub-menu
public class Menu {
  private bool leaf = false;
  public string name;

  public List<MenuItem> menuItems;

  public Menu(string name) {
    menuItems = new List<MenuItem>();
    if (name=="root") {
      menuItems.Add(new MenuItem("games", "games.jpg", "gamesselected.jpg", menuItems.Count, ""));
      menuItems.Add(new MenuItem("movies", "movies.jpg", "moviesselected.jpg", menuItems.Count, ""));
      menuItems.Add(new MenuItem("music", "music.jpg", "musicselected.jpg", menuItems.Count, ""));
      menuItems.Add(new MenuItem("apps", "apps.jpg", "appsselected.jpg", menuItems.Count, ""));
      menuItems.Add(new MenuItem("settings", "settings.jpg", "settingsselected.jpg", menuItems.Count, ""));
    } else if (name == "games") {
      menuItems.Add(new MenuItem("angry birds", "angrybirds.jpg", "angrybirdsselected.jpg", menuItems.Count, "root"));
      menuItems.Add(new MenuItem("carnival games", "carnivalgames.jpg", "carnivalgamesselected.jpg", menuItems.Count, "root"));
      menuItems.Add(new MenuItem("dance central", "dancecentral.jpg", "dancecentralselected.jpg", menuItems.Count, "root"));
      menuItems.Add(new MenuItem("kinectimals", "kinectimals.jpg", "kinectimalsselected.jpg", menuItems.Count, "rpot"));
      menuItems.Add(new MenuItem("kinect sports", "kinectsports.jpg", "kinectsportsselected.jpg", menuItems.Count, "root"));
      menuItems.Add(new MenuItem("kung fu panda", "kungfupanda.jpg", "kungfupandaselected.jpg", menuItems.Count, "root"));
      menuItems.Add(new MenuItem("back", "back.jpg", "backselected.jpg", menuItems.Count, "root"));
    } else if (name == "apps") {
      menuItems.Add(new MenuItem("aim", "aim.jpg", "aimselected.jpg", menuItems.Count, "root"));
      menuItems.Add(new MenuItem("facebook", "facebook.jpg", "facebookselected.jpg", menuItems.Count, "root"));
      menuItems.Add(new MenuItem("google+", "googleplus.jpg", "googleplusselected.jpg", menuItems.Count, "root"));
      menuItems.Add(new MenuItem("twitter", "twitter.jpg", "twitterselected.jpg", menuItems.Count, "root"));
      menuItems.Add(new MenuItem("yelp", "yelp.jpg", "yelpselected.jpg", menuItems.Count, "root"));
      menuItems.Add(new MenuItem("back", "back.jpg", "backselected.jpg", menuItems.Count, "root"));
    } else if (name == "movies") {
      menuItems.Add(new MenuItem("annie hall", "anniehall.jpg", "anniehallselected.jpg", menuItems.Count, "root"));
      menuItems.Add(new MenuItem("emperor's new groove", "emperorsnewgroove.jpg", "emperorsnewgrooveselected.jpg", menuItems.Count, "root"));
      menuItems.Add(new MenuItem("hitchhikers' guide", "hitchhikersguide.jpg", "hitchhikersguideselected.jpg", menuItems.Count, "root"));
      menuItems.Add(new MenuItem("old yeller", "oldyeller.jpg", "oldyellerselected.jpg", menuItems.Count, "root"));
      menuItems.Add(new MenuItem("the matrix", "thematrix.jpg", "thematrixselected.jpg", menuItems.Count, "root"));
      menuItems.Add(new MenuItem("back", "back.jpg", "backselected.jpg", menuItems.Count, "root"));
    } else if (name == "music") {
      menuItems.Add(new MenuItem("abbey road", "abbeyroad.jpg", "abbeyroadselected.jpg", menuItems.Count, "root"));
      menuItems.Add(new MenuItem("dark side of the moon", "darksideofthemoon.jpg", "darksideofthemoonselected.jpg", menuItems.Count, "root"));
      menuItems.Add(new MenuItem("nevermind", "nevermind.jpg", "nevermindselected.jpg", menuItems.Count, "root"));
      menuItems.Add(new MenuItem("rolling stones", "rollingstones.jpg", "rollingstonesselected.jpg", menuItems.Count, "root"));
      menuItems.Add(new MenuItem("back", "back.jpg", "backselected.jpg", menuItems.Count, "root"));
    } else if (name == "abbey road") {
      menuItems.Add(new MenuItem("Come Together", menuItems.Count));
      menuItems.Add(new MenuItem("Something", menuItems.Count));
      menuItems.Add(new MenuItem("Maxwells Silver Hammer", menuItems.Count));
      menuItems.Add(new MenuItem("Oh! Darling", menuItems.Count));
      menuItems.Add(new MenuItem("Octopus Garden", menuItems.Count));
      menuItems.Add(new MenuItem("I Want You", menuItems.Count));
      menuItems.Add(new MenuItem("back", "back.jpg", "backselected.jpg", menuItems.Count, "music"));
    } else if (name == "dark side of the moon") {
      menuItems.Add(new MenuItem("Speak to Me", menuItems.Count));
      menuItems.Add(new MenuItem("Breathe", menuItems.Count));
      menuItems.Add(new MenuItem("On the Run", menuItems.Count));
      menuItems.Add(new MenuItem("Money", menuItems.Count));
      menuItems.Add(new MenuItem("Any Colour You Like", menuItems.Count));
      menuItems.Add(new MenuItem("Brain Damage", menuItems.Count));
      menuItems.Add(new MenuItem("Eclipse", menuItems.Count));
      menuItems.Add(new MenuItem("back", "back.jpg", "backselected.jpg", menuItems.Count, "music"));
    } else if (name == "nevermind") {
      menuItems.Add(new MenuItem("Smells Like Teen Spirit", menuItems.Count));
      menuItems.Add(new MenuItem("In Bloom", menuItems.Count));
      menuItems.Add(new MenuItem("Come as You Are", menuItems.Count));
      menuItems.Add(new MenuItem("Lithium", menuItems.Count));
      menuItems.Add(new MenuItem("On a Plain", menuItems.Count));
      menuItems.Add(new MenuItem("Endless, Nameless", menuItems.Count));
      menuItems.Add(new MenuItem("back", "back.jpg", "backselected.jpg", menuItems.Count, "music"));
    } else if (name == "rolling stones") {
      menuItems.Add(new MenuItem("Rocks Off", menuItems.Count));
      menuItems.Add(new MenuItem("Shake Your Hips", menuItems.Count));
      menuItems.Add(new MenuItem("Casino Boogie", menuItems.Count));
      menuItems.Add(new MenuItem("Turd on the Run", menuItems.Count));
      menuItems.Add(new MenuItem("Let it Loose", menuItems.Count));
      menuItems.Add(new MenuItem("Soul Survivor", menuItems.Count));
      menuItems.Add(new MenuItem("back", "back.jpg", "backselected.jpg", menuItems.Count, "music"));
    } else {
        leaf = true;
    }
  }

  public bool isLeaf()
  {
      return leaf;
  }
}

// MenuItem gives us access to the positioning and 
// some properties of each menu items
public class MenuItem
{
  static int WIDTH = 226;
  static int HEIGHT = 160;

  static int ROWHEIGHT = 200;
  static int COLWIDTH = 266;

  public string name;
  public string icon = null;
  public string selectedIcon = null;
  int order;
  Point upperLeft;
  bool selected;
  public string previousMenu;

  public MenuItem(string name, int order) {
    this.name = name;
    this.order = order;
    this.selected = false;
    chooseDrawLocation();
  }

  public MenuItem(string name, string icon, string selectedIcon, 
      int order, string previousMenu) {
    this.name = name;
    this.icon = icon;
    this.selectedIcon = selectedIcon;
    this.order = order;
    this.previousMenu = previousMenu;
    this.selected = false;
    chooseDrawLocation();
  }

  public bool isIntersecting(Point p) {
    return ((p.X > upperLeft.X) && (p.X < upperLeft.X + WIDTH)) 
        && ((p.Y > upperLeft.Y) && (p.Y < upperLeft.Y + HEIGHT));
  }

  private void chooseDrawLocation() {
    // select a reasonable location to draw
    // based on the order in the menu list
    this.upperLeft = new Point();
    upperLeft.Y = ((int)(order/3))*ROWHEIGHT;
    upperLeft.X = (order % 3)*COLWIDTH;
  }

  public Point getUpperLeft()
  {
      return this.upperLeft;
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

}
