using System;
using UnityEngine;
using System.Collections;

public class Champagne : MonoBehaviour
{
	public AudioClip drink;

  public float drunkenness = 0;
  public FloorArea floor;

	public bool doNonsense() {
		return UnityEngine.Random.Range (0.0f, 1.0f) < drunkenness;
	}

  static void ChampagneForEveryone ()
  {
    
  }

  void Start ()
  {
    style = Camera.main.GetComponent<GeneralRessources> ().Style;
    content = new GUIContent ("Drink up!", GlassFull, "Empty your glass to evade any questions.");
  }

  #region GUI

  Matrix4x4 oldMatrix;
  Rect buttonPosition = new Rect (1110f, 550f, 150f, 150f);
  GUISkin style;
  public Texture2D GlassFull;
  public Texture2D GlassEmpty;
  GUIContent content;

  void OnGUI ()
  {
    GUI.skin = style;
    oldMatrix = GUI.matrix;
    GUI.matrix = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, GeneralRessources.Scale);

    if (GUI.Button (buttonPosition, content, style.customStyles [0])) {
      drunkenness += 0.1f;

      // Kill all challenges
      foreach (Challenge ch in FindObjectsOfType<Challenge> ()) {
        ArtObject ao = GalleryVisitor.GetClosestArtwork (transform.position);
        if (ch.victim == gameObject.GetComponent<GalleryVisitor> () && ch.topic == ao) {
          Destroy (ch.gameObject);
        }
      }

      // Go to another place
      
		if (drink != null) { GetComponent<AudioSource> ().PlayOneShot (drink); }
		GetComponentInChildren<Animator> ().SetTrigger ("pDrink");
			GetComponent<ParticleSystem> ().Play ();
		StartCoroutine (Teleport ());
    }

    GUI.matrix = oldMatrix;
  }

	IEnumerator Teleport() {
		yield return new WaitForSeconds(1.3f);
		Vector3 goTo = floor.GetRandomPositionOnFloor ();
		iTween.MoveTo (this.gameObject, goTo, 2f);
	}

  #endregion

}

