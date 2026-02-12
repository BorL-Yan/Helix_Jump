using UnityEngine;

public class MarkerAttachment 
{
    public void TieMarker(Collision collision,GameObject ballMarker)
    {       
        ballMarker.transform.SetParent(collision.transform);
    }
}
