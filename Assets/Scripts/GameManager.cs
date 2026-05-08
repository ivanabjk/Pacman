using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private InputSystem_Actions controls;
    public Ghost[] ghosts;
    public Pacman pacman;
    public Transform pellets;
    public int ghostMultiplier { get; private set; } = 1;
    public int score { get; private set; } = 0;
    public int lives { get; private set; } = 3;

    public Image[] lifeIcons;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI youWinText;
    public Boolean isGameWon = false;

    private void Awake()
    {
        controls = new InputSystem_Actions();
    }
    private void Start()
    {
        NewGame();
    }
    private void NewGame()
    {
        isGameWon = false;
        SetScore(0);
        SetLives(3);
        NewRound();
    }
    private void Update()
    {
        if (lives <= 0 && Keyboard.current.anyKey.wasPressedThisFrame)
        {
            Invoke(nameof(NewGame), 1f);
            // NewGame();
        }
        if (isGameWon && Keyboard.current.anyKey.wasPressedThisFrame)
        {
            Invoke(nameof(NewGame), 1f);
        }
    }
    private void NewRound()
    {
        gameOverText.enabled = false;
        youWinText.enabled = false;

        foreach (Transform pellet in this.pellets)
        {
            pellet.gameObject.SetActive(true);
        }
        ResetState();
    }
    private void ResetState()
    {
        ResetGhostMultiplier();
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].ResetState();
        }
        this.pacman.ResetState();
    }
    private void GameOver()
    {
        gameOverText.enabled = true;

        // for (int i = 0; i < this.ghosts.Length; i++)
        // {
        //     this.ghosts[i].gameObject.SetActive(false);
        // }

        pacman.DeathSequence();
        // this.pacman.gameObject.SetActive(false);
    }
    private void YouWin()
    {
        isGameWon = true;
        youWinText.enabled = true;
        this.pacman.gameObject.SetActive(false);

    }
    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = "Score: " + score.ToString().PadLeft(2, '0');
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
        for (int i = 0; i < lifeIcons.Length; i++)
        {
            lifeIcons[i].enabled = i < this.lives;
        }
    }

    public void GhostEaten(Ghost ghost)
    {
        int points = ghost.points * this.ghostMultiplier;
        SetScore(this.score + points);
        this.ghostMultiplier++;
    }
    public void PacmanEaten()
    {
        pacman.DeathSequence();

        // this.pacman.gameObject.SetActive(false);

        SetLives(this.lives - 1);

        if (this.lives > 0)
        {
            Invoke(nameof(ResetState), 3f);
        }
        else
        {
            GameOver();
        }
    }

    public void PelletEaten(Pellet pellet)
    {
        pellet.gameObject.SetActive(false);
        SetScore(this.score + pellet.points);

        if (!HasRemainingPellets())
        {
            YouWin();
        }
    }
    public void PowerPelletEaten(PowerPellet pellet)
    {
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].frightened.Enable(pellet.duration);
        }

        PelletEaten(pellet);
        CancelInvoke(nameof(ResetGhostMultiplier));
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);

    }
    private bool HasRemainingPellets()
    {
        foreach (Transform pellet in this.pellets)
        {
            if (pellet.gameObject.activeSelf)
            {
                return true;
            }
        }
        return false;
    }

    private void ResetGhostMultiplier()
    {
        this.ghostMultiplier = 1;
    }

}
