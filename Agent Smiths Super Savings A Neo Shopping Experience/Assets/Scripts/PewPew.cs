using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HTC.UnityPlugin.Vive;
using UnityEngine;

public class PewPew : MonoBehaviour
{
  public bool VR = true;

  public HandRole HandRole = HandRole.RightHand;

  public GameObject ProjectilePrefab;

  public GameObject ShoppingList;
  private ShoppingList _shoppingList;

  public GameObject PewPewDisplay;
  private TextMesh _pewPewDisplay;

  private AudioSource _audioSource;

  public AudioClip LaserSound;
	public AudioClip RicochetSound;
	public AudioClip ChaChingSound;
	public AudioClip MooSound;

  void Start()
  {
    if (!ProjectilePrefab)
      Debug.LogError("Must set ProjectilePrefab on " + gameObject.name);

    if (!ShoppingList)
      Debug.LogError("Must set ShoppingList on " + gameObject.name);

    _shoppingList = ShoppingList.GetComponent<ShoppingList>();
    if (!_shoppingList)
      Debug.LogError(ShoppingList.name + " must have the ShoppingList script");

    if (!PewPewDisplay)
      Debug.LogError("Must set PewPewDisplay on " + gameObject.name);

    _pewPewDisplay = PewPewDisplay.GetComponent<TextMesh>();
    if (!_pewPewDisplay)
      Debug.LogError(PewPewDisplay.name + " must have a TextMesh");

    _pewPewDisplay.richText = true;

    _audioSource = GetComponent<AudioSource>();
    if (!_audioSource)
      Debug.LogError(gameObject.name + " must have an AudioSource");

    if (!LaserSound)
			Debug.LogError(gameObject.name + " must have a LaserSound");

		if (!RicochetSound)
			Debug.LogError(gameObject.name + " must have a RicochetSound");

    if (VR)
      enabled = SteamVR.enabled;
    else
      enabled = !SteamVR.enabled;

    UpdatePewPewText();
  }

  void Update()
  {
    var firing = false;
    if (VR)
      firing = ViveInput.GetPressDown(HandRole, ControllerButton.Trigger);
    else
      firing = Input.GetMouseButtonDown(0);

    var direction = VR ?
            Vector3.RotateTowards(transform.forward, transform.up * -1, Mathf.PI * 0.25f, 0) :
            transform.forward;

    Debug.DrawLine(transform.position, transform.position + direction);

    if (firing)
    {
      var hits = Physics.RaycastAll(transform.position, direction);
      var hitTarget = hits.Select(x => x.transform.GetComponent<Target>()).FirstOrDefault(x => x);
      Debug.Log(string.Format(
          "Raycast hit {0} objects ({1}) {2}",
          hits.Length,
          string.Join(",", hits.Select(x => x.transform.gameObject.name).ToArray()),
          hitTarget ? "got Target" : ""));

      _audioSource.PlayOneShot(LaserSound, 0.2f);

			if (hitTarget) {
				var destructor = hitTarget.GetComponent<TargetDestructor> ();

				if (destructor) {
					destructor.Destroy ();
				} else {
					Destroy (hitTarget.gameObject);
				}

				_shoppingList.Bought (hitTarget);
		    
				Debug.Log (string.Format ("Hit {0} ({1}).", hitTarget.name, hitTarget.ItemType));

				UpdatePewPewText ();

				_audioSource.PlayOneShot (RicochetSound, 0.3f);

				if (_shoppingList.WinnaWinnaChickenDinna) {
					_audioSource.PlayOneShot (ChaChingSound, 1.0f);
				}
			}
			else {
				var hitCreature = hits.Any (x => x.transform.gameObject.name == "Creature");
				if (hitCreature)
				{
					_audioSource.PlayOneShot (MooSound, 1.0f);
				}
			}

      var projectile = Instantiate(ProjectilePrefab);
      projectile.transform.position = transform.position;
      projectile.transform.LookAt(transform.position + direction, transform.up);

    }
  }

  void UpdatePewPewText()
  {
    var stringBuilder = new StringBuilder();

    stringBuilder.AppendLine(string.Format("Budget: {0}", _shoppingList.Budget.ToString("C")));
    stringBuilder.AppendLine(string.Format("Impulse: {0}", _shoppingList.ImpulseTotal.ToString("C")));
    stringBuilder.AppendLine(string.Format("\nWallet: {0}", _shoppingList.Wallet.ToString("C")));

    _pewPewDisplay.text = stringBuilder.ToString();
  }
}