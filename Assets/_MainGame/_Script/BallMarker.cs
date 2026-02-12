using UnityEngine;

public class BallMarker
{
    private GameObject _ballMarkerPrefab;
    public BallMarker(GameObject ballMarkerPrefab)
    {
        _ballMarkerPrefab = ballMarkerPrefab;
    }
    public GameObject AttachMarker(Collision collision)
    {
        Vector3 transform = collision.contacts[0].point;
        Quaternion rotation = Quaternion.LookRotation(collision.contacts[0].normal);
        GameObject marker = Object.Instantiate(_ballMarkerPrefab, transform, rotation);
        return marker;
    }
}
