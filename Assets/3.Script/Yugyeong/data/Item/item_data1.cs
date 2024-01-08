public class item_data
{
    public string player_id;
    public int key_num;
    public int have_fishingrod;
    public int food_num;
    public int money;

    public item_data(string id, int keynum, int havefishingrod, int foodnum, int money_)
    {
        player_id = id;
        key_num = keynum;
        have_fishingrod = havefishingrod;
        food_num = foodnum;
        money = money_;
    }
}
