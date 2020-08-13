using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team
{
    public ArenaGameDetails.TeamColor teamColor;
    public List<GameParticipant> teamMembers;
    public int score;
    public Team(ArenaGameDetails.TeamColor teamColor)
    {
        this.teamColor = teamColor;
        teamMembers = new List<GameParticipant>();
        score = 0;
    }
    public void addMember(GameParticipant teamMember)
    {
        teamMembers.Add(teamMember);
    }

    int getTeamScore()
    {

        return score;
    }
    void setTeamScore()
    {
        score = 0;
        for (int i = 0; i < teamMembers.Count; i++)
        {
            score += teamMembers[i].score;
        }

    }

}

