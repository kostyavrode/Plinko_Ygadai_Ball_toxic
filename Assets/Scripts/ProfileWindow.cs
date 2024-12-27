using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ProfileWindow : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject profileWindow;
    public Image profileImage;
    public TMP_InputField nicknameInput;
    public Button saveButton;

    private Texture2D capturedPhoto;

    private const string NicknameKey = "PlayerNickname";
    private const string PhotoKey = "PlayerPhoto";
    private const string ProfileSavedKey = "ProfileSaved";

    void Start()
    {
        if (PlayerPrefs.GetInt(ProfileSavedKey, 0) == 1)
        {
            profileWindow.SetActive(false);
            LoadProfileData();
            saveButton.interactable = true;
        }
        else
        {
            profileWindow.SetActive(true);
            saveButton.onClick.AddListener(SaveProfileData);
            saveButton.interactable = false;
            //TakePhoto();
        }
    }
    public void TakePhoto()
    {
        if (NativeCamera.IsCameraBusy())
        {
            Debug.LogWarning("Camera is busy!");
            return;
        }

        NativeCamera.Permission permission = NativeCamera.TakePicture((path) =>
        {
            if (!string.IsNullOrEmpty(path))
            {
                // Load the image as a Texture2D
                LoadImage(path);
            }
        }, maxSize: 1024);

        if (permission != NativeCamera.Permission.Granted)
        {
            Debug.LogWarning("Camera permission not granted!");
        }
        saveButton.interactable = true;
    }

    void LoadImage(string path)
    {
        Texture2D tempTexture = NativeCamera.LoadImageAtPath(path, 1024, false);
        if (tempTexture != null)
        {
            if (capturedPhoto != null)
            {
                Destroy(capturedPhoto);
            }

            capturedPhoto = tempTexture;

            // Convert the Texture2D to a Sprite
            Rect rect = new Rect(0, 0, capturedPhoto.width, capturedPhoto.height);
            Sprite photoSprite = Sprite.Create(capturedPhoto, rect, new Vector2(0.5f, 0.5f));
            profileImage.sprite = photoSprite;
        }
        else
        {
            Debug.LogError("Failed to load image from path: " + path);
        }
    }

    void SaveProfileData()
    {
        string nickname = nicknameInput.text;

        if (!string.IsNullOrEmpty(nickname) && capturedPhoto != null)
        {
            // Save nickname
            PlayerPrefs.SetString(NicknameKey, nickname);

            // Save photo as a Base64 string
            byte[] photoBytes = capturedPhoto.EncodeToPNG();
            string photoBase64 = System.Convert.ToBase64String(photoBytes);
            PlayerPrefs.SetString(PhotoKey, photoBase64);

            // Mark profile as saved
            PlayerPrefs.SetInt(ProfileSavedKey, 1);

            // Close the profile window
            profileWindow.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Nickname or photo is missing!");
        }
    }

    void LoadProfileData()
    {
        string nickname = PlayerPrefs.GetString(NicknameKey, "Player");
        string photoBase64 = PlayerPrefs.GetString(PhotoKey, "");

        nicknameInput.text = nickname;

        if (!string.IsNullOrEmpty(photoBase64))
        {
            byte[] photoBytes = System.Convert.FromBase64String(photoBase64);
            Texture2D photoTexture = new Texture2D(2, 2);
            photoTexture.LoadImage(photoBytes);

            Rect rect = new Rect(0, 0, photoTexture.width, photoTexture.height);
            Sprite photoSprite = Sprite.Create(photoTexture, rect, new Vector2(0.5f, 0.5f));
            profileImage.sprite = photoSprite;
        }
    }
}
