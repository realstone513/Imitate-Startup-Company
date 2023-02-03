using System;
using TMPro;
using UnityEngine;

public class JobOffer : GenericWindow
{
    public TextMeshProUGUI recruitCostText;

    public void Test(Int32 num)
    {
        switch (num)
        {
            case 0: // Beginner
                recruitCostText.text = "10";
                break;
            case 1: // Intermediate
                recruitCostText.text = "500";
                break; 
            case 2: // Expert
                recruitCostText.text = "3000";
                break;
        }
    }
}