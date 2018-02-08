using System;

[Serializable]
public class Data {

    public int moneyWeHave;
    public int flourQuantity;
    public int eggQuantity;
    public int sugarQuantity;
    public int milkQuantity;
    public int saltQuantity;
    public int butterQuantity;

    public int tableLevel;
    public int kitchenLevel;

    public int recipeQuantity;
    public int recipeRead;

    public int tutstate;
    public bool unreadMail;
    public bool enoughIngredients;

    public int score;
}

public static class Pancake {
    public static int eggs = 2;
    public static int flour = 2;
    public static int sugar = 2;
    public static int milk = 1;
    public static int salt = 1;
    public static int butter = 1;
}