using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombLobba : Equipment, ReceivesEvents
{

    private AbilityHandler handler;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void equip(Unit host) {
        this.host = host;
        this.host.health += 2;
        this.host.max_health += 2;
        this.host.range += 2;
        this.host.power += 2;
    }

    public void subscribe(AbilityHandler handler) {
        this.handler = handler;
        this.handler.add_subscriber("Attack", this);
    }

    public void unsubscribe() {
        handler.remove_subscriber("Attack", this);
    }

    public Event receive_event(Event data) {
        if (data is Attack) {
            Attack casted_data = data as Attack;
            if (casted_data.unit == host) {
                List<Unit> splash_targets = new List<Unit>();

                if (casted_data.target.board_loc[0] - 1 >= 0) {
                    splash_targets.Add(host.game.board[casted_data.target.board_loc[0] - 1][casted_data.target.board_loc[1]]);
                }
                if (casted_data.target.board_loc[0] + 1 <= 2) {
                    splash_targets.Add(host.game.board[casted_data.target.board_loc[0] + 1][casted_data.target.board_loc[1]]);
                }
                if (casted_data.target.board_loc[1] - 1 >= 0) {
                    splash_targets.Add(host.game.board[casted_data.target.board_loc[0]][casted_data.target.board_loc[1] - 1]);
                }
                if (casted_data.target.board_loc[1] + 1 <= 6) {
                    splash_targets.Add(host.game.board[casted_data.target.board_loc[0]][casted_data.target.board_loc[1] + 1]);
                }

                foreach (Unit splash_target in splash_targets) {
                    splash_target.set_health(splash_target.health - 2);
                }
            }
        }
        return data;
    }
}
